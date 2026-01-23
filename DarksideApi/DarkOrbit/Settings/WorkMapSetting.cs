using DarksideApi.DarkOrbit.Commands;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace DarksideApi.DarkOrbit.Settings
{
    public class WorkMapSetting : Setting
    {
        public int MapId { get; set; }
        public DateTime StartTimeUTC { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string StartTimeUTCValue { get; set; }
        [XmlIgnore, JsonIgnore]
        public string MapName => CommandConstants.MAP_NAMES.TryGetValue(this.MapId, out var name) ? name : this.MapId.ToString();
        [XmlIgnore, JsonIgnore]
        public string StartTime => this.StartTimeUTC.ToShortTimeString();
        [XmlIgnore, JsonIgnore]
        public double TotalMinutesSinceStart => (DateTime.UtcNow - this.StartTimeUTC).TotalMinutes;
        [XmlIgnore, JsonIgnore]
        public double TotalSecondsSinceStart => (DateTime.UtcNow - this.StartTimeUTC).TotalSeconds;
        [XmlIgnore, JsonIgnore]
        public string SinceStart => $"{(DateTime.UtcNow - this.StartTimeUTC).Hours} {(DateTime.UtcNow - this.StartTimeUTC).Minutes} {(DateTime.UtcNow - this.StartTimeUTC).Seconds}";

    }
}
