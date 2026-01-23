using DarksideApi.DarkOrbit;
using System.Net;
using System.Text;

namespace DarksideApi.HttpWeb
{
    public class HttpClient
    {
        public CookieContainer Cookies => this.handler!.CookieContainer;
        public string CurrentURL { get; private set; }
        private System.Net.Http.HttpClient client;
        private readonly HttpClientHandler handler;
        private string _currentUserAgent;

        public HttpClient(bool bigpointClient, bool unity)
        {
            this._currentUserAgent = (unity ? "UnityPlayer/2022.3.58f1 (UnityWebRequest/1.0, libcurl/8.10.1-DEV)" : (bigpointClient ? $"BigpointClient/{Api.BigpointClientVersion}" : "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36"));

            this.CurrentURL = "https://www.google.com";
            this.handler = new()
            {
                UseCookies = true,
                AllowAutoRedirect = true,
            };

            this.client = new(this.handler);
            this.client.DefaultRequestHeaders.UserAgent.ParseAdd(this._currentUserAgent);
        }

        public void UpdateUserAgent(bool toBigpointClient, bool toUnity)
        {
            var ua = (toUnity ? "UnityPlayer/2022.3.58f1 (UnityWebRequest/1.0, libcurl/8.10.1-DEV)" : (toBigpointClient ? $"BigpointClient/{Api.BigpointClientVersion}" : "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36"));

            if (ua == this._currentUserAgent)
            {
                return;
            }

            this._currentUserAgent = ua;
            this.client.DefaultRequestHeaders.UserAgent.Clear();
            this.client.DefaultRequestHeaders.UserAgent.ParseAdd(this._currentUserAgent);
        }

        public void UpdateProxy(WebProxy proxy)
        {
            this.handler.Proxy = proxy;
        }

        public void Shutdown()
        {
            this.client.CancelPendingRequests();
            this.client.Dispose();
            this.handler.Dispose();
        }

        public string Request(RequestBuilder request, string url, Dictionary<string, string>? headers = null)
        {
            if (url.StartsWith("http://.", StringComparison.CurrentCulture))
            {
                return string.Empty;
            }
            headers ??= [];

            var requestString = request.GetRequestString();
            url += (request.Method == RequestMethod.Get && requestString.Length > 0 ? ("?" + requestString) : "");

            HttpRequestMessage? requestBody = null;
            if (request.Method == RequestMethod.Post)
            {
                requestBody = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded"),
                };
                foreach (KeyValuePair<string, string> header in headers)
                {
                    requestBody.Headers.Add(header.Key, header.Value);
                }
            }
            if (request.Method == RequestMethod.Get)
            {
                requestBody = new HttpRequestMessage(HttpMethod.Get, url);
            }
            try
            {
                using HttpResponseMessage response = this.client.Send(requestBody!);

                //var status = response.EnsureSuccessStatusCode();
                //api.Logging.WriteLine(status.ReasonPhrase);

                using Stream responseStream = response.Content.ReadAsStream();
                using StreamReader reader = new(responseStream);
                var responseFromServer = reader.ReadToEnd();

                this.CurrentURL = response.RequestMessage?.RequestUri?.ToString() ?? url;

                return responseFromServer;
            }
            catch (Exception)
            {
            }
            finally
            {
                requestBody?.Dispose();
            }

            return string.Empty;
        }
    }
}
