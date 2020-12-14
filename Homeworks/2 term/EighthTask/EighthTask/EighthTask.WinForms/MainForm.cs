using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EighthTask.MathCurves;

namespace EighthTask.WinForms
{
	public partial class MainForm : Form
	{
		private Graphics Panel { get; set; }
		private int PanelHeight { get; set; }
		private int PanelWidth { get; set; }
		private float SizeNum { get; set; }

		public MainForm()
		{
			InitializeComponent();

			Panel = PictureBox.CreateGraphics();
			PanelHeight = PictureBox.Height;
			PanelWidth = PictureBox.Width;

			SizeNum = 1;
			Label.Text = String.Concat("Масштаб: ", SizeNum.ToString());

			ComboBox.Items.AddRange(new Curve[] { new Ellipse(1, 1, 1), new Ellipse(5, 2, 1), new Hyperbola(2, 4, 1), new Parabola(0.5f, 2.4f) });
			ComboBox.SelectedItem = ComboBox.Items[0];
		}

		private void ButtonClick(object sender, EventArgs e)
		{
			Panel.Clear(BackColor);
			#region Draw coordinates

			var font = new Font(Font.FontFamily, 8);

			//Ox
			Panel.DrawLine(Pens.Black, PanelWidth / 2, 0, PanelWidth / 2, PanelHeight);
			Panel.DrawLine(Pens.Black, PanelWidth / 2 - 5, 15, PanelWidth / 2, 0);
			Panel.DrawLine(Pens.Black, PanelWidth / 2 + 5, 15, PanelWidth / 2, 0);

			//Oy
			Panel.DrawLine(Pens.Black, 0, PanelHeight / 2, PanelWidth, PanelHeight / 2);
			Panel.DrawLine(Pens.Black, PanelWidth - 15, PanelHeight / 2 - 5, PanelWidth, PanelHeight / 2);
			Panel.DrawLine(Pens.Black, PanelWidth - 15, PanelHeight / 2 + 5, PanelWidth, PanelHeight / 2);

			//Разметка по Ox
			float num = SizeNum * 9; // size_param
			for (int width = PanelWidth / 20; width < PanelWidth; width += PanelWidth / 20)
			{
				if (width == PanelWidth / 2)
				{
					num -= SizeNum;
					continue;
				}
				Panel.DrawLine(Pens.Black, width, PanelHeight / 2 - 5, width, PanelHeight / 2 + 5);

				Panel.DrawString(Math.Round(num, 2).ToString(), font, Brushes.Black, PanelHeight / 2 + 5, width - 9);
				num -= SizeNum;
			}

			//Разметка по Oy
			num = -9 * SizeNum; // size_param
			for (int height = PanelHeight / 20; height < PanelHeight; height += PanelHeight / 20)
			{
				if (height == PanelHeight / 2)
				{
					num += SizeNum;
					continue;
				}
				Panel.DrawLine(Pens.Black, PanelWidth / 2 - 5, height, PanelWidth / 2 + 5, height);

				if (num - Math.Round(num, 6) == 0.000000)
				{
					Panel.DrawString(Math.Round(num, 2).ToString(), font, Brushes.Black, height - 5, PanelWidth / 2 + 5);
				}
				else if (Math.Abs(num) < 10)
				{
					Panel.DrawString(Math.Round(num, 2).ToString(), font, Brushes.Black, height - 10, PanelWidth / 2 + 5);
				}
				else
				{
					Panel.DrawString(Math.Round(num, 2).ToString(), font, Brushes.Black, height - 16, PanelWidth / 2 + 5);
				}

				num += SizeNum;
			}

			#endregion

			#region Draw function

			var curve = (Curve)ComboBox.SelectedItem;
			curve.SetPoints(SizeNum);

			for (int i = 0; i < curve.Points.Count; i++)
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
				Panel.DrawLine(Pens.Black, curve.Points[i - 1], curve.Points[i]);
			}

			#endregion
		}

		private void TrackBarScroll(object sender, EventArgs e)
		{
			SizeNum = (float)Math.Round(TrackBar.Value * 0.2d, 2);
			Label.Text = String.Concat("Масштаб: ", SizeNum.ToString());
		}
	}
}
