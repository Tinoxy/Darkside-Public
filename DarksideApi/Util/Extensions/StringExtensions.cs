namespace DarksideApi.Util.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceAll(this string text, string charsToReplace, string replacement)
        {
            var chars = charsToReplace.ToCharArray();
            foreach (var c in chars)
            {
                text = text.Replace(c.ToString(), replacement);
            }
            return text;
        }
    }
}
