using System.Windows;

namespace InputLayer
{
    /// <summary>
    /// Creates an input layer for the given canvas.
    /// </summary>
    public static class WMInputLayerFactory
    {
        /// <summary>
        /// The input layer creator.
        /// </summary>
        /// <param name="element">The element to cover.</param>
        /// <returns>The interface for the layer.</returns>
        public static IWMInputLayer Create(FrameworkElement element)
        {
            return new WMInputLayer(element);
        }
    }
}
