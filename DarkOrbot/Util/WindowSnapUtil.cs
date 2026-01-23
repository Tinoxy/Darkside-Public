using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace DarkOrbot.Util
{
    public class WindowSnapUtil
    {
        private struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }

            public readonly int Height => Bottom - Top;

            public static bool operator !=(Rect r1, Rect r2)
            {
                return !(r1 == r2);
            }

            public static bool operator ==(Rect r1, Rect r2)
            {
                return r1.Left == r2.Left && r1.Right == r2.Right && r1.Top == r2.Top && r1.Bottom == r2.Bottom;
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        private readonly DispatcherTimer _timer;
        private readonly WindowInteropHelper _windowHelper;
        private readonly Window _window;
        private readonly Window _attachTo;

        private IntPtr _windowHandle;
        private Rect _lastBounds;

        public WindowSnapUtil(Window window, Window attachTo)
        {
            _window = window;
            _window.Topmost = true;
            _attachTo = attachTo;
            _windowHelper = new WindowInteropHelper(attachTo);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            _timer.Tick += (x, y) => SnapToWindow();
            _timer.IsEnabled = false;
        }

        public void Attach()
        {
            _windowHandle = _windowHelper.Handle;
            _timer.Start();
        }

        public void Detach()
        {
            _timer.Stop();
        }

        private void SnapToWindow()
        {
            var bounds = GetWindowBounds(_windowHandle);

            if (bounds != _lastBounds)
            {
                _window.Top = bounds.Top;
                _window.Left = bounds.Left - _window.Width;
                _window.Height = bounds.Height;
                _lastBounds = bounds;
            }
        }

        private Rect GetWindowBounds(IntPtr handle)
        {
            Rect bounds = new Rect();
            GetWindowRect(handle, ref bounds);
            return bounds;
        }
    }
}
