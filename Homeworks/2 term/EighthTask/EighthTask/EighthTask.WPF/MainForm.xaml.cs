using System;
using System.Collections.Generic;
//using System.Drawing;
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
		//private Graphics Panel { get; set; }
		private double PanelHeight { get; set; }
		private double PanelWidth { get; set; }
		private float SizeNum { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			PanelHeight = Canvas.Height;
			PanelWidth = Canvas.Width;

			SizeNum = 1;
			Label.Content = String.Concat("Масштаб: ", SizeNum.ToString());

			ComboBox.ItemsSource = new Curve[] { new EighthTask.MathCurves.Ellipse(1, 1, 1), new EighthTask.MathCurves.Ellipse(5, 2, 1), new Hyperbola(2, 4, 1), new Parabola(0.5f, 2.4f) };
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
			float num = SizeNum * 9; // size_param
			for (double width = PanelWidth / 20; width < PanelWidth; width += PanelWidth / 20)
			{
				if (width == PanelWidth / 2)
				{
					num -= SizeNum;
					continue;
				}
				DrawLine(width, PanelHeight / 2 - 5, width, PanelHeight / 2 + 5);

				//DrawString(Math.Round(num, 2).ToString(), font, System.Drawing.Brushes.Black, PanelHeight / 2 + 5, width - 9); //textblock
				num -= SizeNum;
			}

			//Разметка по Oy
			num = -9 * SizeNum; // size_param
			for (double height = PanelHeight / 20; height < PanelHeight; height += PanelHeight / 20)
			{
				if (height == PanelHeight / 2)
				{
					num += SizeNum;
					continue;
				}
				DrawLine(PanelWidth / 2 - 5, height, PanelWidth / 2 + 5, height);

				/*
				if (num - Math.Round(num, 6) == 0.000000)
				{
					Panel.DrawString(Math.Round(num, 2).ToString(), font, System.Drawing.Brushes.Black, height - 5, PanelWidth / 2 + 5);
				}
				else if (Math.Abs(num) < 10)
				{
					Panel.DrawString(Math.Round(num, 2).ToString(), font, System.Drawing.Brushes.Black, height - 10, PanelWidth / 2 + 5);
				}
				else
				{
					Panel.DrawString(Math.Round(num, 2).ToString(), font, System.Drawing.Brushes.Black, height - 16, PanelWidth / 2 + 5);
				}
				*/

				num += SizeNum;
			}

			#endregion
		}

		private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			//Label.Content = "22"; // redo
		}

		private void DrawLine(double x1, double y1, double x2, double y2)
		{
			Canvas.Children.Add(new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Black });
		}
	}
}

//доделать WPF, добавить привязку
//тесты