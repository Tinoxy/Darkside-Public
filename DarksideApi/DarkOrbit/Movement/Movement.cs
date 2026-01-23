using DarksideApi.DarkOrbit.Objects;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.Util;
using Glide;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Movement
{
    public class Movement
    {
        private readonly Api api;
        private bool _running;
        private Tweener tweener;
        private Thread? movementUpdate;
        private Stopwatch stopwatch;
        public List<PoiZone> PATH_ZONES = [];
        private PoiZone LastFirstZone;
        private PoiZone[] CURRENT_ZONES;
        //private DarksideApi.DarkOrbit.Movement.Pathfinder.Pathfinder pathfinder;

        public Movement(Api api)
        {
            this.api = api;
            this.stopwatch = new();
            this.tweener = new();
            this.LastFirstZone = PoiZone.Empty();
            this.CURRENT_ZONES = [];
            //this.pathfinder = new DarkOrbit.Movement.Pathfinder.Pathfinder(api);
        }

        public void StartMovementUpdater()
        {
            this.StopMovementUpdater();
            this.stopwatch = new();
            this.tweener = new();
            this._running = true;
            this.movementUpdate = new(this.UpdateMovement)
            {
                IsBackground = true,
            };
            this.movementUpdate.Start();
        }

        public void StopMovementUpdater()
        {
            try
            {
                this._running = false;
                this.tweener?.CancelAndComplete();
                this.movementUpdate?.Join();
            }
            catch (Exception) { }
        }

        public void ResetPath()
        {
            this.api.Hero.Path.Clear();
        }

        public void ResetZones()
        {
            this.CURRENT_ZONES = [];
        }

        public LinkedList<Vector2> GeneratePath(int targetX, int targetY)
        {
            if (this.CURRENT_ZONES.Length != this.api.POIZones.Count)
            {
                this.CURRENT_ZONES = [.. this.api.POIZones.Values];
            }
            var result = Pathfinder.FindShortestPath(
                this.api.Hero.Position,
                new Vector2(targetX, targetY),
                this.api.POIZones.Values,
                this.api.Map);
            result.RemoveFirst(); //remove current position

            return result;
        }

        private void UpdateMovement()
        {
            this.api.Logging.Log("Starting Movement Updater");
            while (this._running)
            {
                Vector2 current = this.api.Hero.Position;
                var currents = this.api.Ships.Values.ToDictionary(key => key.UserId, value => value.Position);
                Thread.Sleep(10);
                try
                {
                    this.stopwatch.Stop();

                    if (this.api.Hero.FlyingTimeMs > 0)
                    {
                        this.api.Hero.FlyingTimeMs -= this.stopwatch.ElapsedMilliseconds;
                    }

                    lock (this.api.ObjectLocker)
                    {
                        this.tweener?.Update(this.stopwatch.ElapsedMilliseconds);
                    }

                    this.stopwatch.Restart();

                    this.api.Hero.Angle = Math.Atan2(this.api.Hero.Y - current.Y, this.api.Hero.X - current.X);

                    for (var i = 0; i < currents.Count; i++)
                    {
                        KeyValuePair<int, Vector2> element = currents.ElementAt(i);
                        if (!this.api.Ships.TryGetValue(element.Key, out Objects.Ship.Ship? ship))
                        {
                            continue;
                        }
                        ship.Angle = Math.Atan2(ship.Y - element.Value.Y, ship.X - element.Value.X);
                    }
                }
                catch (Exception ex)
                {
                    this.api.Logging.Log("Movement Error: " + ex);
                }
            }
        }

        public void StartMovement(IGameObject? gameObject, int targetX, int targetY, float duration)
        {
            try
            {
                if (gameObject == null)
                {
                    return;
                }
                lock (this.api.ObjectLocker)
                {
                    this.tweener?.Tween(gameObject, new { X = targetX, Y = targetY }, duration);
                }
            }
            catch (Exception)
            {
            }
        }

        public void StopMovement(IGameObject? gameObject)
        {
            try
            {
                if (gameObject == null)
                {
                    return;
                }
                lock (this.api.ObjectLocker)
                {
                    this.tweener?.TargetCancel(gameObject);
                }
            }
            catch (Exception)
            {
            }
        }

        public void CompleteMovement(IGameObject? gameObject)
        {
            try
            {
                if (gameObject == null)
                {
                    return;
                }
                lock (this.api.ObjectLocker)
                {
                    this.tweener?.TargetCancelAndComplete(gameObject);
                }
            }
            catch (Exception x)
            {
                this.api.Logging.Log(x);
            }
        }
    }
}
