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
using EighthTask.MathCurves;

namespace EighthTask.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private float PanelHeight { get; set; }
		private float PanelWidth { get; set; }
		private float SizeNum { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			PanelHeight = (float)Canvas.Height;
			PanelWidth = (float)Canvas.Width;

			SizeNum = 1.0f;
			TextBlock.Text = String.Concat("Масштаб: ", SizeNum.ToString());

			ComboBox.ItemsSource = new Curve[] { new EighthTask.MathCurves.Ellipse(1, 1, 1), new EighthTask.MathCurves.Ellipse(5, 1.8f, 1), new Hyperbola(2, 4, 1), new Parabola(0.5f, 2.4f) };
			ComboBox.SelectedItem = ComboBox.Items[0];
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			Canvas.Children.Clear();

			#region Draw coordinates

			//var font = new System.Windows.Media.Font(this.FontFamily, 8);

			//Ox
			DrawLine(PanelWidth / 2, 0, PanelWidth / 2, PanelHeight);
			DrawLine(PanelWidth / 2 - 5, 15, PanelWidth / 2, 0);
			DrawLine(PanelWidth / 2 + 5, 15, PanelWidth / 2, 0);

			//Oy
			DrawLine(0, PanelHeight / 2, PanelWidth, PanelHeight / 2);
			DrawLine(PanelWidth - 15, PanelHeight / 2 - 5, PanelWidth, PanelHeight / 2);
			DrawLine(PanelWidth - 15, PanelHeight / 2 + 5, PanelWidth, PanelHeight / 2);

			//Разметка по Ox
			float num = -9 * SizeNum; // size_param
			for (double width = PanelWidth / 20; width < PanelWidth; width += PanelWidth / 20)
			{
				if (width == PanelWidth / 2)
				{
					num += SizeNum;
					continue;
				}
				DrawLine(width, PanelHeight / 2 - 5, width, PanelHeight / 2 + 5);

				if (num - Math.Round(num, 6) == 0.000000)
				{
					DrawNum(Math.Round(num, 2).ToString(), width - 2, PanelHeight / 2 + 5);
				}
				else if (Math.Abs(num) < 10)
				{
					DrawNum(Math.Round(num, 2).ToString(), width - 8, PanelHeight / 2 + 5);
				}
				else
				{
					DrawNum(Math.Round(num, 2).ToString(), width - 12, PanelHeight / 2 + 5);
				}

				num += SizeNum;
			}

			//Разметка по Oy
			num = 9 * SizeNum; // size_param
			for (double height = PanelHeight / 20; height < PanelHeight; height += PanelHeight / 20)
			{
				if (height == PanelHeight / 2)
				{
					num -= SizeNum;
					continue;
				}
				DrawLine(PanelWidth / 2 - 5, height, PanelWidth / 2 + 5, height);

				DrawNum(Math.Round(num, 2).ToString(), PanelWidth / 2 + 5, height - 7);

				num -= SizeNum;
			}

			#endregion


			#region Draw function
			
			var curve = (Curve)ComboBox.SelectedItem;
			curve.SetPoints(SizeNum);

			for (int i = 0; i < curve.Points.Count; i++) // ограничить canvas
			{
				var local = curve.Points[i];
				local.X = PanelWidth / 2 + (local.X * (PanelWidth / 20) / SizeNum);
				local.Y = PanelHeight / 2 - (local.Y * (PanelHeight / 20) / SizeNum);
				curve.Points[i] = local;
			}

			for (int i = 1; i < curve.Points.Count; i++)
			{
				if (i == curve.Points.Count / 2)
				{
					continue;
				}
				if (Math.Abs(curve.Points[i - 1].X) < Math.Abs(Canvas.ActualWidth) && Math.Abs(curve.Points[i - 1].Y) < Math.Abs(Canvas.ActualHeight) && Math.Abs(curve.Points[i].X) < Math.Abs(Canvas.ActualWidth) && Math.Abs(curve.Points[i].Y) < Math.Abs(Canvas.ActualHeight))
				{
					DrawLine(curve.Points[i - 1].X, curve.Points[i - 1].Y, curve.Points[i].X, curve.Points[i].Y);
				}
			}
			
			#endregion
		}

		private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			SizeNum = (float)Math.Round(Slider.Value * 0.2d, 2);
			TextBlock.Text = String.Concat("Масштаб: ", SizeNum.ToString());
		}

		private void DrawLine(double x1, double y1, double x2, double y2)
		{
			Canvas.Children.Add(new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Black });
		}

		private void DrawNum(string num, double x, double y)
		{
			var tb = new TextBlock { Text = num, FontSize = 10 };
			
			Canvas.SetLeft(tb, x);
			Canvas.SetTop(tb, y);
			
			Canvas.Children.Add(tb);
		}
	}
}