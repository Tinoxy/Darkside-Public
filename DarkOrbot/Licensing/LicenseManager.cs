using DarksideApi.DarkOrbit;
using DeviceId;
using System.Diagnostics;

namespace DarkOrbot.Licensing
{
    public class LicenseManager
    {
        private const int CHECK_SUM = 41;

        private string? injectionKey;
        private DateTime ValidUntil;
        private DateTime StartDate;
        private readonly string uniqueDeviceIdentifier;
        private readonly List<string> licenseStateTexts;

        private System.Timers.Timer? timer;
        private LicenseState licenseState;
        private LicenseObject? lastLicenseObject;

        private event EventHandler<LicenseState> LicenseStateChanged;
        public event EventHandler<string>? InjectionKeyChanged;

        public double DaysLeft => (DateTime.UtcNow - this.StartDate).TotalDays / (this.ValidUntil - this.StartDate).TotalDays;
        public double FullPercentageNotNull => this.PercentageNullable < 0 ? 0d : ((double)this.PercentageNullable * 100d);
        public double PercentageNotNull => this.PercentageNullable < 0 ? 0d : this.PercentageNullable;
        public double PercentageNullable => (1 - this.DaysLeft);

        public LicenseState LicenseState
        {
            get => this.licenseState;
            set
            {
                this.licenseState = value;
                LicenseStateChanged?.Invoke(this, this.licenseState);
            }
        }

        public string UID => this.uniqueDeviceIdentifier ?? string.Empty;
        public string InjectionKey
        {
            get => this.injectionKey ?? string.Empty;
            set
            {
                this.injectionKey = value;
                InjectionKeyChanged?.Invoke(this, value);
            }
        }
        public string[] LicenseStateTexts => [.. this.licenseStateTexts];

        public LicenseManager()
        {
            this.uniqueDeviceIdentifier = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString();
            LicenseStateChanged += this.LicenseManager_licenseStateChanged;
            this.licenseStateTexts = [];
        }

        public void InitUpdate()
        {
            LicenseMethods.IsNewVersionAvailable();
            Api.TrackVersion = LicenseMethods.GetServerTrackingVersion();
            this.LicenseState = LicenseState.Refresh;
        }

        private void RestartTimer()
        {
            this.timer?.Stop();

            this.timer ??= new(TimeSpan.FromMinutes(30))
            {
                AutoReset = false,
            };

            this.timer.Elapsed += (s, e) =>
            {
                this.LicenseState = LicenseState.Refresh;
            };

            this.timer.Start();
        }

        private void LicenseManager_licenseStateChanged(object? sender, LicenseState e)
        {
            LicenseObject? @object = null;

            this.licenseStateTexts.Clear();

            if (e == LicenseState.Refresh)
            {
                @object = LicenseMethods.Fetch(this.UID);
                this.RestartTimer();
                Debug.WriteLine(@object.ToString());
            }
            if (e == LicenseState.Error)
            {
                //this.InjectionKey = this.lastLicenseObject!.key; //lastLicenseObject is null here!
                this.licenseStateTexts.Add("There was an error contacting the Darkside Server!");
                if (this.lastLicenseObject != null && this.lastLicenseObject.additionalInfo.Length > 0)
                {
                }
                return;
            }
            if (e == LicenseState.Valid)
            {
                //Get Received Key -> Inject into API
                /* lastLicenseObject is null here!
                if (!int.TryParse(this.lastLicenseObject!.additionalInfo, out var checksum) || checksum != CHECK_SUM)
                {
                    this.lastLicenseObject.additionalInfo = "Invalid Checksum!";
                    this.LicenseState = LicenseState.Error;
                    return;
                }
                */
                this.InjectionKey = "9942-8832-9999-9999";//this.lastLicenseObject!.key;
                /*if (this.lastLicenseObject.type == LicenseType.Admin)
                {
                    return;
                }*/
                return;
            }
            this.LicenseObjectHandler(@object);
        }

        private void LicenseObjectHandler(LicenseObject? licenseObject)
        {
            this.LicenseState = LicenseState.Valid;
        }
    }
}
