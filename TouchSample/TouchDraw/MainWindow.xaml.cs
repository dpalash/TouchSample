using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSample.TouchDraw
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
            TouchDown += MainWindow_TouchDown;
            TouchUp += MainWindow_TouchUp;
            TouchMove += MainWindow_TouchMove;
        }

        private void MainWindow_TouchDown(object sender, TouchEventArgs e)
        {
            if (canvas1 != null)
            {
                canvas1.Children.Clear();
                e.TouchDevice.Capture(canvas1);

                // Record the ID of the first touch point if it hasn't been recorded. 
                if (firstId == -1)
                    firstId = e.TouchDevice.Id;
            }
        }

        private void MainWindow_TouchMove(object sender, TouchEventArgs e)
        {
            if (canvas1 != null)
            {
                var tp = e.GetTouchPoint(canvas1);

                if (e.TouchDevice.Id == firstId)
                {
                    pt1.X = tp.Position.X;
                    pt1.Y = tp.Position.Y;
                }
                else if (e.TouchDevice.Id != firstId)
                {
                    pt2.X = tp.Position.X;
                    pt2.Y = tp.Position.Y;

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

        void MainWindow_TouchUp(object sender, TouchEventArgs e)
        {
            if (canvas1 != null && e.TouchDevice.Captured == canvas1)
                canvas1.ReleaseTouchCapture(e.TouchDevice);
        }
    }
}