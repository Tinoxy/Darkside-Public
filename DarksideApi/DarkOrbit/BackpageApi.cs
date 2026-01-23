using DarksideApi.DarkOrbit.Commands.Client;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.HttpWeb;
using DarksideApi.Util;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TwoCaptcha.Captcha;
using HttpClient = DarksideApi.HttpWeb.HttpClient;

namespace DarksideApi.DarkOrbit
{
    public class BackpageApi
    {
        private readonly Api api;
        private readonly HttpClient httpClient;
        public Dictionary<int, System.Net.IPEndPoint> IpAdresses { get; private set; }
        public HttpClient Client => this.httpClient;

        public BackpageApi(Api api)
        {
            this.api = api;
            this.httpClient = new(true, api.Settings.IsUnity);
            this.IpAdresses = [];
        }

        public BackpageApi() { }

        public void ShutDown()
        {
            this.httpClient.Shutdown();
        }

        public void UpdateUserAgent()
        {
            this.httpClient.UpdateUserAgent(true, this.api.Settings.IsUnity);
        }

        public void UpdateProxy(WebProxy? proxy)
        {
            if (proxy == null)
                return;

            this.httpClient.UpdateProxy(proxy);
        }

        public string GetBaseUrl()
        {
            return $"https://{this.api.Hero.ServerTag}.darkorbit.com/";
        }

        public string GetSessionIdUrl()
        {
            return $"{this.GetBaseUrl()}/?dosid={this.api.Hero.SessionId}";
        }

        public string GetHomeUrl()
        {
            return $"{this.GetBaseUrl()}indexInternal.es?action=internalStart";
        }

        /// <summary>
        /// Login on the Darkorbit website.
        /// </summary>
        /// <param name="language">The language in which to load the page.</param>
        /// <returns></returns>
        public bool Login(string language = "en")
        {
            try
            {
                //https://gbl2.darkorbit.com/unityApi/login.php 
                var homeUrl = "https://www.darkorbit.com/?locale=" + language;
                var loginResult = this.httpClient.Request(RequestBuilder.Empty, homeUrl);
                var loginUrl = Regex.Match(loginResult, "<form name=\"bgcdw_login_form\" method=\"post\" class=\"bgcdw_login_form\" action=\"(.*?)\">").Groups[1].Value.Replace("amp;", "");
                var solvedCaptcha = string.Empty;

                RequestBuilder loginRequest = new(RequestMethod.Post);
                loginRequest.Add("username", this.api.Settings.Username);
                loginRequest.Add("password", this.api.Settings.Password);

                if (string.IsNullOrEmpty(loginUrl))
                {
                    this.api.WriteLog("Could not load Darkorbit Homepage...");
                    this.api.GameClient.GameState = (this.api.Settings.AutoLogin || this.api.Settings.IsRunning) ? GameState.Disconnected : GameState.LoginFailed;
                    return false;
                }

                if (loginResult.Contains("id=\"recaptcha_image\"") && !string.IsNullOrEmpty(solvedCaptcha = this.SolveCaptcha(homeUrl)))
                {
                    loginRequest.Add("g-recaptcha-response", solvedCaptcha);
                    loginRequest.Add("h-captcha-response", solvedCaptcha);
                }

                var loginResponse = this.httpClient.Request(loginRequest, loginUrl);

                return this.HandleLoginResponse(loginResponse);
            }
            catch (Exception ex)
            {
                this.api.Logging.Log(ex);
            }
            return false;
        }

        public bool LoginSessionId(string serverTag)
        {
            var internalHome = $"https://{serverTag}.darkorbit.com/indexInternal.es?action=internalStart&prc=100";
            RequestBuilder requestBuilder = new(RequestMethod.Post);
            requestBuilder.Add("dosid", this.api.Settings.SessionId);
            var loginResponse = this.httpClient.Request(requestBuilder, internalHome);
            return this.HandleLoginResponse(loginResponse);
        }

