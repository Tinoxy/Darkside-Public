using DarksideApi.HttpWeb;

namespace DarksideApi.DarkOrbit.Objects.Client
{
    public class Skylab
    {
        public static List<string> GetUpgradeUrls(string server, string reloadToken)
        {
            return [
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=baseModule&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=solarModule&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=terbiumCollector&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=prometiumCollector&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=enduriumCollector&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=prometidRefinery&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=duraniumRefinery&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=promeriumRefinery&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=xenoModule&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=sepromRefinery&reloadToken={reloadToken}",
            $"https://{server}.darkorbit.com/indexInternal.es?action=internalSkylab&subaction=upgrade&construction=storageModule&reloadToken={reloadToken}",
        ];
        }

        public static List<string> GetCollectors()
        {
            return [
            "prometiumCollector",
            "enduriumCollector",
            "terbiumCollector",
        ];
        }

        public static List<string> GetModules()
        {
            return [
            $"baseModule",
            $"solarModule",
            $"terbiumCollector",
            $"prometiumCollector",
            $"enduriumCollector",
            $"prometidRefinery",
            $"duraniumRefinery",
            $"promeriumRefinery",
            $"xenoModule",
            $"sepromRefinery",
            $"storageModule"
        ];
        }

        public static void UpgradeModules(Api api, string skylabResponse)
        {
            if (!api.Settings.GeneralUpgradeSkylab
                || api.Hero.IsCurrentlyUpgradingSkylab
                || api.Settings.IsUnity)
                return;

            var startIndex = skylabResponse.LastIndexOf("&reloadToken=");
            var reloadToken = skylabResponse.Substring(startIndex + 13, 32);

            List<string> modules = GetUpgradeUrls(api.Hero.ServerTag, reloadToken);

            api.WriteLog("Upgrading Skylab...");
            api.Hero.IsCurrentlyUpgradingSkylab = true;
            foreach (var module in modules)
            {
                _ = api.BackpageApi.Client.Request(new RequestBuilder(RequestMethod.Get), module);
                Thread.Sleep(Random.Shared.Next(1500, 2250));
            }

            api.Hero.IsCurrentlyUpgradingSkylab = false;
        }

        public static void UpgradeModules(Api api)
        {
            if (!api.Settings.GeneralUpgradeSkylab
                || api.Hero.IsCurrentlyUpgradingSkylab
                || !api.Settings.IsUnity
                || api.GameClient.GameState < GameState.Ready)
                return;

            api.WriteLog("Upgrading Skylab...");
            api.Hero.IsCurrentlyUpgradingSkylab = true;
            foreach (var module in GetModules())
            {
                api.GameMethods.SkylabUpgradeModule(module);
                Thread.Sleep(Random.Shared.Next(1000, 2050));
            }

            BuyRobots(api);

            api.Hero.IsCurrentlyUpgradingSkylab = false;
        }

        public static void BuyRobots(Api api)
        {
            if (!api.Settings.GeneralBuySkylabRobots)
                return;

            if (api.Settings.IsUnity && api.GameClient.GameState != GameState.Ready)
                return;

            api.WriteLog("Buying Skylab Robots...");
            HttpWeb.HttpClient httpClient = api.BackpageApi.Client;
            foreach (var collector in GetCollectors())
            {
                if (api.Settings.IsUnity)
                {
                    for (var i = 0; i < 12; i++)
                        api.GameMethods.SkylabBuyRobot(collector);

                    continue;
                }
                RequestBuilder requestBuilder = new(RequestMethod.Post);
                requestBuilder.Add("command", "buySkylabRobot");
                requestBuilder.Add("type", "1");
                requestBuilder.Add("construction", collector);

                for (var i = 0; i < 12; i++)
                {
                    _ = httpClient.Request(requestBuilder, $"https://{api.Hero.ServerTag}.darkorbit.com/ajax/skylab.php");
                    Thread.Sleep(Logic.Logic.GetRandomSleepTime());
                }
            }
        }
    }
}
