using DarksideApi;
using DarksideApi.Util;
using DarksideApi.Util.Collections;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace DarkOrbot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string ApiAssemblyName = "DarksideApi";
        public static Licensing.LicenseManager? LicenseManager { get; private set; }
        public static ObservableKeyedCollection<string, CompactApiControl> Apis { get; private set; } = new(x => x.ApiKey, "ApiKey");

        private static Thread? Watcher { get; set; }

        public App() : base()
        {
            this.CreateErrorHandlers();

            App.LicenseManager = new();
            App.LicenseManager.InjectionKeyChanged += this.HandleInjectionKeyInit;
            //App.LicenseManager.InjectionKey = "9942-8832-9999-9999";
            App.LicenseManager.InitUpdate();

            this.StartDebugWatcher();
        }

        private void HandleInjectionKeyInit(object? sender, string injectionKey)
        {
            Debug.WriteLine("Key init");
            this.LoadSettings(injectionKey);
            App.LicenseManager!.InjectionKeyChanged -= this.HandleInjectionKeyInit;
        }

        private Assembly? LoadAssembly(object? sender, ResolveEventArgs args)
        {
            AssemblyName assemblyToLoad = new(args.Name);

            return assemblyToLoad.FullName.Contains(ApiAssemblyName)
                ? Assembly.Load(Util.HelpTools.LoadApiAssembly([0]))
                : null;
        }

        private void StartDebugWatcher()
        {
            if (Watcher != null && Watcher.IsAlive)
            {
                return;
            }
            Watcher = new Thread(this.DebugWatcher)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal,
            };
            Watcher.Start();
        }

        private void DebugWatcher()
        {
            while (true)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    Thread.Sleep(10000);
                    continue;
                }
                Process.GetCurrentProcess().Kill();
            }
        }

        private void CreateErrorHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
            DispatcherUnhandledException += this.App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += this.TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.AssemblyResolve += this.LoadAssembly;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            //HelpTools.WriteErrorLog(e.Exception.Message, e.Exception.ToString());
            e.SetObserved();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HelpTools.WriteErrorLog(e.Exception.Message, e.Exception.ToString());
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HelpTools.WriteErrorLog(((Exception)e.ExceptionObject).ToString(), "Unhandled Current Domain Exception");
        }

        public static int IsActiveBot(int userId)
        {
            return Apis.IndexOf(Apis.FirstOrDefault(a => a.Api.Hero.UserId == userId) ?? CompactApiControl.Empty());
        }

        public static void SaveSettings()
        {
            try
            {
                DarkOrbot.Properties.Settings.Default.DarkOrbotSettings = [];

                foreach (CompactApiControl api in Apis)
                {
                    var settingsObj = JsonConvert.SerializeObject(api.Settings, Newtonsoft.Json.Formatting.Indented);
                    DarkOrbot.Properties.Settings.Default.DarkOrbotSettings.Add(GZipUtilities.ZipToString(settingsObj));
                }

                DarkOrbot.Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void LoadSettings(string injectionKey)
        {
            if (DarkOrbot.Properties.Settings.Default == null)
            {
                DarkOrbot.Properties.Settings.Default?.Reset();
            }
            if (DarkOrbot.Properties.Settings.Default!.DarkOrbotSettings == null)
            {
                DarkOrbot.Properties.Settings.Default!.DarkOrbotSettings = [];
            }

            foreach (var setting in DarkOrbot.Properties.Settings.Default.DarkOrbotSettings)
            {
                if (setting == null)
                {
                    continue;
                }
                try
                {
                    var uncompressed = GZipUtilities.UnzipFromString(setting);
                    DarkOrbotSettings? result = JsonConvert.DeserializeObject<DarkOrbotSettings>(uncompressed);

                    result ??= new DarkOrbotSettings();

                    if (string.IsNullOrEmpty(result.ApiKey))
                    {
                        result.GenerateKey();
                    }

                    var control = new CompactApiControl(result, injectionKey, null);
                    if (Apis.TryGetValue(result.ApiKey, out CompactApiControl? settings))
                    {
                        Apis.Remove(result.ApiKey);
                    }
                    Apis.Add(control);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    HelpTools.WriteErrorLog(ex.Message, ex.ToString());
                    continue;
                }
            }
        }
    }

}
