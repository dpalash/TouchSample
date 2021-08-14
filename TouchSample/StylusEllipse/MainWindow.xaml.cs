using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSample.StylusEllipse
{
    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
    public partial class MainWindow : Window
    {
        const Int32 radius = 50;
        Dictionary<Ellipse, Int32> TouchPoints;

        public MainWindow()
        {
            TouchPoints = new Dictionary<Ellipse, Int32>();

            for (int i = 0; i < 10; i++)
            {
                TouchPoints[new Ellipse()
                {
                    Height = radius,
                    Width = radius,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.LightBlue,
                    RenderTransform = new TranslateTransform(-radius / 2, -radius / 2)
                }] = -1;
            }

            InitializeComponent();

            StylusDown += MainWindow_StylusDown;
            StylusUp += MainWindow_StylusUp;
            StylusMove += MainWindow_StylusMove;
        }
        
        private void MainWindow_StylusDown(object sender, StylusEventArgs e)
        {
            if (canvas1 != null)
            {
                var device = e.StylusDevice;

                device.Capture(canvas1);

                if (!TouchPoints.ContainsValue(device.Id))
                {
                    var ellipse = TouchPoints.FirstOrDefault(iE => iE.Value < 0);

                    if (ellipse.Key != null)
                    {
                        canvas1.Children.Add(ellipse.Key);
                        TouchPoints[ellipse.Key] = device.Id;
                    }
                }
            }
        }

        
        private void MainWindow_StylusMove(object sender, StylusEventArgs e)
        {
            if (canvas1 != null)
            {
                var device = e.StylusDevice;
                var temp = e.GetStylusPoints(canvas1).Last();
                var tp = new Point(temp.X, temp.Y);

                if (TouchPoints.ContainsValue(device.Id))
                {
                    var ellipse = TouchPoints.First(iE => iE.Value == device.Id);
                    Canvas.SetLeft(ellipse.Key, tp.X);
                    Canvas.SetTop(ellipse.Key, tp.Y);
                }
            }
        }

        
        private void MainWindow_StylusUp(object sender, StylusEventArgs e)
        {
            var device = e.StylusDevice;

            if (canvas1 != null && device.Captured == canvas1)
            {
                if (TouchPoints.ContainsValue(device.Id))
                {
                    var ellipse = TouchPoints.First(iE => iE.Value == device.Id);
                    canvas1.Children.Remove(ellipse.Key);
                    TouchPoints[ellipse.Key] = -1;
                }
            }
        }
    }
}