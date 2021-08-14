using System;

namespace InputLayer
{
    /// <summary>
    /// Event arguments for the touch layer.
    /// </summary>
    public sealed class WMTouchEventArgs : EventArgs
    {
        #region ctor

        internal WMTouchEventArgs()
        {
        }

        #endregion

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

        /// <summary>
        /// Gets if the touch point is the primary contact.
        /// </summary>
        public Boolean IsPrimaryContact
        {
            get { return (Flags & Unmanaged.TOUCHEVENTF_PRIMARY) != 0; }
        }

        #endregion
    }
}
