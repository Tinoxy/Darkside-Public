namespace DarkOrbot.Licensing
{
    internal static class LicenseMethods
    {
        private static readonly LicenseHttpClient httpClient;

        static LicenseMethods()
        {
            httpClient = new();
        }

        internal static LicenseObject Fetch(string uid)
        {
            return new LicenseObject(DateTime.UtcNow, DateTime.MaxValue, LicenseType.Admin, "9186-6716-9999-9502", "41");
        }

        internal static string GetServerVersion()
        {
            return "1.0.950";
        }

        internal static string GetServerTrackingVersion()
        {
            return "10.0.90998538";
        }

        internal static void IsNewVersionAvailable()
        {
            return;
        }
    }
}
