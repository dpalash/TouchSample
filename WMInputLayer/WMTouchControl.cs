using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using win = System.Windows;

namespace InputLayer
{
    /// <summary>
    /// Form to act as invisible touch input layer.
    /// </summary>
    sealed class WMInputLayer : Form, IWMInputLayer
    {
        #region Fields

        static readonly Int32 touchInputSize = Marshal.SizeOf(new Unmanaged.TOUCHINPUT());

        win.FrameworkElement parent;
        Boolean active;
        Boolean intitialized;

        #endregion

        #region Events

        // Touch event handlers
        public event EventHandler<WMTouchEventArgs> WMTouchDown;
        public event EventHandler<WMTouchEventArgs> WMTouchUp;
        public event EventHandler<WMTouchEventArgs> WMTouchMove;

        public event EventHandler<WMTouchCollectionEventArgs> WMTouchCollection;

        // Mouse event handlers
        public event EventHandler<WMMouseEventArgs> WMMouseDown;
        public event EventHandler<WMMouseEventArgs> WMMouseMove;
        public event EventHandler<WMMouseEventArgs> WMMouseUp;

        // Keyboard event handlers
        public event EventHandler<WMKeyboardEventArgs> WMKeyDown;
        public event EventHandler<WMKeyboardEventArgs> WMKeyUp;

        #endregion

        #region ctor

        [SecurityPermission(SecurityAction.Demand)]
        public WMInputLayer(win.FrameworkElement canvas)
        {
            Active = false;
            parent = canvas;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            AllowTransparency = true;
            Opacity = 1e-2;
            TopMost = true;
            ShowInTaskbar = false;
            InstallHandlers();
            Size = new Size((Int32)parent.ActualWidth, (Int32)parent.ActualHeight);
            AdjustPosition();
        }

        #endregion

        #region Properties

        public Boolean Active
        {
            get { return active; }
            set
            {
                active = value;

                if (value) ShowAbove();
                else Hide();
            }
        }

        #endregion

        #region Methods

        public void ShowAbove()
        {
            if (!intitialized)
            {
                var parentWindow = win.Window.GetWindow(parent);
                intitialized = true;
                parentWindow.Activated += (sender, e) => { if (Active) ShowAbove(); };
                parentWindow.GotKeyboardFocus += (sender, e) => { if (Active) ShowAbove(); };
                GotFocus += (sender, e) => { parentWindow.Activate(); };
            }

            TopMost = true;
            TopMost = false;
            Show();
            Focus();
        }

