using Newtonsoft.Json;

namespace DarkOrbot.Licensing
{
    internal class LicenseObject
    {
        [JsonProperty("start")]
        public DateTime startTime;
        [JsonProperty("stop")]
        public DateTime endTime;
        [JsonProperty("type")]
        public LicenseType type;
        [JsonProperty("key")]
        public string key;
        [JsonProperty("additional")]
        public string additionalInfo;
        [JsonIgnore]
        public DateTime receivedTimestamp;

        public LicenseObject(DateTime startTime, DateTime endTime, LicenseType type, string key, string additionalInfo = "")
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.type = type;
            this.key = key;
            this.additionalInfo = additionalInfo;
            this.receivedTimestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return this.startTime.ToShortDateString()
            + ", " + this.endTime.ToShortDateString()
            + ", " + this.type
            + ", " + this.key
            + ", " + this.additionalInfo
            + ", " + this.receivedTimestamp.ToShortDateString();
        }
    }
}
