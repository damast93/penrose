using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Math;

namespace Penrose
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Triangle> Tiles;

        private readonly SolidColorBrush redbrush = new SolidColorBrush(Color.FromRgb(255, 90, 90));
        private readonly SolidColorBrush bluebrush = new SolidColorBrush(Color.FromRgb(102, 102, 255));
        private readonly SolidColorBrush bdbrush = new SolidColorBrush(Color.FromRgb(50, 50, 50));

        private readonly PenroseTilingGenerator penrose = new PenroseTilingGenerator();

        public MainWindow()
        {
            InitializeComponent();

            var depth = 0;

            ComputeInitialTiling(depth);

            foreach (var tile in Tiles)
            {
                var poly = MakePoly(tile, depth);
                MainCanvas.Children.Add(poly);
            }
        }

        public void ComputeInitialTiling(int initialDepth)
        {
            var a = new Vector(400, 300);
            var r = 250.0;
            var alpha = 36 * PI / 180;

            Tiles = new List<Triangle>();

            for (int i = 0; i < 10; ++i)
            {
                var b = a + new Vector(r * Cos(i*alpha), r * Sin(i*alpha));
                var c = a + new Vector(r * Cos((i+1)*alpha), r * Sin((i+1)*alpha));

                var triangle = (i % 2 == 0)
                                   ? new Triangle(a, b, c, TriangleType.Red)
                                   : new Triangle(a, c, b, TriangleType.Red);

                Tiles.Add(triangle);
            }
            
            for (int i = 0; i < initialDepth; ++i)
            {
                Tiles = penrose.Subdivide(Tiles).ToList();
            }
        }

        private Polygon MakePoly(Triangle triangle, int depth = 0)
        {
            var poly = new Polygon();
            poly.Fill = (triangle.Type == TriangleType.Red) ? redbrush : bluebrush;

            var points = new PointCollection();
            points.Add((Point)triangle.A);
            points.Add((Point)triangle.B);
            points.Add((Point)triangle.C);

            poly.Points = points;
            poly.Stroke = bdbrush;
            poly.StrokeThickness = 2;

            if (depth < 5)
            {
                poly.MouseMove += (sender, args) =>
                {
                    Tiles.Remove(triangle);
                    MainCanvas.Children.Remove(poly);

                    foreach (var sub in penrose.SubdivideTriangle(triangle))
                    {
                        Tiles.Add(sub);
                        MainCanvas.Children.Add(MakePoly(sub, depth + 1));
                    }
                };
            }

            return poly;
        }

 
    }
}
