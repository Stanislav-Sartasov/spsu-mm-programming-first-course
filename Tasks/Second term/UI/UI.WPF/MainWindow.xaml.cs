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
using UI.MathCurve;

namespace UI.WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Curve[] curves = new Curve[]
            {
                new MyParabola(),
                new MyHyperbola(),
                new MyEllipse()
            };
            ChooseCurve.ItemsSource = curves;
        }

        private void BuildFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            Curve shape = (Curve)ChooseCurve.SelectedItem;
            if (shape != null)
            {
                DrawingArea.Children.Clear();
                BuildFunction(shape);
                BuildCoordinateSystem();
            }
        }

        private void BuildFunction(Curve shape)
        { 
            double offsetX = Application.Current.MainWindow.ActualWidth / 2 - 50;
            double offsetY = Application.Current.MainWindow.ActualHeight / 2;
            double scale;
            bool correctScale = double.TryParse(WindowScale.Text, out scale);
            if (scale < 50 || scale > 200 || !correctScale) 
                scale = 100;

            float tempA;
            float tempB;
            float tempP;
            bool correctA = float.TryParse(a.Text, out tempA);
            bool correctB = float.TryParse(b.Text, out tempB);
            bool correctC = float.TryParse(p.Text, out tempP);
            bool flagCalculate = false;
            if (shape.Name.Equals("Hyperbola") || shape.Name.Equals("Ellipse"))
                if (correctA && correctB && tempA > 0 && tempB > 0)
                {
                    shape.A = tempA;
                    shape.B = tempB;
                    flagCalculate = true;
                }
            if (shape.Name.Equals("Parabola"))
                if (correctC && tempP > 0)
                {
                    shape.P = tempP;
                    flagCalculate = true;
                }
            if (flagCalculate)
            {
                //double step = Math.Min(offsetX, offsetY) / 300;
                double step = 100 / scale;
                var pointGroupList = shape.FindValues(step, offsetX, offsetY, scale);
                foreach (List<double[]> points in pointGroupList)
                {
                    Polyline result = new Polyline();
                    result.Points = ListOfDoubleToPointCollection(points);
                    result.StrokeThickness = 1;
                    result.Stroke = Brushes.Black;
                    //result.ClipToBounds = true;
                    DrawingArea.Children.Add(result);
                }
            }
        }

        private PointCollection ListOfDoubleToPointCollection(List<double[]> a)
        {
            PointCollection result = new PointCollection();
            foreach(double[] point in a)
                result.Add(new Point(point[0], point[1]));
            return result;
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

            DrawingArea.Children.Add(line);
        }
        private void BuildCoordinateSystem()
        {
            double offsetX = Application.Current.MainWindow.ActualWidth / 2 - 50;
            double offsetY = Application.Current.MainWindow.ActualHeight / 2;

            double leftBorder = offsetX / 5;
            double rightBorder = offsetX * 9 / 5;
            double upperBorder = offsetY / 5;
            double bottomBorder = offsetY * 9 / 5;

            BuildLine(leftBorder, offsetY, rightBorder, offsetY);
            BuildLine(offsetX, upperBorder, offsetX, bottomBorder);

            BuildLine(rightBorder, offsetY, rightBorder - 5, offsetY - 5);
            BuildLine(rightBorder, offsetY, rightBorder - 5, offsetY + 5);

            BuildLine(offsetX, upperBorder, offsetX - 5, upperBorder + 5);
            BuildLine(offsetX, upperBorder, offsetX + 5, upperBorder + 5);

            double scale;
            bool correctScale = double.TryParse(WindowScale.Text, out scale);
            if (scale < 50 || scale > 200 || !correctScale)
                scale = 100;
            int step = (int)Math.Round(scale / 5);
            int sign = 0;
            for (int x = (int)Math.Round(leftBorder + 5); x < rightBorder  - 5; x += step)
            {
                BuildLine(x, offsetY - 5, x, offsetY + 5);
                TextBlock outputValue = new TextBlock();
                outputValue.Text = ((int)Math.Round((x - offsetX) / (scale / 100))).ToString();
                outputValue.FontSize = 8;
                outputValue.Margin = new Thickness(x - 5, offsetY + 5 * Math.Pow(-3, sign % 2), x + 5, offsetY + 10 * Math.Pow(-3, sign % 2));
                DrawingArea.Children.Add(outputValue);
                sign++;
            }
            sign = 0;
            for (int y = (int)Math.Round(bottomBorder - 5); y > upperBorder + 5; y -= step)
            {
                BuildLine(offsetX - 5, y, offsetX + 5, y);
                TextBlock outputValue = new TextBlock();
                outputValue.Text = ((int)Math.Round((offsetY - y) / (scale / 100))).ToString();
                outputValue.FontSize = 8;
                outputValue.Margin = new Thickness(offsetX + 5 * Math.Pow(-4, sign % 2), y - 5, offsetX + 10 * Math.Pow(-3, sign % 2), y + 5);
                DrawingArea.Children.Add(outputValue);
                sign++;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DrawingArea.Children.Clear();
        }
    }

}
