using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSample.StylusDraw
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
            pt2 = new Point();
            firstId = -1;
            InitializeComponent();

            StylusDown += MainWindow_StylusDown;
            StylusUp += MainWindow_StylusUp;
            StylusMove += MainWindow_StylusMove;
        }
        
        void MainWindow_StylusDown(object sender, StylusEventArgs e)
        {
            if (canvas1 != null)
            {
                var id = e.StylusDevice.Id;
                canvas1.Children.Clear();
                e.StylusDevice.Capture(canvas1);

                // Record the ID of the first Stylus point if it hasn't been recorded. 
                if (firstId == -1)
                    firstId = id;
            }
        }
        
        void MainWindow_StylusMove(object sender, StylusEventArgs e)
        {
            if (canvas1 != null)
            {
                var id = e.StylusDevice.Id;
                var tp = e.GetPosition(canvas1);

                // This is the first Stylus point; just record its position. 
                if (id == firstId)
                {
                    pt1.X = tp.X;
                    pt1.Y = tp.Y;
                }
                else if (id != firstId)
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

        void MainWindow_StylusUp(object sender, StylusEventArgs e)
        {
            var device = e.StylusDevice;

            if (canvas1 != null && device.Captured == canvas1)
                canvas1.ReleaseStylusCapture();
        }
    }
}