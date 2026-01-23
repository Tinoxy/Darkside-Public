namespace DarksideApi.Util.Extensions
{
    public static class LongExtension
    {
        public static string KiloFormat(this long num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            return num >= 100000 ? (num / 1000).ToString("#,0K") : num >= 10000 ? (num / 1000).ToString("0.#") + "K" : num.ToString("#,0");
        }
    }
}
