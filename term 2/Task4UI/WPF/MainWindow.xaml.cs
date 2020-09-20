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
using MathLibrary;
using Ellipse = MathLibrary.Ellipse;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double pixelsPerUnit;
        private double width, height;
        private bool buildButtonWasClicked = false;
        private double scale = 10;
        public MainWindow()
        {
            InitializeComponent();
            AbstractCurve[] abstractCurves = new AbstractCurve[]
            {   
                new Ellipse(2, 1),
                new Parabola(2),
                new Hyperbola(1, 1)
            };
            collectionOfCurves.ItemsSource = abstractCurves;
        }
        private void BuildLine(double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = 1;
            line.Stroke = Brushes.Black;
            graphArea.Children.Add(line);
        }

        private void buildButton_Click(object sender, RoutedEventArgs e)
        {
            graphArea.Children.Clear();
            BuildCoordinateSystem();
            AbstractCurve curveToDraw = (AbstractCurve)collectionOfCurves.SelectedItem;
            if (curveToDraw != null)
            {
                buildButtonWasClicked = true;
                BuildGraph(curveToDraw);
            }
        }
        private void BuildGraph(AbstractCurve curveToDraw)
        {
            Polyline graph = new Polyline();
            List<double[]> points = curveToDraw.GetPointsToDraw(pixelsPerUnit, (int)width, (int)height);
            PointCollection pointsToDraw = new PointCollection();
            foreach (double[] point in points)
            {
                pointsToDraw.Add(new Point(point[0], point[1]));
            }
            graph.Points = pointsToDraw;
            graph.StrokeThickness = 1;
            graph.Stroke = Brushes.Black;
            graphArea.Children.Add(graph);

            if (curveToDraw.Name == "Hyperbola")
            {
                Polyline symmetryGraph = new Polyline();
                PointCollection symmetryPoints = new PointCollection();
                foreach (double[] point in points)
                {
                    symmetryPoints.Add(new Point(width - point[0], point[1]));
                }
                symmetryGraph.Points = symmetryPoints;
                symmetryGraph.StrokeThickness = 1;
                symmetryGraph.Stroke = Brushes.Black;
                graphArea.Children.Add(symmetryGraph);
            }
        }
        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (buildButtonWasClicked)
                buildButton_Click(sender, e);
        }

        private void plusScaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (buildButtonWasClicked)
            {
                scale += 1;
                boxScale.Text = (scale / 10).ToString();
                BuildCoordinateSystem();
                buildButton_Click(sender, e);
            }
        }

        private void minusScaleButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (buildButtonWasClicked)
            {
                if (scale == 5)
                {
                    MessageBox.Show("Scale is minimal");
                }
                else
                    scale -= 1;
                boxScale.Text = (scale / 10).ToString();
                BuildCoordinateSystem();
                buildButton_Click(sender, e);
            }
        }

        private void BuildCoordinateSystem()
        {

            width = graphArea.ActualWidth;
            height = graphArea.ActualHeight;
            pixelsPerUnit = width / 15 * scale / 10;
            BuildLine(0, height / 2, width, height / 2);
            BuildLine(width / 2, 0, width / 2, height);

            double xCentre = width / 2;
            for (double i = pixelsPerUnit; xCentre < width - pixelsPerUnit; i += pixelsPerUnit)
            {
                xCentre += pixelsPerUnit;
                TextBlock unitPlus = new TextBlock();
                unitPlus.Text = ((int)Math.Round(i / pixelsPerUnit)).ToString();
                unitPlus.FontSize = 10;
                unitPlus.Margin = new Thickness(xCentre, height / 2, width - xCentre, height / 2);
                BuildLine(xCentre, height / 2 - 2, xCentre, height / 2 + 2);

                TextBlock unitMinus = new TextBlock();
                unitMinus.Text = ((int)Math.Round(-i / pixelsPerUnit)).ToString();
                unitMinus.FontSize = 10;
                unitMinus.Margin = new Thickness(width / 2 - i, height / 2, xCentre, height / 2);
                BuildLine(width / 2 - i, height / 2 - 2, width / 2 - i, height / 2 + 2);
                graphArea.Children.Add(unitPlus);
                graphArea.Children.Add(unitMinus);
                
            }

            double yCentre = height / 2;
            for (double i = pixelsPerUnit; yCentre < height - pixelsPerUnit; i += pixelsPerUnit)
            {
                yCentre += pixelsPerUnit;
                TextBlock unitPlus = new TextBlock();
                unitPlus.Text = ((int)Math.Round(-i / pixelsPerUnit)).ToString();
                unitPlus.FontSize = 10;
                unitPlus.Margin = new Thickness(width / 2, yCentre, width / 2, height - yCentre);
                BuildLine(width / 2 - 2, yCentre, width / 2 + 2, yCentre);

                TextBlock unitMinus = new TextBlock();
                unitMinus.Text = ((int)Math.Round(i / pixelsPerUnit)).ToString();
                unitMinus.FontSize = 10;
                unitMinus.Margin = new Thickness(width / 2, height / 2 - i, width / 2, yCentre);
                BuildLine(width / 2 - 2, height / 2 - i, width / 2 + 2, height / 2 - i);
                graphArea.Children.Add(unitPlus);
                graphArea.Children.Add(unitMinus);

            }


        }
    }
}
