using System;

namespace InputLayer
{
    /// <summary>
    /// Event arguments for the touch events.
    /// </summary>
    public sealed class WMTouchCollectionEventArgs : EventArgs
    {
        #region ctor

        internal WMTouchCollectionEventArgs()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the touch x client coordinates in pixels.
        /// </summary>
        public Double[] LocationX
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the touch y client coordinates in pixels.
        /// </summary>
        public Double[] LocationY
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the contact IDs.
        /// </summary>
        public Int32[] Id
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

        #endregion
    }
}
