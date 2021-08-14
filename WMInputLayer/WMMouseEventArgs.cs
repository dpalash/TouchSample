using System;

namespace InputLayer
{
    /// <summary>
    /// Event arguments for the mouse events.
    /// </summary>
    public sealed class WMMouseEventArgs : EventArgs
    {
        #region ctor

        internal WMMouseEventArgs()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the scrolling delta.
        /// </summary>
        public Double Delta
        { 
            get; 
            internal set;
        }

        /// <summary>
        /// Gets the y coordinate in pixels.
        /// </summary>
        public Double Y 
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the x coordinate in pixels.
        /// </summary>
        public Double X 
        { 
            get;
            internal set;
        }

        /// <summary>
        /// Gets if the right button is clicked.
        /// </summary>
        public Boolean IsRight 
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets if the left button is clicked.
        /// </summary>
        public Boolean IsLeft 
        { 
            get;
            internal set;
        }

        #endregion
    }
}
