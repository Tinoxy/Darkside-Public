using DarksideApi.Util.Extensions;
using System.ComponentModel;

namespace DarksideApi.Statistics
{
    public class StatisticsItem : INotifyPropertyChanged
    {
        private long _amount;
        private long _total;
        private double _perHour;
        private string? _displayInfo;
        private bool _formatAmount;

        public string Name { get; set; }
        public long Amount
        {
            get => this._amount;
            set
            {
                this._amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AmountString)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FullString)));
            }
        }
        public long Total
        {
            get => this._total;
            set
            {
                this._total = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TotalString)));
            }
        }
        public string DisplayInfo
        {
            get => this._displayInfo;
            set
            {
                this._displayInfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DisplayInfo)));
            }
        }

        public string AmountString => (this._formatAmount ? this._amount.KiloFormat() : this._amount.ToString()) + (!string.IsNullOrEmpty(this.DisplayInfo) ? $"({this.DisplayInfo})" : "");
        public string TotalString => this._total.KiloFormat();
        public string PerHourString => this._perHour.KiloFormat();
        public string FullString => this._amount != 0 ? (this.AmountString + "\n" + this.PerHourString + "/h") : "-";

        public event PropertyChangedEventHandler? PropertyChanged;

        public StatisticsItem(string name, long amount, long total = 0, string displayInfo = "", bool formatAmount = true)
        {
            this.Name = name;
            this.Amount = amount;
            this.Total = total;
            this.DisplayInfo = displayInfo;
            this._formatAmount = formatAmount;
        }

        public void UpdatePerHour(DateTime runTime)
        {
            this._perHour = Math.Round(this._amount / (DateTime.UtcNow - runTime).TotalHours, 2);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PerHourString)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FullString)));
        }

        public static StatisticsItem operator +(StatisticsItem a, long amount)
        {
            a.Amount += amount;
            return a;
        }

        public static StatisticsItem operator -(StatisticsItem a, long amount)
        {
            a.Amount -= amount;
            return a;
        }

        public static StatisticsItem operator ++(StatisticsItem a)
        {
            a.Amount += 1;
            return a;
        }

        public static StatisticsItem operator --(StatisticsItem a)
        {
            a.Amount -= 1;
            return a;
        }
    }
}
