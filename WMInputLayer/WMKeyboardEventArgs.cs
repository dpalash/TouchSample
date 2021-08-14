using System;
using System.Windows.Input;

namespace InputLayer
{
    /// <summary>
    /// Keyboard event arguments for the touch layer.
    /// </summary>
    public sealed class WMKeyboardEventArgs : EventArgs
    {
        #region ctor

        internal WMKeyboardEventArgs()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key.
        /// </summary>
        public Key Key 
        { 
            get; 
            internal set; 
        }

        /// <summary>
        /// Gets if the alt key is pressed.
        /// </summary>
        public Boolean IsAlt
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets if the control key is pressed.
        /// </summary>
        public Boolean IsCtrl
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets if the shift key is pressed.
        /// </summary>
        public Boolean IsShift
        {
            get;
            internal set;
        }

        #endregion
    }
}
