using System;
using System.Runtime.InteropServices;

namespace InputLayer
{
    /// <summary>
    /// A collection of helpful native API bindings.
    /// </summary>
    static class Unmanaged
    {
        // Touch event window message constants
        internal const Int32 WM_TOUCH = 0x0240;

        // Touch event flags
        internal const Int32 TOUCHEVENTF_MOVE = 0x0001;
        internal const Int32 TOUCHEVENTF_DOWN = 0x0002;
        internal const Int32 TOUCHEVENTF_UP = 0x0004;
        internal const Int32 TOUCHEVENTF_INRANGE = 0x0008;
        internal const Int32 TOUCHEVENTF_PRIMARY = 0x0010;
        internal const Int32 TOUCHEVENTF_NOCOALESCE = 0x0020;
        internal const Int32 TOUCHEVENTF_PEN = 0x0040;

        // Touch input mask values
        internal const Int32 TOUCHINPUTMASKF_TIMEFROMSYSTEM = 0x0001;
        internal const Int32 TOUCHINPUTMASKF_EXTRAINFO = 0x0002;
        internal const Int32 TOUCHINPUTMASKF_CONTACTAREA = 0x0004;

        /// <summary>
        /// Arguments for a touch input point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct TOUCHINPUT
        {
            public Int32 x;
            public Int32 y;
            public IntPtr hSource;
            public Int32 dwID;
            public Int32 dwFlags;
            public Int32 dwMask;
            public Int32 dwTime;
            public IntPtr dwExtraInfo;
            public Int32 cxContact;
            public Int32 cyContact;
        }

        /// <summary>
        /// A 32-bit point based on short values.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTS
        {
            public Int16 x;
            public Int16 y;
        }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean RegisterTouchWindow(IntPtr hWnd, UInt64 ulFlags);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean GetTouchInputInfo(IntPtr hTouchInput, Int32 cInputs, [In, Out] TOUCHINPUT[] pInputs, Int32 cbSize);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern void CloseTouchInputHandle(IntPtr lParam);
    }
}
