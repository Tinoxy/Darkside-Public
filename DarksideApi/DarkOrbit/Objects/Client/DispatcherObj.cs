using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Constants;

namespace DarksideApi.DarkOrbit.Objects.Client
{
    public class DispatcherObj
    {
        public int Id { get; private set; }
        public double Duration { get; set; }
        public int SlotId { get; set; }
        public bool IsActive => this.SlotId > 0;
        public DateTime StartTime { get; set; }
        public bool IsEnabled { get; set; }
        public string Name => this.Id >= 1 && this.Id <= 6 ? "A-0" + this.Id
                            : this.Id >= 7 && this.Id <= 12 ? "B-0" + (this.Id - 6)
                            : this.Id >= 19 && this.Id <= 21 ? "H-0" + (this.Id - 16)
                            : this.Id >= 13 && this.Id <= 18 ? "R-0" + (this.Id - 12)
                            : "D-" + this.Id;
        public double DurationPercentage => (double)(this.Duration / ConstantCollection.DISPATCHER_DEFAULT_LIST.Where(d => d.Id == this.Id).FirstOrDefault()!.Duration * 100);

        public DispatcherObj(int id, double duration, int slotId)
        {
            this.Id = id;
            this.Duration = duration;
            this.SlotId = slotId;
        }

        public static DispatcherObj From(DispatcherObj obj)
        {
            return new DispatcherObj(obj.Id, obj.Duration, obj.SlotId)
            {
                IsEnabled = obj.IsEnabled,
                StartTime = obj.StartTime,
            };
        }

        public static DispatcherObj Empty()
        {
            return new DispatcherObj(0, 0, 0);
        }
    }
}
