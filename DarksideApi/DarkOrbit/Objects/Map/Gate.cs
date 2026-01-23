using DarksideApi.DarkOrbit.Commands;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Map
{
    public class Gate : IGameObject
    {
        private readonly Api api;

        public int GateId { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position => new(this.X, this.Y);
        public int GateDesignId { get; set; }
        public int FactionId { get; set; }
        public bool IsVisible { get; set; }
        public bool IsBlocked { get; set; }
        public bool CanJump { get; set; }
        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.LastPathElement, this.Position) : double.MaxValue;
        public bool HasApi => this.api != null;

        public bool IsGalaxyGate => CommandConstants.GALAXY_GATES.ContainsKey(this.GateDesignId);
                public bool IsPirateGate => this.GateDesignId == 51
                || this.GateDesignId == 52;
                public bool IsInvasionGate => this.GateDesignId == 41
                || this.GateDesignId == 42
                || this.GateDesignId == 43;
        public bool IsQuarantineZoneGate => this.GateDesignId == 84;
        public bool IsLowGate => this.GateDesignId == 34;
        public bool IsDeepSpaceEchoesGate => this.GateDesignId == 301;
        public bool IsFrozenLabyrinthGate => this.GateDesignId == 83;
        public bool IsLocked => this.HasApi && this.api.LockedGateIds.Contains(this.GateId);

        public int LeadsToMapId()
        {
            if (!Api.StarSystem.TryGetValue(this.api.Map.Id, out StarSystem.StarMap? system))
            {
                return 0;
            }
            StarSystem.StarPortal? portal = system.Portals.FirstOrDefault(p => this.api.Logic.GetClosestGateToPosition(p.X, p.Y) == this);
            return portal == null ? 0 : portal.LeadsToMapId;
        }

        public bool LeadsToPvPMap()
        {
            var leadsTo = this.LeadsToMapId();
            return leadsTo == 13 || leadsTo == 14 || leadsTo == 15 || leadsTo == 16 || leadsTo == 29;
        }

        public Gate(Api api, int gateId, int x, int y, int gateDesignId, int factionId, bool visible)
        {
            this.api = api;
            this.GateId = gateId;
            this.X = x;
            this.Y = y;
            this.GateDesignId = gateDesignId;
            this.FactionId = factionId;
            this.IsVisible = visible;

            //api.Logging.WriteLine($"Gate: {gateId} created at [{x}, {y}] with design id: {gateDesignId}.");
        }

        public bool IsUsable()
        {
            if (!Api.StarSystem.TryGetValue(this.api.Map.Id, out StarSystem.StarMap? starMap))
            {
                return false;
            }
            List<StarSystem.StarPortal> list = starMap.Portals;
            foreach (StarSystem.StarPortal gj in list)
            {
                if (this.api.Logic.GetClosestGateToPosition(gj.X, gj.Y).GateId != this.GateId)
                {
                    continue;
                }
                return gj.LeadsToMapId != 91 && gj.LeadsToMapId != 92 && gj.LeadsToMapId != 93 && gj.LeadsToMapId != 94;
            }
            return true;
        }

        public bool IsOnTarget(Vector2 position)
        {
            return HelpTools.CalculateDistance(this.Position, position) <= 10;
        }

        public static Gate Empty()
        {
            return new Gate(null, 0, 0, 0, -1, 0, false);
        }
    }
}