        private string SolveCaptcha(string originalUrl = "www.darkorbit.com&")
        {
            try
            {
                if (string.IsNullOrEmpty(this.api.Settings.TwoCaptchaApiKey))
                {
                    return string.Empty;
                }

                TwoCaptcha.TwoCaptcha solver = new(this.api.Settings.TwoCaptchaApiKey);
                HCaptcha captcha = new();
                captcha.SetSiteKey("fba93119-6a3d-4fbf-8de3-e0d9d2948c45");
                captcha.SetUrl($"https://www.darkorbit.com/?originalURL={originalUrl}");

                try
                {
                    solver.Solve(captcha).Wait();
                }
                catch (Exception ex)
                {
                    this.api.Logging.Log(ex);
                }

                return captcha.Code;
            }
            catch (Exception ex)
            {
                this.api.Logging.Log(ex);
            }
            return string.Empty;
        }

        private bool HandleLoginResponse(string loginResponse)
        {
            loginResponse = this.CheckForPolicyUpdate(loginResponse);
            if (loginResponse.Contains("bgcdw_errors_all bgc"))
            {
                var errorMsg = HelpTools.Between(loginResponse, "bgcdw_errors_all bgc", "bgcdw_errors_all_close");
                errorMsg = HttpUtility.UrlDecode(errorMsg.Replace("<div class=\"", "")
                                    .Replace("<ul>", "")
                                    .Replace("</ul>", "")
                                    .Replace("</li>", "")
                                    .Replace("<li style=\"margin-bottom: 0px;\">", "")
                                    .Replace("\">", "")
                                    .Replace("<li>", ""));
                this.api.WriteLog(errorMsg);
                this.api.GameClient.GameState = GameState.LoginFailed;
                return false;
            }
            if (!loginResponse.Contains("id=\"pilotAvatar")
                && !loginResponse.Contains("serverSelection")
                && loginResponse.Length != 32)
            {
                this.api.WriteLog("Could not login! Check Username & Password!");
                this.api.GameClient.GameState = GameState.LoginFailed;
                this.api.Logging.Log(loginResponse);
                return false;
            }

            this.api.Hero.ServerTag = HelpTools.Between(this.httpClient.CurrentURL, "://", ".darkorbit");

            if (string.IsNullOrEmpty(this.api.Hero.ServerTag))
            {
                this.api.WriteLog("Could not get Server! Something went wrong!");
                return false;
            }

            this.api.Hero.SessionId = this.httpClient.Cookies.GetAllCookies()
                                                   .FirstOrDefault(c => c.Name == "dosid")?.Value!;
            if (loginResponse.Contains("do_img/global/bg_standard_"))
            {
                var factionName = HelpTools.Between(loginResponse, "do_img/global/bg_standard_", ".jpg?").Trim();
                this.api.Hero.FactionId = factionName == "mmo" ? 1 : factionName == "eic" ? 2 : factionName == "vru" ? 3 : 0;
            }

            return true;
        }

        private string CheckForPolicyUpdate(string loginResponse)
        {
            if (!loginResponse.Contains("id=\"bgc_policy_accept_form\""))
            {
                return loginResponse;
            }
            var policyAcceptUrl = Regex.Match(loginResponse, @"<form .*? action=""(.*?)""").Groups[1].Value.Replace("amp;", "");
            Match languageMatch = Regex.Match(loginResponse, @"<input type=""hidden"" name=""language"" value=""(.*?)""");
            Match geoCountryMatch = Regex.Match(loginResponse, @"<input type=""hidden"" name=""geoCountry"" value=""(.*?)""");
            Match affiliateIdMatch = Regex.Match(loginResponse, @"<input type=""hidden"" name=""affiliateId"" value=""(.*?)""");
            Match acceptMatch = Regex.Match(loginResponse, @"<input type=""hidden"" name=""accept"" value=""(.*?)""");

            var formData = new RequestBuilder(RequestMethod.Post);
            formData.Add("language", languageMatch.Success ? languageMatch.Groups[1].Value : "cs");
            formData.Add("geoCountry", geoCountryMatch.Success ? geoCountryMatch.Groups[1].Value : "US");
            formData.Add("affiliateId", affiliateIdMatch.Success ? affiliateIdMatch.Groups[1].Value : "821");
            formData.Add("accept", acceptMatch.Success ? acceptMatch.Groups[1].Value : "1");

            this.api.WriteLog("Accepting new TOS...");

            return this.httpClient.Request(formData, policyAcceptUrl);
        }

