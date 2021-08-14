using InputLayer;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSample.LayerDraw
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

            Loaded += (s, ev) =>
            {
                //Keep reference to the following object if you want to de-activate it at some point
                var layer = WMInputLayerFactory.Create(canvas1);
                layer.WMTouchDown += layer_WMTouchDown;
                layer.WMTouchMove += layer_WMTouchMove;
                layer.Active = true;
            };
        }
        
        void layer_WMTouchDown(object sender, WMTouchEventArgs e)
        {
            if (canvas1 != null)
            {
                canvas1.Children.Clear();
                var id = e.Id;

                // Record the ID of the first Stylus point if it hasn't been recorded. 
                if (firstId == -1)
                    firstId = id;
            }
        }
        
        void layer_WMTouchMove(object sender, WMTouchEventArgs e)
        {
            if (canvas1 != null)
            {
                var id = e.Id;
                var tp = new Point(e.LocationX, e.LocationY);

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
    }
}