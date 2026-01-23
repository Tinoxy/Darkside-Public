using System.Web;

namespace DarksideApi.HttpWeb
{
    public class RequestBuilder
    {
        public static RequestBuilder Empty = new(RequestMethod.Get);

        public RequestMethod Method { get; set; }

        private readonly List<KeyValuePair<string, string>> _parameters;

        public RequestBuilder() { this._parameters = []; }
        public RequestBuilder(RequestMethod method)
        {
            this._parameters = [];
            this.Method = method;
        }

        public void Add(string key, string value)
        {
            this._parameters.Add(new KeyValuePair<string, string>(key, value));
        }

        public string GetRequestString()
        {
            var output = this._parameters.Aggregate("", (current, kv) => current + $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}&");
            return output.Length > 0 ? output[..^1] : string.Empty;
        }
    }
}
