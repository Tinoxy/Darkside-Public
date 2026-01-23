using DarksideApi.DarkOrbit.Commands;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.StarSystem
{
    public class StarPortal(Api api, int gateId, int x, int y, int factionId, int leadsToMapId) : IGameObject
    {
        private readonly Api api = api;
        private readonly int leadsToMapId = leadsToMapId;

        public int GateId { get; private set; } = gateId;
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public Vector2 Position => new(this.X, this.Y);
        public int FactionId { get; private set; } = factionId;
        public int LeadsToMapId => (this.leadsToMapId == 0 ? CommandConstants.GET_HOMEMAP_ID(this.api.Hero.FactionId) : this.leadsToMapId);

        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public string DisplayName => "Portal to " + (CommandConstants.MAP_NAMES.TryGetValue(this.LeadsToMapId, out var mapName) ? mapName : this.LeadsToMapId.ToString());
        public bool HasApi => this.api != null;

        public static StarPortal Empty()
        {
            return new(null!, 0, 0, 0, 0, 0);
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
