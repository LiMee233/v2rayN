using Microsoft.Win32;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace v2rayN
{
    internal class UI
    {
        private static readonly string caption = "v2rayN";

        public static void Show(string msg)
        {
            MessageBox.Show(msg, caption, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        public static MessageBoxResult ShowYesNo(string msg)
        {
            return MessageBox.Show(msg, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static bool? OpenFileDialog(out string fileName, string filter)
        {
            fileName = string.Empty;

            var fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = filter
            };

            if (fileDialog.ShowDialog() != true)
            {
                return false;
            }
            fileName = fileDialog.FileName;

            return true;
        }

        [DllImport("DWMAPI")]
        public static extern nint DwmSetWindowAttribute(nint hwnd, DwmWindowAttribute attribute, nint dataPointer, uint dataSize);

        [DllImport("DWMAPI")]
        public static extern nint DwmExtendFrameIntoClientArea(nint hwnd, ref Margins margins);


        public enum DwmWindowAttribute
        {
            NCRENDERING_ENABLED,
            NCRENDERING_POLICY,
            TRANSITIONS_FORCEDISABLED,
            ALLOW_NCPAINT,
            CAPTION_BUTTON_BOUNDS,
            NONCLIENT_RTL_LAYOUT,
            FORCE_ICONIC_REPRESENTATION,
            FLIP3D_POLICY,
            EXTENDED_FRAME_BOUNDS,
            HAS_ICONIC_BITMAP,
            DISALLOW_PEEK,
            EXCLUDED_FROM_PEEK,
            CLOAK,
            CLOAKED,
            FREEZE_REPRESENTATION,
            PASSIVE_UPDATE_MODE,
            USE_HOSTBACKDROPBRUSH,

            // 表示是否使用暗色模式, 它会将窗体的模糊背景调整为暗色
            USE_IMMERSIVE_DARK_MODE = 20,
            WINDOW_CORNER_PREFERENCE = 33,
            BORDER_COLOR,
            CAPTION_COLOR,
            TEXT_COLOR,
            VISIBLE_FRAME_BORDER_THICKNESS,

            // 背景类型, 值可以是: 自动, 无, 云母, 或者亚克力
            SYSTEMBACKDROP_TYPE,
            LAST
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            public int LeftWidth;
            public int RightWidth;
            public int TopHeight;
            public int BottomHeight;
        }

        public static unsafe void SetMicaBackground(Window window)
        {
            // 取得窗口句柄
            var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);

            // 设置 HwndSource.CompositionTarget.BackgroundColor 为透明
            hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;

            // 设置边框
            var margins = new Margins()
            {
                LeftWidth = -1,
                TopHeight = -1,
                RightWidth = -1,
                BottomHeight = -1
            };

            DwmExtendFrameIntoClientArea(hwndSource.Handle, ref margins);

            // 设置背景
            var backdrop = (int)2;

            DwmSetWindowAttribute(hwndSource.Handle, DwmWindowAttribute.SYSTEMBACKDROP_TYPE, (nint)(void*)&backdrop, sizeof(int));
        }
    }
}