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
using System.Drawing;
using MathLibrary;
using static System.Math;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double scale;

        public MainWindow()
        {
            InitializeComponent();
            AbstractCurve[] curves = new AbstractCurve[]
            {
                new Parabola(1),
                new MathLibrary.Ellipse(1, 1),
                new Hyperbola(1, 1)
            };
            ChooseCurve.ItemsSource = curves;
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            AbstractCurve curve = (AbstractCurve)ChooseCurve.SelectedItem;
            if (curve != null)
            {
                DrawingCanvas.Children.Clear();
                PaintCurve(curve);
                PaintCoordinateSystem();
            }
        }

        private void BuildLine(double x1, double y1, double x2, double y2, double strokeThickness)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                StrokeThickness = strokeThickness,
                Stroke = Brushes.Black
            };
            DrawingCanvas.Children.Add(line);
        }

        private void PaintCurve(AbstractCurve curve)
        {
            curve.A = float.TryParse(coeffA.Text, out float A) ? A : 1;
            curve.B = float.TryParse(coeffB.Text, out float B) ? B : 1;
            curve.P = float.TryParse(coeffP.Text, out float P) ? Math.Abs(P) : 1;

            List<PointF> points = curve.GetPoints(DrawingCanvas.ActualHeight, DrawingCanvas.ActualWidth + 150, scale);
            for (int point = 1; point < points.Count; point++)
                if (point != points.Count / 2)
                    BuildLine(points[point - 1].X + DrawingCanvas.ActualWidth / 2, points[point - 1].Y + DrawingCanvas.ActualHeight / 2, points[point].X + DrawingCanvas.ActualWidth / 2, points[point].Y + DrawingCanvas.ActualHeight / 2, 1);
        }

        private void PaintCoordinateSystem()
        {
            double width = DrawingCanvas.ActualWidth;
            double height = DrawingCanvas.ActualHeight;
            double pixelsPerUnit = width / 200 * scale;
            BuildLine(0, height / 2, width, height / 2, 0.2);
            BuildLine(width / 2, 0, width / 2, height, 0.2);

            for (double i = pixelsPerUnit; width / 2 + i < width - pixelsPerUnit; i += pixelsPerUnit)
            {
                if (scale > 2 || (int)(i / pixelsPerUnit) % 2 == 0)
                {
                    TextBlock unitPlus = new TextBlock
                    {
                        Text = ((int)Math.Round(i / pixelsPerUnit)).ToString(),
                        FontSize = 9,
                        Margin = new Thickness(width / 2 + i, height / 2, width - width / 2 - i, height / 2)
                    };
                    DrawingCanvas.Children.Add(unitPlus);

                    TextBlock unitMinus = new TextBlock
                    {
                        Text = ((int)Math.Round(-i / pixelsPerUnit)).ToString(),
                        FontSize = 9,
                        Margin = new Thickness(width / 2 - i, height / 2, width / 2 + i, height / 2)
                    };
                    DrawingCanvas.Children.Add(unitMinus);
                }

                BuildLine(width / 2 + i, height / 2 - 2, width / 2 + i, height / 2 + 2, 0.2);
                BuildLine(width / 2 - i, height / 2 - 2, width / 2 - i, height / 2 + 2, 0.2);
            }
        }

        private void ScaleSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scale = scaleSlider.Value / 10;
            textScaling.Text = $"Scaling: {scale:0.0}";
        }
    }
}
