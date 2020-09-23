using DrawingCurves;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Curves.WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        double xShift;
        double yShift;
        double scale;
        int pixelsPerSegment;
        bool pointSetDrawn;
        PointSet[] PointSets { get; set; }
        List<Point[]> SelectedPoints { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            pixelsPerSegment = 40;
            xShift = drawingCanvas.Width / 2f;
            yShift = drawingCanvas.Height / 2;
            scale = 1;
            pointSetDrawn = false;
            DrawCoordinateAxes(1f);
            SelectedPoints = new List<Point[]>();
            PointSets = new PointSet[]
            {
                new Parabola(pixelsPerSegment, xShift * 2, 3),
                new DrawingCurves.Ellipse(pixelsPerSegment, 5, 2),
                new Hyperbola(pixelsPerSegment,xShift * 2, 2, 2)
            };
            curvesCB.ItemsSource = PointSets;
        }


        private void DrawCurve(double scale)
        {
            foreach (var points in SelectedPoints)
            {
                Path path = CreatePath(points, scale);
                drawingCanvas.Children.Add(path);
            }
        }

        private void DrawNumber(double number, double x, double y, double scale)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = number.ToString();
            textBlock.FontSize = 10 * scale;
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            drawingCanvas.Children.Add(textBlock);
        }

        private void DrawNumbers(double scale)
        {
            double step = pixelsPerSegment * scale;
            double x = xShift;
            int number = 0;
            while (x < xShift * 2)
            {
                DrawNumber(number, x + 2, yShift + 2, scale);
                DrawNumber(-number, 2 * xShift - x + 2, yShift + 2, scale);
                number++;
                x += step;
            }
            double y = yShift;
            number = 0;
            while (y < yShift * 2)
            {
                DrawNumber(-number, xShift + 2, y + 2, scale);
                DrawNumber(number, xShift + 2, 2 * yShift - y + 2, scale);
                number++;
                y += step;
            }
        }

        private Path CreatePath(Point[] points, double scale)
        {

            PathGeometry pathGeometry = new PathGeometry();
            PolyLineSegment polyLineSegment = new PolyLineSegment();
            PathFigure pathFigure = new PathFigure();
            polyLineSegment.Points = new PointCollection(points);
            Path path = new Path();
            pathFigure.StartPoint = points[0];
            pathFigure.Segments.Add(polyLineSegment);
            pathGeometry.Figures.Add(pathFigure);
            path.Stroke = Brushes.LightBlue;
            path.StrokeThickness = 2;
            path.Data = pathGeometry;
            path.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
            return path;
        }

        private void DrawCoordinateAxes(double scale)
        {
            DrawAuxiliaryLines(scale);
            DrawMainLines(scale);
            DrawNumbers(scale);
        }

        private void DrawAuxiliaryLines(double scale)
        {
            double x = xShift + pixelsPerSegment;
            while (x < xShift * 2)
            {
                Line posLine = new Line() { X1 = x, Y1 = 2 * yShift, X2 = x, Y2 = 0 };
                Line negLine = new Line() { X1 = 2 * xShift - x, Y1 = 2 * yShift, X2 = 2 * xShift - x, Y2 = 0 };
                posLine.Stroke = Brushes.LightGray;
                negLine.Stroke = Brushes.LightGray;
                negLine.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
                posLine.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
                drawingCanvas.Children.Add(posLine);
                drawingCanvas.Children.Add(negLine);
                x += pixelsPerSegment;
            }
            double y = yShift + pixelsPerSegment;
            while (y < yShift * 2)
            {
                Line posLine = new Line() { X1 = 0, Y1 = y, X2 = xShift * 2, Y2 = y };
                Line negLine = new Line() { X1 = 0, Y1 = 2 * yShift - y, X2 = xShift * 2, Y2 = 2 * yShift - y };
                posLine.Stroke = Brushes.LightGray;
                negLine.Stroke = Brushes.LightGray;
                negLine.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
                posLine.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
                drawingCanvas.Children.Add(posLine);
                drawingCanvas.Children.Add(negLine);
                y += pixelsPerSegment;
            }
        }

        private void DrawMainLines(double scale)
        {
            Line vertL = new Line() { X1 = xShift, Y1 = 0, X2 = xShift, Y2 = 2 * yShift };
            Line horL = new Line() { X1 = 0, Y1 = yShift, X2 = 2 * xShift, Y2 = yShift };
            vertL.Stroke = Brushes.Black;
            horL.Stroke = Brushes.Black;
            horL.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
            vertL.RenderTransform = new ScaleTransform(scale, scale, xShift, yShift);
            drawingCanvas.Children.Add(vertL);
            drawingCanvas.Children.Add(horL);
        }

        private void Scale(double scale)
        {
            drawingCanvas.Children.Clear();
            DrawCoordinateAxes(scale);
            if (pointSetDrawn)
                DrawCurve(scale);
            DrawNumbers(scale);
        }

        private void ClearButtonClicked(object sender, RoutedEventArgs e)
        {
            if (pointSetDrawn)
            {
                drawingCanvas.Children.Clear();
                pointSetDrawn = false;
                DrawCoordinateAxes(scale);
            }
        }

        private void DrawButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedPoints != null)
            {
                pointSetDrawn = true;
                drawingCanvas.Children.Clear();
                DrawCoordinateAxes(scale);
                DrawCurve(scale);
            }
        }

        private void CurvesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (PointSet)(((ComboBox)sender).SelectedItem);
            var pointSet = selected.GetPoints();
            SelectedPoints.Clear();
            foreach (var points in pointSet)
            {
                SelectedPoints.Add(points.Select(p => new Point(p.X * pixelsPerSegment + xShift, p.Y * pixelsPerSegment + yShift)).ToArray());
            }
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scale = scaleSlider.Value / 10;
            scaleLabel.Content = $"Scaling: {scale.ToString("0.0")}";
            Scale(scale);
        }
    }
}
