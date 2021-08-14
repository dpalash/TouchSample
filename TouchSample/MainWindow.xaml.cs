using System;
using System.Windows;

namespace TouchSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OldTouchClicked(object sender, RoutedEventArgs e)
        {
            new TouchDraw.MainWindow().ShowDialog();
        }

        void OldTouchStylusClicked(Object sender, RoutedEventArgs e)
        {
            new StylusDraw.MainWindow().ShowDialog();
        }

        void OldTouchWMClicked(Object sender, RoutedEventArgs e)
        {
            new LayerDraw.MainWindow().ShowDialog();
        }

        void DisabledTouchClicked(Object sender, RoutedEventArgs e)
        {
            new DisableDraw.MainWindow().ShowDialog();
        }

        void NewTouchClicked(Object sender, RoutedEventArgs e)
        {
            new StylusEllipse.MainWindow().ShowDialog();
        }
    }
}
