using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DarkOrbot.Util
{
    public static class HelpTools
    {
        [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
        private static extern bool IsOS(int os);

        /*[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string lpszUrl, string lpszCookieName, string lpszCookieData);*/
        [DllImport("wininet.dll")]
        static extern InternetCookieState InternetSetCookieEx(
    string lpszURL,
    string lpszCookieName,
    string lpszCookieData,
    int dwFlags,
    int dwReserved);

        enum InternetCookieState : int
        {
            COOKIE_STATE_UNKNOWN = 0x0,
            COOKIE_STATE_ACCEPT = 0x1,
            COOKIE_STATE_PROMPT = 0x2,
            COOKIE_STATE_LEASH = 0x3,
            COOKIE_STATE_DOWNGRADE = 0x4,
            COOKIE_STATE_REJECT = 0x5,
            COOKIE_STATE_MAX = COOKIE_STATE_REJECT
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }

        private enum KeyType
        {
            api = 0,
        }

        static HelpTools()
        {
        }

        private static byte[] GetKey(KeyType keyType = KeyType.api)
        {
            return [0];
        }

        public static byte[] GetArrayFromKeyInfo(string keyInfo)
        {
            if (keyInfo.Length <= 0)
            {
                return [];
            }
            var toBeConverted = keyInfo.Split(',');
            var toBeReplaced = new byte[toBeConverted.Length];

            for (var i = 0; i < toBeReplaced.Length; i++)
            {
                toBeReplaced[i] = (byte)int.Parse(toBeConverted[i]);
            }

            return toBeReplaced;
        }

        public static bool IsWindowsServer()
        {
            return IsOS(29);
        }

        public static string GetMD5(string toHash)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(toHash);
            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        public static Brush GetBrushFromHTML(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = 255;
            byte r;
            byte g;
            byte b;
            switch (hex.Length)
            {
                case < 8:
                    r = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
                    g = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
                    b = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
                    break;
                default:
                    a = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
                    r = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
                    g = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
                    b = (byte)Convert.ToUInt32(hex.Substring(6, 2), 16);
                    break;
            }
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public static string FormatNumber(long num)
        {
            if (num >= 100000000)
                return FormatNumber(num / 1000000) + "M";

            return num >= 100000 ? FormatNumber(num / 1000) + "K" : num >= 10000 ? (num / 1000D).ToString("0.#") + "K" : num.ToString("#,0");
        }

        public static bool SetCookie(string url, string? cookieName, string cookieData)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(cookieData))
                return false;
            return InternetSetCookieEx(url, cookieName!, cookieData, 0, 0) == InternetCookieState.COOKIE_STATE_ACCEPT;//InternetSetCookie(url, cookieName, cookieData);
        }

        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            var sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                var IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                var IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }

        public static string GetTimeStamp()
        {
            return $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}-{DateTime.UtcNow.Day} {DateTime.UtcNow.Hour}:{DateTime.UtcNow.Minute}:{DateTime.UtcNow.Second}";
        }

        public static BitmapImage LoadImageFromByteArray(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) 
                return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static BitmapImage GetMinimapImage(int mapId)
        {
            return mapId <= 0
                ? new BitmapImage()
                : new BitmapImage(new Uri($"https://darkorbit-22.bpsecure.com/spacemap/graphics/minimaps/minimap-{mapId}-300.jpg"));
        }

        //https://darkorbit-22.bpsecure.com/do_img/global/items/ship/solace-plus/design/solace-plus-asimov_100x100.png?__cv=f351410bc11a65e6a9f83226148f8400?__cv=
        public static BitmapImage GetShipImage(string shipName)
        {
            return string.IsNullOrEmpty(shipName)
                ? new BitmapImage()
                : new BitmapImage(
                new Uri($"{"https://darkorbit-22.bpsecure.com/do_img/global/items/ship/"}{shipName.Replace("ship_", "").Replace('_', '/')}_100x100.png"));
        }

        /// <summary>
        /// Read the input data, crypting/decrypting it using XOR
        /// </summary>
        /// <param name="input">data to crypt/decrypt</param>
        /// <param name="key">pass</param>
        /// <returns></returns>
        private static string ReadData(string input, string key)
        {
            StringBuilder sb = new();
            for (var i = 0; i < input.Length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));

            return sb.ToString();
        }

        public static byte[] LoadApiAssembly(byte[] key)
        {
            return null;
        }
    }
}
