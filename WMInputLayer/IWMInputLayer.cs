using System;
using System.ComponentModel;

namespace InputLayer
{
    /// <summary>
    /// Interface to the touch layer.
    /// </summary>
    public interface IWMInputLayer
    {
        /// <summary>
        /// The touch down event.
        /// </summary>
        event EventHandler<WMTouchEventArgs> WMTouchDown;

        /// <summary>
        /// The touch up event.
        /// </summary>
        event EventHandler<WMTouchEventArgs> WMTouchUp;

        /// <summary>
        /// The touch move event.
        /// </summary>
        event EventHandler<WMTouchEventArgs> WMTouchMove;

        /// <summary>
        /// The touch collection changed event.
        /// </summary>
        event EventHandler<WMTouchCollectionEventArgs> WMTouchCollection;

        /// <summary>
        /// The closing event.
        /// </summary>
        event CancelEventHandler Closing;

        /// <summary>
        /// The mouse down event.
        /// </summary>
        event EventHandler<WMMouseEventArgs> WMMouseDown;

        /// <summary>
        /// The mouse move event.
        /// </summary>
        event EventHandler<WMMouseEventArgs> WMMouseMove;

        /// <summary>
        /// The mouse up event.
        /// </summary>
        event EventHandler<WMMouseEventArgs> WMMouseUp;

        /// <summary>
        /// The key down event.
        /// </summary>
        event EventHandler<WMKeyboardEventArgs> WMKeyDown;

        /// <summary>
        /// The key up event.
        /// </summary>
        event EventHandler<WMKeyboardEventArgs> WMKeyUp;

        /// <summary>
        /// Gets or sets if the layer is active.
        /// </summary>
        Boolean Active { get; set; }
    }
}
