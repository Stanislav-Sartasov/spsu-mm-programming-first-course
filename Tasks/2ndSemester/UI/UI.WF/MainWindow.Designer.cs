using System.Drawing;

namespace UI.WF
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.textScaling = new System.Windows.Forms.TextBox();
            this.coeffP = new System.Windows.Forms.TextBox();
            this.coeffB = new System.Windows.Forms.TextBox();
            this.textBoxP = new System.Windows.Forms.TextBox();
            this.textBoxB = new System.Windows.Forms.TextBox();
            this.TextBoxA = new System.Windows.Forms.TextBox();
            this.coeffA = new System.Windows.Forms.TextBox();
            this.Build = new System.Windows.Forms.Button();
            this.scaleSlider = new System.Windows.Forms.TrackBar();
            this.ChooseCurve = new System.Windows.Forms.ComboBox();
            this.DrawingCanvas = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textScaling);
            this.panel1.Controls.Add(this.coeffP);
            this.panel1.Controls.Add(this.coeffB);
            this.panel1.Controls.Add(this.textBoxP);
            this.panel1.Controls.Add(this.textBoxB);
            this.panel1.Controls.Add(this.TextBoxA);
            this.panel1.Controls.Add(this.coeffA);
            this.panel1.Controls.Add(this.Build);
            this.panel1.Controls.Add(this.scaleSlider);
            this.panel1.Controls.Add(this.ChooseCurve);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 426);
            this.panel1.TabIndex = 0;
            // 
            // textScaling
            // 
            this.textScaling.Location = new System.Drawing.Point(2, 323);
            this.textScaling.Name = "textScaling";
            this.textScaling.ReadOnly = true;
            this.textScaling.Size = new System.Drawing.Size(188, 23);
            this.textScaling.TabIndex = 8;
            this.textScaling.Text = "Scaling: 1,0";
            // 
            // coeffP
            // 
            this.coeffP.Location = new System.Drawing.Point(37, 87);
            this.coeffP.Name = "coeffP";
            this.coeffP.Size = new System.Drawing.Size(29, 23);
            this.coeffP.TabIndex = 7;
            // 
            // coeffB
            // 
            this.coeffB.Location = new System.Drawing.Point(37, 58);
            this.coeffB.Name = "coeffB";
            this.coeffB.Size = new System.Drawing.Size(29, 23);
            this.coeffB.TabIndex = 6;
            // 
            // textBoxP
            // 
            this.textBoxP.Location = new System.Drawing.Point(2, 87);
            this.textBoxP.Name = "textBoxP";
            this.textBoxP.ReadOnly = true;
            this.textBoxP.Size = new System.Drawing.Size(29, 23);
            this.textBoxP.TabIndex = 5;
            this.textBoxP.Text = "p = ";
            // 
            // textBoxB
            // 
            this.textBoxB.Location = new System.Drawing.Point(2, 58);
            this.textBoxB.Name = "textBoxB";
            this.textBoxB.ReadOnly = true;
            this.textBoxB.Size = new System.Drawing.Size(29, 23);
            this.textBoxB.TabIndex = 4;
            this.textBoxB.Text = "b = ";
            // 
            // TextBoxA
            // 
            this.TextBoxA.Location = new System.Drawing.Point(2, 29);
            this.TextBoxA.Name = "TextBoxA";
            this.TextBoxA.ReadOnly = true;
            this.TextBoxA.Size = new System.Drawing.Size(29, 23);
            this.TextBoxA.TabIndex = 0;
            this.TextBoxA.Text = "a = ";
            // 
            // coeffA
            // 
            this.coeffA.Location = new System.Drawing.Point(37, 29);
            this.coeffA.Name = "coeffA";
            this.coeffA.Size = new System.Drawing.Size(29, 23);
            this.coeffA.TabIndex = 3;
            // 
            // Build
            // 
            this.Build.Location = new System.Drawing.Point(0, 403);
            this.Build.Name = "Build";
            this.Build.Size = new System.Drawing.Size(188, 23);
            this.Build.TabIndex = 2;
            this.Build.Text = "Build";
            this.Build.UseVisualStyleBackColor = true;
            this.Build.Click += new System.EventHandler(this.Build_Click);
            // 
            // scaleSlider
            // 
            this.scaleSlider.Location = new System.Drawing.Point(2, 352);
            this.scaleSlider.Maximum = 5;
            this.scaleSlider.Minimum = 1;
            this.scaleSlider.Name = "scaleSlider";
            this.scaleSlider.Size = new System.Drawing.Size(188, 45);
            this.scaleSlider.TabIndex = 1;
            this.scaleSlider.Value = 1;
            this.scaleSlider.Scroll += new System.EventHandler(this.ScaleSlider_Scroll);
            // 
            // ChooseCurve
            // 
            this.ChooseCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChooseCurve.FormattingEnabled = true;
            this.ChooseCurve.Location = new System.Drawing.Point(0, 0);
            this.ChooseCurve.Name = "ChooseCurve";
            this.ChooseCurve.Size = new System.Drawing.Size(190, 23);
            this.ChooseCurve.TabIndex = 0;
            // 
            // DrawingCanvas
            // 
            this.DrawingCanvas.Location = new System.Drawing.Point(208, 12);
            this.DrawingCanvas.Name = "DrawingCanvas";
            this.DrawingCanvas.Size = new System.Drawing.Size(580, 426);
            this.DrawingCanvas.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 441);
            this.Controls.Add(this.DrawingCanvas);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(820, 480);
            this.MinimumSize = new System.Drawing.Size(820, 480);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel DrawingCanvas;
        private System.Windows.Forms.ComboBox ChooseCurve;
        private System.Windows.Forms.Button Build;
        private System.Windows.Forms.TrackBar scaleSlider;
        private System.Windows.Forms.TextBox textScaling;
        private System.Windows.Forms.TextBox coeffP;
        private System.Windows.Forms.TextBox coeffB;
        private System.Windows.Forms.TextBox textBoxB;
        private System.Windows.Forms.TextBox TextBoxA;
        private System.Windows.Forms.TextBox textBoxP;
        private System.Windows.Forms.TextBox coeffA;
    }
}