        /// <summary>
        /// Collects the daily Login Bonus.
        /// </summary>
        public bool CollectDailyLoginBonus()
        {
            if (this.api.Settings.IsUnity)
            {
                return false;
            }
            RequestBuilder requestBuilder = new(RequestMethod.Get);
            Dictionary<string, string> headers = [];

            headers.Add("accept", "*/*");
            headers.Add("accept-encoding", "gzip, deflate, br");
            headers.Add("accept-language", "en");
            headers.Add("Host", $"{this.api.Hero.ServerTag}.darkorbit.com");
            headers.Add("referer", $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalMapRevolution");
            headers.Add("referrerPolicy", "no-referrer-when-downgrade");
            headers.Add("sec-fetch-dest", "embed");
            headers.Add("sec-fetch-mode", "no-cors");
            headers.Add("sec-fetch-site", "cross-site");
            headers.Add("x-requested-with", "ShockwaveFlash/11.2.999.999");

            var response = this.httpClient.Request(requestBuilder, $"https://{this.api.Hero.ServerTag}.darkorbit.com/flashAPI/dailyLogin.php", headers);

            return response.Contains("remainingSeconds");
        }

        /// <summary>
        /// Get's the user info to login on map.
        /// </summary>
        public bool GetServer()
        {
            var baseUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/unityApi/login.php";
            RequestBuilder requestBuilder = new(RequestMethod.Get);
            if (!this.api.Settings.IsUnity)
            {
                requestBuilder.Add("action", "internalMapRevolution");
                baseUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es";
            }

            var mapResponse = this.httpClient.Request(requestBuilder, baseUrl);

            Debug.WriteLine(mapResponse);

            if (!(this.api.Settings.IsUnity ? this.ParseUnityVars(mapResponse) : this.ParseFlashVars(mapResponse)))
            {
                this.api.WriteLog("Could not load Server, Received data length was invalid!");
                this.api.Logging.Log("Error Parsing Map Data: " + mapResponse);
                return false;
            }

            this.IpAdresses = HelpTools.LoadIpAdresses(this.api.Hero.ServerTag!);
            if (Api.StarSystem.Count <= 0)
            {
                Api.StarSystem = HelpTools.LoadGateData(this.api);
                api.Logging.Log("Starsystem loaded...");
            }

            this.api.GameClient.IpAdress = this.IpAdresses[this.api.Map.Id].Address.ToString();
            this.api.GameClient.Port = this.IpAdresses[this.api.Map.Id].Port;
            api.Logging.Log("Found IP: " + this.api.GameClient.IpAdress + " & Port: " + this.api.GameClient.Port);

            //var chatHost = HelpTools.Between(flashVars, "chatHost\"", "\",\"cdn");
            //var chatIp = chatHost.Split(':')[0];
            //var chatPort = chatHost.Split(":")[1];

            this.api.WriteLog($"Connecting to: {this.api.GameClient.IpAdress}:{this.api.GameClient.Port}");

            return true;
        }

        private bool ParseUnityVars(string mapResponse)
        {
            var unityVars = HelpTools.Between(mapResponse, "data\":\"{", "}\"}");
            var hangarVars = HelpTools.Between(mapResponse, "{\\\"hangars\\\":[{\\\"", "\\\"}],");
            var hangars = hangarVars.Split("\\\"},");

            if (unityVars.Length < 1)
            {
                return false;
            }

            this.api.Logging.Log("Unit Vars received:\n" + unityVars);

            this.api.Hero.SessionId = HelpTools.Between(unityVars, "sessionID\\\":\\\"", "\\\",\\\"basePath");

            ReadyRequest.IsTwoDimensional = int.Parse(HelpTools.Between(unityVars, "display2d\\\":", ",\\\"theme")) != 0;
            //","newUser 
            var mapId = HelpTools.Between(unityVars, "mapID\\\":\\\"", "\\\"hangars");

            if (mapId.Contains("newUser"))
            {
                this.api.Logging.Log("Found NEW USER: " + mapId);
                mapId = mapId.Replace(",\\\"newUser", "");
                if (mapId.Contains(':'))
                {
                    mapId = mapId.Split(':').ElementAt(0);
                }
            }
            mapId = mapId.Replace("\\\"", "").Replace(",", "");
            this.api.Map = new(int.Parse(mapId), 0, 0);
            this.api.Hero.UserId = int.Parse(HelpTools.Between(unityVars, "userID\\\":\\\"", "\\\",\\\"session"));

            api.Logging.Log("UserId set: " + api.Hero.UserId);

            if (unityVars.Contains("faction"))
            {
                var faction = HelpTools.Between(unityVars, "\"faction\\\":\\\"", "\\\"}},\\\"serverInfo");
                this.api.Hero.FactionId = faction == "mmo" ? 1 : faction == "eic" ? 2 : faction == "vru" ? 3 : 4;
                api.Logging.Log("FactionId set on Login: " + faction);
            }

            if (!HelpTools.TryBetween(unityVars, "pid\\\":\\\"", "\\\",\\\"opid", out var result, api.Logging) || !int.TryParse(result, out var pid))
            {
                this.api.Logging.Log("Could not parse InstanceId -> Trying secondary InstanceId");
                if (!HelpTools.TryBetween(unityVars, "\\\"pid\\\":", ",\\\"opid", out result, api.Logging) || !int.TryParse(result, out pid))
                {
                    this.api.Logging.Log("Could not parse InstanceId -> Trying Original InstanceId");
                    if (!HelpTools.TryBetween(unityVars, "opid\\\":\\\"", "\\\",\\\"board", out result, api.Logging) || !int.TryParse(result, out pid))
                    {
                        this.api.Logging.Log("Could not parse InstanceId -> default = 0!");
                        pid = 0;
                    }
                }
            }

            this.api.Hero.InstanceId = pid; 
            this.api.Logging.Log("InstanceId set: " + this.api.Hero.InstanceId);
            
            api.Logging.Log("Unity VARs parsed...");

            var hangarsList = new List<Hangar>();
            foreach (var hangar in hangars)
            {
                var hangarId = int.Parse(HelpTools.Between(hangar, "\\\"hangarId\\\":\\\"", "\\\",\\\"shipName"));
                var shipName = HelpTools.Between(hangar, "shipName\\\":\\\"", "\\\",\\\"mapId");
                var isActive = bool.Parse(HelpTools.Between(hangar, "hangarActive\\\":", ",\\\"tooltip"));
                var shipMapId = int.Parse(HelpTools.Between(hangar, "mapId\\\":\\\"", "\\\",\\\"ship"));
                var hangarObj = new Hangar(hangarId, isActive, shipName, isActive, shipMapId);

                hangarsList.Add(hangarObj);
            }

            api.Logging.Log("Unity Hangars parsed...");
            this.api.AddHangar(hangarsList);

            return true;
        }

        private bool ParseFlashVars(string mapResponse)
        {
            var flashVars = HelpTools.Between(mapResponse, "flashembed(\"container\"", "\"});");

            if (flashVars.Length < 1)
            {
                return false;
            }

            this.api.Hero.SessionId = HelpTools.Between(flashVars, "sessionID\": \"", "\",\"basePath\"");

            ReadyRequest.IsTwoDimensional = int.Parse(HelpTools.Between(flashVars, "display2d\": \"", "\",\"theme")) != 0;
            //","newUser
            var mapId = HelpTools.Between(flashVars, "mapID\": \"", "\",\"allowChat");
            if (mapId.Contains("newUser"))
            {
                mapId = mapId.Remove(mapId.IndexOf("newUser") - 3);
            }
            this.api.Map = new(int.Parse(mapId), 0, 0);
            this.api.Hero.UserId = int.Parse(HelpTools.Between(flashVars, "userID\": \"", "\",\"session"));
            if (flashVars.Contains("faction"))
                this.api.Hero.FactionId = int.Parse(HelpTools.Between(flashVars, "factionID\": \"", "\",\""));

            this.api.Hero.InstanceId = int.Parse(HelpTools.Between(flashVars, "pid\": \"", "\",\"board"));
            return true;
        }

        /// <summary>
        /// Checks if the current SessionId is Valid.
        /// </summary>
        /// <returns>True if Session is Valid, otherwise false</returns>
        public bool IsSessionValid(bool logout = false)
        {
            if (string.IsNullOrEmpty(this.api.Hero.SessionId))
            {
                return false;
            }
            var baseUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/unityApi/login.php";
            RequestBuilder requestBuilder = new(RequestMethod.Get);
            if (!this.api.Settings.IsUnity)
            {
                requestBuilder.Add("action", logout ? "externalHome" : "internalStart");
                baseUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es";
            }

            var homeResponse = this.httpClient.Request(requestBuilder, baseUrl);

            return homeResponse.Contains("id=\"pilotAvatar") || homeResponse.Contains("sessionID");
        }

        public void GetInventory()
        {
            if (this.api.Settings.IsUnity)
            {
                return;
            }

            this.api.Logging.Log("Requesting Hangars...");

            var inventoryUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/flashAPI/inventory.php";

            RequestBuilder requestBuilder = new(RequestMethod.Post);
            requestBuilder.Add("Referer", "https://darkorbit-22.bpsecure.com/swf_global/inventory/inventory.swf?__cv=4825d57d6eaaa9a42440acf6299c8200");
            requestBuilder.Add("X-Requested-With", "ShockwaveFlash/11.2.999.999");
            requestBuilder.Add("action", "getHangarList");
            requestBuilder.Add("params", "e30=");

            var response = string.Empty;
            var hangarResponse = string.Empty;
            try
            {
                response = this.httpClient.Request(requestBuilder, inventoryUrl);
                hangarResponse = Encoding.UTF8.GetString(Convert.FromBase64String(response));
            }
            catch (Exception ex)
            {
                this.api.Logging.Log(ex);
            }

            this.HandleHangarResponse(hangarResponse);
        }

        public void UpgradeSkylab()
        {
            if (this.api.Settings.IsUnity)
            {
                return;
            }

            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return;

            var skylabUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalSkylab";
            var skylabResponse = this.httpClient.Request(new RequestBuilder(RequestMethod.Get), skylabUrl);

            Skylab.BuyRobots(this.api);
            Skylab.UpgradeModules(this.api, skylabResponse);
        }

        public void ShopBuyPetFuel()
        {
            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return;

            _ = this.httpClient.Request(new(RequestMethod.Get), $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalDock&tpl=internalDockPetGear");
            var shopResponse = this.ShopBuy("petGear", "resource_pet-fuel", 1000);

            if (int.TryParse(HelpTools.Between(shopResponse, "\"fuel\":", ",\"maxFuel"), out var newFuel))
            {
                this.api.Hero.Pet.Fuel = newFuel;
            }
            this.api.WriteLog(HelpTools.Between(shopResponse, "message\":\"", "\",\"showEquip"));

        }

        public void ShopBuyCloak()
        {
            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return;

            _ = this.httpClient.Request(new(RequestMethod.Get), $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalDock&tpl=internalDockSpecials");
            _ = this.ShopBuy("special", "equipment_extra_cpu_cl04k-xs", 1);
        }

        public void ShopBuyLaser(string laserType)
        {
            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return;

            if (string.IsNullOrEmpty(laserType))
            {
                return;
            }

            _ = this.httpClient.Request(new(RequestMethod.Get), $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalDock&tpl=internalDockAmmo");
            _ = this.ShopBuy("battery", laserType, 10000);
        }

        public void ShopBuyRocket(string rocketType)
        {
            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return;

            if (string.IsNullOrEmpty(rocketType))
            {
                return;
            }

            _ = this.httpClient.Request(new(RequestMethod.Get), $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalDock&tpl=internalDockAmmo");
            _ = this.ShopBuy("rocket", rocketType, 100);
        }

        private string ShopBuy(string category, string itemId, int amount)
        {
            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return string.Empty;

            var shopUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/ajax/shop.php";
            RequestBuilder requestBuilder = new(RequestMethod.Post);
            requestBuilder.Add("action", "purchase");
            requestBuilder.Add("category", category);
            requestBuilder.Add("itemId", itemId);
            requestBuilder.Add("amount", amount.ToString());
            requestBuilder.Add("level", string.Empty);
            requestBuilder.Add("selectedName", string.Empty);
            return this.httpClient.Request(requestBuilder, shopUrl);
        }

        public void SwitchHangar(long hangardId, int tries = 0)
        {
            if (this.api.Settings.IsUnity)
            {
                //https://gbl2.darkorbit.com/unityApi/activate.php
                this.api.Logic.DeactivateCurrentHangar();
                var activateUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/unityApi/activate.php";
                RequestBuilder request = new(RequestMethod.Post);
                request.Add("hangarId", hangardId.ToString());
                var res = this.httpClient.Request(request, activateUrl);

                if (res.Contains("success\":1"))
                {
                    Hangar? newHangar = this.api.Hangars.FirstOrDefault(h => h.HangarId == hangardId);

                    if (newHangar != null)
                    {
                        newHangar.IsActive = true;
                        if (newHangar.MapId > 0)
                        {
                            this.api.Hero.NextMapId = newHangar!.MapId;
                        }
                    }

                    this.api.WriteLog("Hangar changed...");
                    this.api.Logging.Log("Hangar Changed: Next MapId = " + this.api.Hero.NextMapId);
                    return;
                }
                if (this.Login() && tries < 1)
                {
                    this.SwitchHangar(hangardId, 1);
                    return;
                }
                this.api.Logging.Log(res);
                return;
            }

            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
            {
                this.api.WriteLog("Session is invalid! Relog required to change Hangar!");
                this.api.GameClient.GameState = GameState.Disconnected;
                return;
            }

            var hangarUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalDock";

            var response = this.httpClient.Request(new RequestBuilder(RequestMethod.Get), hangarUrl);
            var reloadToken = HelpTools.Between(response, "&reloadToken=", "\">")[..32];

            hangarUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es";

            RequestBuilder requestBuilder = new(RequestMethod.Get);
            requestBuilder.Add("action", "internalDock");
            requestBuilder.Add("subAction", "changeHangar");
            requestBuilder.Add("hangarId", hangardId.ToString());
            requestBuilder.Add("reloadToken", reloadToken);

            this.api.Logging.Log("Hangar Switch Request: " + requestBuilder.GetRequestString());
            var changeResponse = this.httpClient.Request(requestBuilder, hangarUrl);

            this.GetInventory();
        }

        private void HandleHangarResponse(string hangarResponse)
        {
            var hangars = hangarResponse.Split("{\"hangarID");
            Dictionary<long, Hangar> hangarsList = [];
            foreach (var hangar in hangars)
            {
                if (!hangar.Contains("hangar_is_active"))
                {
                    continue;
                }
                var hangarId = long.Parse(HelpTools.Between(hangar, "\":\"", "\",\"hangar_is_active"));
                var hangarActv = bool.Parse(HelpTools.Between(hangar, "hangar_is_active\":", ",\"general"));
                var hangarShip = HelpTools.Between(hangar, "\"SM\":\"", "\",\"M\":");

                var hangarObj = new Hangar(hangarId, hangarActv, hangarShip, false);

                hangarsList.Add(hangarId, hangarObj);
            }
            var shipInfos = hangarResponse.Split("{\"lootId");
            foreach (var shipInfo in shipInfos)
            {
                if (!shipInfo.Contains(",\"hangarId\":"))
                {
                    continue;
                }
                //var lootId = HelpTools.Between(shipInfo, ":\"", "\",\"owned");
                var hangarId = long.Parse(HelpTools.Between(shipInfo, ",\"hangarId\":", ",\"factionBased").Replace("\"", "").Trim());
                var isFavorite = HelpTools.Between(shipInfo, ",\"fav\":", ",\"price");
                if (!hangarsList.ContainsKey(hangarId))
                {
                    continue;
                }
                hangarsList[hangarId].IsFavorite = (isFavorite == "1");
                Debug.WriteLine("HangarId: " + hangarId + " is Favorite: " + isFavorite);
            }

            this.api.Logging.Log("Received " + hangarsList.Count + " Hangars!");

            this.api.AddHangar([.. hangarsList.Values]);
        }

        /// <summary>
        /// Loads the Auction to place bids on the selected Items.
        /// </summary>
        public void AuctionBidInit()
        {
            if (!this.api.GameObservers.SessionObserver.IsSessionValid)
                return;

            //https://de4.darkorbit.com/indexInternal.es?action=internalAuction
            var auctionUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalAuction";
            RequestBuilder requestBuilder = new(RequestMethod.Get);
            var auctionResponse = this.httpClient.Request(requestBuilder, auctionUrl);

            if (!auctionResponse.Contains("auction_item_name_col"))
            {
                return;
            }
            var bidBtnTxt = HelpTools.Between(auctionResponse, "<input type=\"submit\" name=\"auction_buy_button\" class=\"disabled\" id=\"auction_place_bid\" value=\"", "\" />");
            var reloadToken = HelpTools.Between(auctionResponse, "indexInternal.es?action=internalAuction&reloadToken=", "\" onsubmit");
            var auctionData = HelpTools.Between(auctionResponse, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"auctionItemList\">", "</table>");
            var auctionItemsList = auctionData.Split("<tr class=\"auctionItemRow evenRow\"");

            string itemKey, lootId;//, itemName;
            double currentBid;

            this.api.Hero.IsCurrentlyBidding = true;

            foreach (Settings.AuctionSetting setting in this.api.Settings.AuctionSettings)
            {
                if (!setting.IsEnabled || setting.CreditsAmount > this.api.Inventory.GetCredits)
                {
                    continue;
                }

                foreach (var item in auctionItemsList)
                {
                    itemKey = HelpTools.Between(item, "itemKey=\"", "\" onclick=").Trim();
                    lootId = HelpTools.Between(item, $"id=\"{itemKey}_lootId\" value=\"", "\" />").Trim();

                    if (lootId != setting.ItemLoodId)
                    {
                        continue;
                    }

                    //itemName = HelpTools.Between(item, "<td class=\"auction_item_name_col\">", "</td>").Trim();
                    currentBid = double.Parse(HelpTools.Between(item, "<td class=\"auction_item_current\">", "</td>").Trim());

                    if (currentBid >= setting.CreditsAmount)
                    {
                        continue;
                    }

                    this.AuctionBidRequest(itemKey, lootId, setting.CreditsAmount, bidBtnTxt, reloadToken);
                    Thread.Sleep(Random.Shared.Next(3750, 6240));
                }
            }

            this.api.Hero.IsCurrentlyBidding = false;
        }

        /// <summary>
        /// Sends an Auction bid request.
        /// </summary>
        /// <param name="itemKey">Item Key</param>
        /// <param name="lootId">Item Loot Name</param>
        /// <param name="creditsAmount">Credits to be bid</param>
        /// <param name="bidBtnTxt">Auction Bid Button Text</param>
        /// <param name="reloadToken">Auction Reload Token</param>
        public void AuctionBidRequest(string itemKey, string lootId, double creditsAmount, string bidBtnTxt, string reloadToken)
        {
            if (this.api.Settings.IsUnity)
            {
                //api.GameMethods.AuctionBid(lootId, (long)creditsAmount, itemKey);
                return;
            }
            //reloadToken=ffb4e58aa2ff0c636873ea0f2ee31277&auctionType=hour&subAction=bid&lootId=equipment_extra_cpu_smb-01&itemId=item_hour_20&credits=20000&auction_buy_button=BIETEN
            var auctionUrl = $"https://{this.api.Hero.ServerTag}.darkorbit.com/indexInternal.es?action=internalAuction&reloadToken={reloadToken}";
            var requestBuilder = new RequestBuilder(RequestMethod.Post);
            requestBuilder.Add("reloadToken", reloadToken);
            requestBuilder.Add("auctionType", "hour");
            requestBuilder.Add("subAction", "bid");
            requestBuilder.Add("lootId", lootId);
            requestBuilder.Add("itemId", itemKey);
            requestBuilder.Add("credits", creditsAmount.ToString());
            requestBuilder.Add("auction_buy_button", bidBtnTxt);

            this.httpClient.Request(requestBuilder, auctionUrl);
            this.api.WriteLog($"Bidding on {lootId}, with {creditsAmount} Credits.");
        }
    }
}
