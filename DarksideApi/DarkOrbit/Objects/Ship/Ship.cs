using DarksideApi.Render;
using DarksideApi.Util;
using DarksideApi.Util.Extensions;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Ship
{
    public class Ship(Api api, int userID, string username, int x, int y, string shipname) : IGameObject, IRenderable
    {
        //Api
        protected readonly Api api = api;

        //Info
        public int UserId { get; private set; } = userID;
        public string Username { get; private set; } = username;

        // Movement
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public Vector2 Position => new(this.X, this.Y);
        public Vector2 TargetPosition => new(this.TargetX, this.TargetY);
        public double Speed { get; set; }
        public double Angle { get; set; }
        public double CurrentDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.X, this.api.Hero.Y, this.X, this.Y) : double.MaxValue;
        public double TargetDistance => this.HasApi ? HelpTools.CalculateDistance(this.api.Hero.TargetX, this.api.Hero.TargetY, this.X, this.Y) : double.MaxValue;
        public double PathDistance => this.HasApi ? this.Position.DistanceTo(this.api.Hero.LastPathElement) : double.MaxValue;
        public bool IsMoving => this.HasApi && HelpTools.CalculateDistance(this.X, this.Y, this.TargetX, this.TargetY) > 10;
        public bool HasApi => this.api != null;

        // Ship
        public string Shipname { get; private set; } = shipname;
        public string Title { get; set; } = string.Empty;

        //Status
        public long Hitpoints { get; set; }
        public long MaxHitpoints { get; set; }
        public int Shield { get; set; }
        public int MaxShield { get; set; }
        public int NanoShield { get; set; }
        public int MaxNanoShield { get; set; }
        public bool IsSelected { get; set; }
        public bool IsSelectable => this.tempSelectTries < 3;

        //StatusPercentage
        public double HitpointsPercentage => this.MaxHitpoints > 0 ? this.Hitpoints / (double)this.MaxHitpoints * 100.0 : 0;
        public double ShieldPercentage => this.MaxShield > 0 ? this.Shield / (double)this.MaxShield * 100.0 : 0;

        //Render
        public DateTime ActionStartTime { get; set; }
        public double Radius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double DamageReceived { get; set; }

        //Temp Values
        public bool tempCircleClockwise = false;
        public int tempAttackTries = 0;
        public int tempSelectTries = 0;
        public DateTime tempRsbLastUsed = DateTime.UtcNow;
        public DateTime tempLastCircleMove = DateTime.UtcNow;
        public List<Vector2> tempLastPositions = [new(x, y)];

        public Vector2 GetPositionInTime(long time)
        {
            if (this.TargetX == 0 || this.TargetY == 0)
            {
                return new(this.X, this.Y);
            }
            Vector2 destination = new();

            var move = this.Speed * time;

            destination.X = (float)(this.X + Math.Cos(this.Angle) * move);
            destination.Y = (float)(this.Y + Math.Sin(this.Angle) * move);

            return destination;
        }

        public double GetRadius(double radius)
        {
            return radius;
        }

        public override string ToString()
        {
            return $"{this.Username} (ID: {this.UserId}), Pos: {this.X}/{this.Y}, Target: {this.TargetX}/{this.TargetY}, Ship: {this.Shipname} Hitpoints: {this.Hitpoints}, Max: {this.MaxHitpoints}";
        }
    }
}
