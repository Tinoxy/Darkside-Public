using System.IO;
using System.Net.Http;

namespace DarkOrbot.Licensing
{
    internal class LicenseHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly HttpClientHandler handler;

        public LicenseHttpClient()
        {
            this.handler = new()
            {
                UseCookies = true,
                AllowAutoRedirect = true,
            };

            this.httpClient = new(this.handler);
            this.httpClient.BaseAddress = new Uri("https://google.com");

            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"1.3 API Darkside/1.0.X");
        }

        internal string Get(string address)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, address);
            using HttpResponseMessage response = this.httpClient.Send(request);

            response.EnsureSuccessStatusCode();

            using StreamReader streamReader = new(response.Content.ReadAsStream());

            return streamReader.ReadToEnd();
        }

        internal string Post(string address, string requestData)
        {
            using var requestBody = new HttpRequestMessage(HttpMethod.Post, address)
            {
                Content = new StringContent(requestData, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"),
            };

            using HttpResponseMessage response = this.httpClient.Send(requestBody);

            using StreamReader streamReader = new(response.Content.ReadAsStream());

            return streamReader.ReadToEnd();
        }
    }
}
