using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyMath;

namespace WinFormsCurves
{
	public partial class SimpleCurves : Form
	{
		Graphics graphic;
		List<string> curves = new List<string>()
		{
			"Parabola",
			"Hyperbola",
			"Ellipse"
		};
		List<ICurveBuilder> curveBuilders = new List<ICurveBuilder>()
		{
			new MyMath.Parabola(),
			new MyMath.Hyperbola(),
			new MyMath.Ellipse()
		};

		const double StartOneSize = 15;
		double actualOneSize = StartOneSize;
		public SimpleCurves()
		{
			InitializeComponent();
			CurveComboBox.Items.AddRange(curves.ToArray());
		}
		private void CurveComboBoxLeave(object sender, EventArgs e)
		{
			BTextBox.ReadOnly = curves.IndexOf(CurveComboBox.Text) == 0;
			DrawCurve(sender, e);
		}
		private void CurveComboBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				ATextBox.Focus();
		}
		private void ATextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (!BTextBox.ReadOnly)
					BTextBox.Focus();
				else
					DrawCurve(sender, e);
			}
		}
		private void BTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				CurveArea.Focus();
			}
		}
		private void DrawCurve(object sender, EventArgs e)
		{
			graphic = CurveArea.CreateGraphics();
			graphic.Clear(Color.White);

			int index = curves.IndexOf(CurveComboBox.Text);
			if (index < 0)
			{
				actualOneSize = StartOneSize;
				DrawCoordinate(0, 0, -1);
				return;
			}

			switch (index)
			{
				case 0:
					FormulaBox.Text = "\nY^2 = 2 * a * X";
					break;
				case 1:
					FormulaBox.Text = "\nX^2   Y^2\n------ - ------ = 1\na^2   b^2";
					break;
				case 2:
					FormulaBox.Text = "\nX^2   Y^2\n------ + ------ = 1\na^2   b^2";
					break;
			}

			List<List<double[]>> points;
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

			if (index == 2)
				newOneSize = Math.Min(CurveArea.Size.Width, CurveArea.Size.Height) / Math.Min(Math.Abs(a), Math.Abs(b)) / StartOneSize;
			else
				newOneSize = Math.Min(CurveArea.Size.Width, CurveArea.Size.Height) / Math.Abs(a) / StartOneSize;

			points = curveBuilders[index].Build(0.1 / (CurveSize.Value + 1) / newOneSize, CurveArea.Size.Width / 2 / (CurveSize.Value + 1) / newOneSize, a, b);

			if (points == null)
			{
				actualOneSize = StartOneSize;
				DrawCoordinate(0, 0, -1);
				return;
			}

			actualOneSize = newOneSize;

			try
			{
				DrawCoordinate(a, b, index);
				foreach (List<double[]> curve in points)
					graphic.DrawCurve(new Pen(Color.Black), PreparePointF(curve));
			}
			catch { }
		}
		PointF[] PreparePointF(List<double[]> points)
		{
			List<PointF> result = new List<PointF>();
			foreach (var point in points)
			{
				float x = (float)(point[0] * actualOneSize * (CurveSize.Value + 1) + CurveArea.Size.Width / 2);
				float y = (float)(point[1] * actualOneSize * (CurveSize.Value + 1) + CurveArea.Size.Height / 2);
				result.Add(new PointF(x, y));
			}
			return result.ToArray();
		}
		void DrawLine(double x1, double y1, double x2, double y2)
		{
			graphic.DrawLine(new Pen(Color.Black), (float)x1, (float)y1, (float)x2, (float)y2);
		}
		void AddMark(int width, int height, double value, int direct, string text)
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
			graphic.DrawString(text, new Font(Font.FontFamily, 7), Brushes.Black, (float)(x1 + 3), (float)(y1 + 3));
		}
		void DrawCoordinate(double a, double b, int type)
		{
			int width = CurveArea.Size.Width;
			int height = CurveArea.Size.Height;

			DrawLine(0, height / 2, width, height / 2);
			DrawLine(width / 2, 0, width / 2, height);

			int param;
			for (param = 1; param * actualOneSize * (CurveSize.Value + 1) < StartOneSize; param *= 10) ;

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
	}
}
