using DarksideApi;

namespace DarkOrbot
{
    internal interface IControl
    {
        public bool Ready { get; protected set; }
        public string ApiKey { get; }
        public DarkOrbotSettings Settings { get; }
    }
}
