namespace DarksideApi.Util.Extensions
{
    public static class DoubleExtension
    {
        /// <summary>
        /// Inverts the given Angle by subtracting it from half of a circle
        /// </summary>
        /// <param name="value">angle to be inverted</param>
        /// <returns></returns>
        public static double InvertAngle(this double value)
        {
            return (180 - value);
        }

        public static string KiloFormat(this double num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            return num >= 10000 ? (num / 1000).ToString("0.#") + "K" : num < 1 && num > 0 ? "<1" : num.ToString("#,0");
        }

        public static double ToRadians(this double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
