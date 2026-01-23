using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace DarksideApi.DarkOrbit
{
    internal class Proxy
    {
        private readonly WebProxy proxy;
        private readonly Uri proxyUri;
        private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;

        public Proxy(string httpProxyHost, int httpProxyPort, string proxyUserName, string proxyPassword)
        {
            this.proxyUri = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttp,
                Host = httpProxyHost,
                Port = httpProxyPort
            }.Uri;

            this.proxy = new WebProxy(this.proxyUri, true)
            {
                Credentials = new NetworkCredential(proxyUserName, proxyPassword),
            };
        }

        public WebProxy GetWebProxy()
        {
            return this.proxy;
        }

        public TcpClient ConnectToTarget(string targetHost, int targetPort)
        {
            Uri targetUri = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttp,
                Host = targetHost,
                Port = targetPort
            }.Uri;

            var request = WebRequest.Create(targetUri);
            request.Proxy = this.proxy;
            request.Method = "CONNECT";
            using var response = (HttpWebResponse)request.GetResponse();
            using Stream responseStream = response.GetResponseStream();
            Type responseType = responseStream.GetType();
            PropertyInfo? connectionProperty = responseType.GetProperty("Connection", Flags);
            var connection = connectionProperty.GetValue(responseStream, null);
            Type connectionType = connection.GetType();
            PropertyInfo? networkStreamProperty = connectionType.GetProperty("NetworkStream", Flags);
            using var networkStream = (NetworkStream)networkStreamProperty.GetValue(connection, null);
            Type nsType = networkStream.GetType();
            PropertyInfo? socketProperty = nsType.GetProperty("Socket", Flags);
            using var socket = (Socket)socketProperty.GetValue(networkStream, null);

            return new TcpClient { Client = socket };
        }
    }
}
