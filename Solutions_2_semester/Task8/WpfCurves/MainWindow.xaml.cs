using MyMath;
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

namespace WpfCurves
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const double StartOneSize = 15;
		double actualOneSize = StartOneSize;
		public MainWindow()
		{
			InitializeComponent();
			ICurveBuilder[] curveBuilders = new ICurveBuilder[]
			{
				new MyMath.Parabola(),
				new MyMath.Hyperbola(),
				new MyMath.Ellipse()
			};
			CurveComboBox.ItemsSource = curveBuilders;
		}

		private void DrawCurve(object sender, object e)
		{
			CurveArea.Children.Clear();

			switch (CurveComboBox.SelectedIndex)
			{
				case 0:
					FormulaBox.Content = "\nY^2 = 2 * a * X";
					BTextBox.IsReadOnly = true;
					break;
				case 1:
					FormulaBox.Content = "\nX^2    Y^2\n----- - ----- = 1\na^2    b^2";
					BTextBox.IsReadOnly = false;
					break;
				case 2:
					FormulaBox.Content = "\nX^2    Y^2\n---- + ----- = 1\na^2    b^2";
					BTextBox.IsReadOnly = false;
					break;
				default:
					return;
			}

			double a = -1;
			double b = -1;
			try
			{
				a = double.Parse(ATextBox.Text);
			}
			catch { }
			try
			{
				b = double.Parse(BTextBox.Text);
			}
			catch { }

			double newOneSize;

			if (CurveComboBox.SelectedIndex == 2)
				newOneSize = Math.Min(CurveArea.ActualWidth, CurveArea.ActualHeight) / Math.Max(Math.Abs(a), Math.Abs(b)) / StartOneSize;
			else
				newOneSize = Math.Min(CurveArea.ActualWidth, CurveArea.ActualHeight) / Math.Abs(a) / StartOneSize;

			var points = ((ICurveBuilder)CurveComboBox.SelectedItem).Build(0.1 / (CurveSize.Value + 1) / newOneSize, CurveArea.ActualWidth / 2 / (CurveSize.Value + 1) / newOneSize, a, b);
			if (points == null)
				return;

			actualOneSize = newOneSize;

			DrawCoordinate(a, b, CurveComboBox.SelectedIndex);

			foreach (var curve in points)
				CurveArea.Children.Add(PreparePoints(curve));
		}

		Polyline PreparePoints(List<double[]> points)
		{
			Polyline result = new Polyline();
			result.StrokeThickness = 1;
			result.Stroke = Brushes.Black;
			foreach (var point in points)
			{
				double x = point[0] * actualOneSize * (CurveSize.Value + 1) + CurveArea.ActualWidth / 2;
				double y = point[1] * actualOneSize * (CurveSize.Value + 1) + CurveArea.ActualHeight / 2;
				result.Points.Add(new Point(x, y));
			}
			return result;
		}
		void DrawLine(double x1, double y1, double x2, double y2)
		{
			CurveArea.Children.Add(new Line()
			{
				X1 = x1,
				X2 = x2,
				Y1 = y1,
				Y2 = y2,
				StrokeThickness = 1,
				Stroke = Brushes.Black
			});
		}
		void AddMark(double width, double height, double value, int direct, string text)
		{
			double x1;
			double x2;
			double y1;
			double y2;
			if (direct == 0)
			{
				x1 = width / 2 + actualOneSize * (CurveSize.Value + 1) * value;
				x2 = x1;
				y1 = height / 2 - 3;
				y2 = y1 + 6;
			}
			else
			{
				x1 = width / 2 - 3;
				x2 = x1 + 6;
				y1 = height / 2 - actualOneSize * (CurveSize.Value + 1) * value;
				y2 = y1;
			}
			DrawLine(x1, y1, x2, y2);
			CurveArea.Children.Add(new TextBlock() { Text = text, FontSize = 7, Margin = new Thickness(x1 + 3, y1 + 3, x1 - 3, y1 - 3) });
		}
		void DrawCoordinate(double a, double b, int type)
		{
			double width = CurveArea.ActualWidth;
			double height = CurveArea.ActualHeight;

			DrawLine(0, height / 2, width, height / 2);
			DrawLine(width / 2, 0, width / 2, height);

			double param;
			for (param = 0.001; param * actualOneSize * (CurveSize.Value + 1) < StartOneSize; param *= 10);

			AddMark(width, height, 0, 0, "0");
			AddMark(width, height, param, 0, param.ToString());
			AddMark(width, height, param, 1, param.ToString());

			switch (type)
			{
				case 0:
					AddMark(width, height, a / 2, 0, (a / 2).ToString());
					break;
				case 1:
					AddMark(width, height, a, 0, Math.Round(a, 3).ToString());
					AddMark(width, height, -a, 0, Math.Round(-a, 3).ToString());
					break;
				case 2:
					if (Math.Abs(a) * actualOneSize * (CurveSize.Value + 1) > StartOneSize)
					{
						AddMark(width, height, a, 0, Math.Round(a, 3).ToString());
						AddMark(width, height, -a, 0, Math.Round(-a, 3).ToString());
					}
					if (Math.Abs(b) * actualOneSize * (CurveSize.Value + 1) > StartOneSize)
					{
						AddMark(width, height, b, 1, Math.Round(b, 3).ToString());
						AddMark(width, height, -b, 1, Math.Round(-b, 3).ToString());
					}
					break;
			}

		}

		private void CurveComboBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				ATextBox.Focus();
		}
		private void ATextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				if (!BTextBox.IsReadOnly)
					BTextBox.Focus();
				else
					CurveSize.Focus();
		}
		private void BTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				CurveSize.Focus();
		}
	}
}
