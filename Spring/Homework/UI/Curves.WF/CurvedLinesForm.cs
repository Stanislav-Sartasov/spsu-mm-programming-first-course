using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DrawingCurves;

namespace WinForms
{
	public partial class CurvedLinesForm : Form
	{
		Bitmap picture;
		PictureBox pictureBox;
		Pen mainPen;
		Pen auxiliaryPen;
		Pen graphPen;
		Graphics graph;
		int pixelsPerSegment;
		List<PointF[]> pointsToDraw;
		PointSet[] pointSets;
		float scale;
		bool pointSetDrawn;

		public CurvedLinesForm()
		{
			InitializeComponent();

			Width = 800;
			Height = 600;
			pixelsPerSegment = Width / 20;
			pointSetDrawn = false;
			scale = 1;

			pointSets = new PointSet[]
			{
				new Parabola(pixelsPerSegment, Width, 1),
				new Ellipse(pixelsPerSegment, 4, 2),
				new Hyperbola(pixelsPerSegment, Width, 1, 1),
			};

		
			picture = new Bitmap(Width, Height);
			mainPen = new Pen(Color.Black, 2 / 30f);
			graphPen = new Pen(Color.FromArgb(80, 170, 200), 40 / pixelsPerSegment);
			auxiliaryPen = new Pen(Color.FromArgb(235, 235, 235), 2);
			DoubleBuffered = true;

			var scaleTB = new TrackBar()
			{
				Location = new Point(5, ClientSize.Height - 100),
				SmallChange = 1,
				LargeChange = 10,
				TickFrequency = 5,
				Maximum = 500,
				Minimum = 100
			};
			Controls.Add(scaleTB);


			var scalingLabel = new Label()
			{
				Location = new Point(5, ClientSize.Height - 120),
				Size = new Size(scaleTB.Width, 20),
				Font = new Font("Arial", 12, FontStyle.Bold),
				Text = "Scaling: 1,0"
			};
			Controls.Add(scalingLabel);


			scaleTB.Scroll += (sender, args) =>
			{
				scalingLabel.Text = "Scaling: " + (scaleTB.Value / 100f).ToString();
				scale = scaleTB.Value / 100f;
				ScalePicture(scale);
			};

			var clearButton = new Button()
			{
				Location = new Point(5, ClientSize.Height - 150),
				Size = new Size(scaleTB.Width, 20),
				Text = "Clear"
			};

			clearButton.Click += (sender, args) =>
			{
				if (pointSetDrawn)
				{
					CreateCoordinateAxes(scale);
					pointSetDrawn = false;
				}
			};
			Controls.Add(clearButton);

			var drawButton = new Button()
			{
				Location = new Point(5, ClientSize.Height - 180),
				Size = new Size(scaleTB.Width, 20),
				Text = "Draw"
			};
			Controls.Add(drawButton);
			drawButton.Click += (sender, args) =>
			{
				if (pointsToDraw != null)
				{
					CreateCoordinateAxes(scale);
					DrawCurve(scale);
					pointSetDrawn = true;
				}
			};

			var curvesComboBox = new ComboBox()
			{
				Location = new Point(5, ClientSize.Height - 210),
				Size = new Size(drawButton.Width, 20)
			};
			curvesComboBox.DataSource = pointSets;
			curvesComboBox.SelectedIndexChanged += (sender, args) =>
			{
				ChangePointsToDraw((PointSet)curvesComboBox.SelectedValue);
			};
			Controls.Add(curvesComboBox);



			pictureBox = new PictureBox()
			{
				Dock = DockStyle.Fill,
				BackColor = Color.White,
			};
			Controls.Add(pictureBox);

			CreateCoordinateAxes(1);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
		}

		private void CreateCoordinateAxes(float scale)
		{
			graph = Graphics.FromImage(picture);
			graph.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
			graph.SmoothingMode = SmoothingMode.HighQuality;
			graph.Clear(Color.White);
			graph.ScaleTransform(scale, scale);

			for (int x = pixelsPerSegment; x < ClientSize.Width / 2; x += pixelsPerSegment)
			{
				graph.DrawLine(auxiliaryPen, new Point(x, ClientSize.Height), new Point(x, -ClientSize.Height));
				graph.DrawLine(auxiliaryPen, new Point(-x, ClientSize.Height), new Point(-x, -ClientSize.Height));
				graph.DrawString($"{x / pixelsPerSegment}", new Font("Arial", 5, FontStyle.Bold), Brushes.Black, new PointF(x, 1));
				graph.DrawString($"{-x / pixelsPerSegment}", new Font("Arial", 5, FontStyle.Bold), Brushes.Black, new PointF(-x, 1));
			}
			for (int y = pixelsPerSegment; y < ClientSize.Height / 2; y += pixelsPerSegment)
			{
				graph.DrawLine(auxiliaryPen, new Point(ClientSize.Width, y), new Point(-ClientSize.Width, y));
				graph.DrawLine(auxiliaryPen, new Point(ClientSize.Width, -y), new Point(-ClientSize.Width, -y));
				graph.DrawString($"{y / pixelsPerSegment}", new Font("Arial", 5, FontStyle.Bold), Brushes.Black, new PointF(1, -y));
				graph.DrawString($"{-y / pixelsPerSegment}", new Font("Arial", 5, FontStyle.Bold), Brushes.Black, new PointF(1, y));
			}
			graph.DrawLine(mainPen, new Point(0, ClientSize.Height), new Point(0, -ClientSize.Height));
			graph.DrawLine(mainPen, new Point(ClientSize.Width, 0), new Point(-ClientSize.Width, 0));

			pictureBox.Image = picture;

		}


		private void ScalePicture(float scale)
		{
			CreateCoordinateAxes(scale);
			if (pointSetDrawn)
				DrawCurve(scale);
		}


		private void ChangePointsToDraw(PointSet pointSet) 
		{
			pointsToDraw = pointSet.Points;
		}


		private void DrawCurve(float scale)
		{
			graph = Graphics.FromImage(picture);
			graph.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
			graph.SmoothingMode = SmoothingMode.HighQuality;
			graphPen.Width = 1 / (scale * 20);
			graph.ScaleTransform(scale * pixelsPerSegment, scale * pixelsPerSegment);
			foreach (var points in pointsToDraw)
			{
				graph.DrawCurve(graphPen, points);
			}
			pictureBox.Image = picture;
		}
	}
}