        public void AdjustPosition()
        {
            try
            {
                var _parentPosition = parent.PointToScreen(new win.Point(0, 0));
                Location = new Point((Int32)_parentPosition.X, (Int32)_parentPosition.Y);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Handlers

        void InstallHandlers()
        {
            parent.SizeChanged += ParentSizeChanged;
            Load += OnLoadHandler;
            KeyDown += LayerKeyDown;
            KeyUp += LayerKeyUp;
            MouseDown += LayerMouseDown;
            MouseUp += LayerMouseUp;
            MouseMove += LayerMouseMove;
        }

        void UninstallHandlers()
        {
            parent.SizeChanged -= ParentSizeChanged;
            Load -= OnLoadHandler;
            KeyDown -= LayerKeyDown;
            KeyUp -= LayerKeyUp;
            MouseDown -= LayerMouseDown;
            MouseUp -= LayerMouseUp;
            MouseMove -= LayerMouseMove;
        }

        void ParentSizeChanged(Object sender, win.SizeChangedEventArgs e)
        {
            Size = new Size((Int32)e.NewSize.Width, (Int32)e.NewSize.Height);
            AdjustPosition();
        }

        void LayerMouseMove(Object sender, MouseEventArgs e)
        {
            if (WMMouseMove != null)
            {
                WMMouseMove(this, new WMMouseEventArgs
                {
                    IsLeft = e.Button == MouseButtons.Left,
                    IsRight = e.Button == MouseButtons.Right,
                    X = e.X,
                    Y =  e.Y,
                    Delta = e.Delta
                });
            }
        }

        void LayerMouseUp(Object sender, MouseEventArgs e)
        {
            if (WMMouseUp != null)
            {
                WMMouseUp(this, new WMMouseEventArgs
                {
                    IsLeft = e.Button == MouseButtons.Left,
                    IsRight = e.Button == MouseButtons.Right,
                    X = e.X,
                    Y = e.Y,
                    Delta = e.Delta
                });
            }
        }

        void LayerMouseDown(Object sender, MouseEventArgs e)
        {
            if (WMMouseDown != null)
            {
                WMMouseDown(this, new WMMouseEventArgs
                {
                    IsLeft = e.Button == MouseButtons.Left,
                    IsRight = e.Button == MouseButtons.Right,
                    X = e.X,
                    Y = e.Y,
                    Delta = e.Delta
                });
            }
        }

        void LayerKeyUp(Object sender, KeyEventArgs e)
        {
            if (WMKeyUp != null)
            {
                WMKeyUp(this, new WMKeyboardEventArgs
                {
                    Key = WPFKey(e.KeyCode),
                    IsAlt = e.Alt,
                    IsCtrl = e.Control,
                    IsShift = e.Shift
                });
            }
        }

        void LayerKeyDown(Object sender, KeyEventArgs e)
        {
            if (WMKeyDown != null)
            {
                WMKeyDown(this, new WMKeyboardEventArgs
                {
                    Key = WPFKey(e.KeyCode),
                    IsAlt = e.Alt,
                    IsCtrl = e.Control,
                    IsShift = e.Shift
                });
            }
        }

        #endregion

        #region Overrides

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            var handled = false;

            if (m.Msg == Unmanaged.WM_TOUCH)
                handled = HandleTouch(ref m);

            base.WndProc(ref m);

            if (handled)
                m.Result = new IntPtr(1);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (!Active)
                Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            win.Application.Current.Shutdown();
            base.OnClosing(e);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Extracts the lower 16-bit word from a signed 32-bit integer.
        /// </summary>
        /// <param name="number">The integer number.</param>
        /// <returns>The lower word part.</returns>
        static Int32 LoWord(Int32 number)
        {
            return number & 0xffff;
        }

        static win.Input.Key WPFKey(Keys key)
        {
            var keyCode = (Int32)key;
            var value = 0;

            if (key == Keys.ControlKey || key == (Keys.LButton | Keys.ShiftKey | Keys.Control) || key == Keys.LControlKey)
                return System.Windows.Input.Key.LeftCtrl;
            if (keyCode == 0)
                value = keyCode;
            else if (keyCode == 3)
                value = 1;
            else if (keyCode <= 7)
                value = 0;
            else if (keyCode <= 10)
                value = keyCode - 6;
            else if (keyCode <= 13)
                value = keyCode - 7;
            else if (keyCode <= 21)
                value = keyCode - 12;
            else if (keyCode <= 25)
                value = keyCode - 13;
            else if (keyCode <= 57)
                value = keyCode - 14;
            else if (keyCode <= 93)
                value = keyCode - 21;
            else if (keyCode <= 135)
                value = keyCode - 22;
            else if (keyCode <= 145)
                value = keyCode - 30;
            else if (keyCode <= 183)
                value = keyCode - 44;
            else if (keyCode <= 192)
                value = keyCode - 46;
            else if (keyCode <= 223)
                value = keyCode - 70;
            else if (keyCode <= 226)
                value = keyCode - 72;
            else if (keyCode <= 229)
                value = keyCode - 74;
            else if (keyCode <= 231)
                value = keyCode - 75;
            else if (keyCode <= 254)
                value = keyCode - 83;

            return (win.Input.Key)value;
        }

        void OnLoadHandler(Object sender, EventArgs e)
        {
            try
            {
                if (!Unmanaged.RegisterTouchWindow(Handle, 0))
                    Debug.WriteLine("ERROR: Could not register window for multi-touch");
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                MessageBox.Show("RegisterTouchWindow API seems to be unavailable.", "Critical error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }

        /// <summary>
        /// Decodes and handles the WM_TOUCH message.
        /// </summary>
        /// <param name="m">The window message.</param>
        /// <returns>True if the message has been handled, otherwise false.</returns>
        Boolean HandleTouch(ref Message m)
        {
            var inputCount = LoWord(m.WParam.ToInt32());
            var inputs = new Unmanaged.TOUCHINPUT[inputCount];

            if (!Unmanaged.GetTouchInputInfo(m.LParam, inputCount, inputs, touchInputSize))
                return false;

            var handled = false;

            for (int i = 0; i < inputCount; i++)
            {
                var input = inputs[i];

                // Assign a handler to this message.
                EventHandler<WMTouchEventArgs> handler = null;

                if ((input.dwFlags & Unmanaged.TOUCHEVENTF_DOWN) != 0)
                    handler = WMTouchDown;
                else if ((input.dwFlags & Unmanaged.TOUCHEVENTF_UP) != 0)
                    handler = WMTouchUp;
                else if ((input.dwFlags & Unmanaged.TOUCHEVENTF_MOVE) != 0)
                    handler = WMTouchMove;

                // Convert message parameters into touch event arguments and handle the event.
                if (handler != null)
                {
                    var pt = parent.PointFromScreen(new win.Point(input.x * 0.01, input.y * 0.01));
                    var te = new WMTouchEventArgs
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
                    };

                    handler(this, te);
                    handled = true;
                }
            }

            if (WMTouchCollection != null)
            {
                var nPointsDownOrMove = 0;

                for (int i = 0; i < inputCount; i++)
                {
                    var input = inputs[i];

                    if ((input.dwFlags & Unmanaged.TOUCHEVENTF_DOWN) != 0 || (input.dwFlags & Unmanaged.TOUCHEVENTF_MOVE) != 0)
                        nPointsDownOrMove++;
                }

                var tce = new WMTouchCollectionEventArgs
                {
                    Count = nPointsDownOrMove,
                    LocationX = new Double[nPointsDownOrMove],
                    LocationY = new Double[nPointsDownOrMove],
                    Id = new Int32[nPointsDownOrMove]
                };

                for (int p = 0, i = 0; p < inputCount; p++)
                {
                    var input = inputs[p];

                    if ((input.dwFlags & Unmanaged.TOUCHEVENTF_DOWN) != 0 || (input.dwFlags & Unmanaged.TOUCHEVENTF_MOVE) != 0)
                    {
                        var pt = parent.PointFromScreen(new win.Point(input.x * 0.01, input.y * 0.01));
                        tce.Id[i] = input.dwID;
                        tce.LocationX[i] = pt.X;
                        tce.LocationY[i] = pt.Y;
                        i++;
                    }
                }

                WMTouchCollection(this, tce);
                handled = true;
            }

            Unmanaged.CloseTouchInputHandle(m.LParam);
            return handled;
        }

        #endregion
    }
}
