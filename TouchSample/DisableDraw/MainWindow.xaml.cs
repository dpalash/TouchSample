using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSample.DisableDraw
{
    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
    public partial class MainWindow : Window
    {
        // Variables to track the first two touch points  
        // and the ID of the first touch point. 
        Point pt1;
        Point pt2;
        Int32 firstId;

        public MainWindow()
        {
            firstId = -1;
            pt2 = new Point();
            InitializeComponent();
            //Disable usual stylus tablet support
            DisableWPFTabletSupport();
        }

        #region Handle Events

        void TouchDown(TouchEventArgs e)
        {
            if (canvas1 != null)
            {
                canvas1.Children.Clear();

                // Record the ID of the first touch point if it hasn't been recorded. 
                if (firstId == -1)
                    firstId = e.Id;
            }
        }

        void TouchMove(TouchEventArgs e)
        {
            if (canvas1 != null)
            {
                var tp = new Point(e.LocationX, e.LocationY);

                if (e.Id == firstId)
                {
                    pt1.X = tp.X;
                    pt1.Y = tp.Y;
                }
                else if (e.Id != firstId)
                {
                    pt2.X = tp.X;
                    pt2.Y = tp.Y;

                    canvas1.Children.Add(new Line
                    {
                        Stroke = new RadialGradientBrush(Colors.White, Colors.Black),
                        X1 = pt1.X,
                        X2 = pt2.X,
                        Y1 = pt1.Y,
                        Y2 = pt2.Y,
                        StrokeThickness = 2
                    });
                }
            }
        }

        void TouchUp(TouchEventArgs e)
        {
        }

        #endregion

        #region Arguments
        
        class TouchEventArgs : EventArgs
        {

        #region Properties

        /// <summary>
        /// Gets the touch x client coordinate in pixels.
        /// </summary>
        public Double LocationX
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the touch y client coordinate in pixels.
        /// </summary>
        public Double LocationY
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the contact ID.
        /// </summary>
        public Int32 Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total number of current touch points.
        /// </summary>
        public Int32 Count
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the given flags.
        /// </summary>
        public Int32 Flags
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the mask which fields in the structure are valid.
        /// </summary>
        public Int32 Mask
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the touch event time.
        /// </summary>
        public Int32 Time
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the x size of the contact area in pixels.
        /// </summary>
        public Double ContactX
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the y size of the contact area in pixels.
        /// </summary>
        public Double ContactY
        {
            get;
            internal set;
        }

        #endregion
    }

        #endregion

        #region Disable WPF tablet support

        static void DisableWPFTabletSupport()
        {
            // Get a collection of the tablet devices for this window.  
            var devices = Tablet.TabletDevices;

            if (devices.Count > 0)
            {
                // Get the Type of InputManager.
                var inputManagerType = typeof(InputManager);

                // Call the StylusLogic method on the InputManager.Current instance.
                var stylusLogic = inputManagerType.InvokeMember("StylusLogic",
                            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, InputManager.Current, null);

                if (stylusLogic != null)
                {
                    //  Get the type of the stylusLogic returned from the call to StylusLogic.
                    var stylusLogicType = stylusLogic.GetType();

                    // Loop until there are no more devices to remove.
                    while (devices.Count > 0)
                    {
                        // Remove the first tablet device in the devices collection.
                        stylusLogicType.InvokeMember("OnTabletRemoved",
                                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                                null, stylusLogic, new object[] { (uint)0 });
                    }
                }

            }
        }

        #endregion

        #region Enable touch

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            RegisterTouchWindow(source.Handle, 0);
        }

        IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            if (msg == 0x0240)
            {
                handled = HandleTouch(wParam, lParam);
                return new IntPtr(1);
            }

            return IntPtr.Zero;
        }

        #endregion

        #region Windows API

        static readonly Int32 touchInputSize = Marshal.SizeOf(new TOUCHINPUT());

        [StructLayout(LayoutKind.Sequential)]
        struct TOUCHINPUT
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

        [DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean RegisterTouchWindow(IntPtr hWnd, UInt64 ulFlags);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean GetTouchInputInfo(IntPtr hTouchInput, Int32 cInputs, [In, Out] TOUCHINPUT[] pInputs, Int32 cbSize);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern void CloseTouchInputHandle(IntPtr lParam);

        #endregion

        #region Helpers

        static Int32 LoWord(Int32 number)
        {
            return number & 0xffff;
        }

        Boolean HandleTouch(IntPtr wParam, IntPtr lParam)
        {
            var inputCount = LoWord(wParam.ToInt32());
            var inputs = new TOUCHINPUT[inputCount];

            if (!GetTouchInputInfo(lParam, inputCount, inputs, touchInputSize))
                return false;

            var handled = false;

            for (int i = 0; i < inputCount; i++)
            {
                var input = inputs[i];

                // Assign a handler to this message.
                Action<TouchEventArgs> handler = null;

                if ((input.dwFlags & 0x0002) != 0)
                    handler = TouchDown;
                else if ((input.dwFlags & 0x0004) != 0)
                    handler = TouchUp;
                else if ((input.dwFlags & 0x0001) != 0)
                    handler = TouchMove;

                // Convert message parameters into touch event arguments and handle the event.
                if (handler != null)
                {
                    var pt = PointFromScreen(new Point(input.x * 0.01, input.y * 0.01));

                    handler(new TouchEventArgs
                    {
                        // TOUCHINFO point coordinates and contact size is in 1/100 of a pixel; convert it to pixels.
                        // Also convert screen to client coordinates.
                        ContactY = input.cyContact * 0.01,
                        ContactX = input.cxContact * 0.01,
                        Id = input.dwID,
                        LocationX = pt.X,
                        LocationY = pt.Y,
                        Time = input.dwTime,
                        Mask = input.dwMask,
                        Flags = input.dwFlags,
                        Count = inputCount
                    });

                    handled = true;
                }
            }

            CloseTouchInputHandle(lParam);
            return handled;
        }

        #endregion
    }
}