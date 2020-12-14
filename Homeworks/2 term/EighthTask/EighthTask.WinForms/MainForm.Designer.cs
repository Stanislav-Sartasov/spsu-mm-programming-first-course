
namespace EighthTask.WinForms
{
	partial class MainForm
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
			this.PictureBox = new System.Windows.Forms.PictureBox();
			this.Button = new System.Windows.Forms.Button();
			this.TrackBar = new System.Windows.Forms.TrackBar();
			this.ComboBox = new System.Windows.Forms.ComboBox();
			this.Label = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// PictureBox
			// 
			this.PictureBox.Location = new System.Drawing.Point(12, 12);
			this.PictureBox.Name = "PictureBox";
			this.PictureBox.Size = new System.Drawing.Size(500, 500);
			this.PictureBox.TabIndex = 0;
			this.PictureBox.TabStop = false;
			// 
			// Button
			// 
			this.Button.Location = new System.Drawing.Point(527, 462);
			this.Button.Name = "Button";
			this.Button.Size = new System.Drawing.Size(313, 46);
			this.Button.TabIndex = 1;
			this.Button.Text = "Draw";
			this.Button.UseVisualStyleBackColor = true;
			this.Button.Click += new System.EventHandler(this.ButtonClick);
			// 
			// TrackBar
			// 
			this.TrackBar.Location = new System.Drawing.Point(527, 91);
			this.TrackBar.Maximum = 9;
			this.TrackBar.Minimum = 1;
			this.TrackBar.Name = "TrackBar";
			this.TrackBar.Size = new System.Drawing.Size(313, 56);
			this.TrackBar.TabIndex = 3;
			this.TrackBar.Value = 5;
			this.TrackBar.Scroll += new System.EventHandler(this.TrackBarScroll);
			// 
			// ComboBox
			// 
			this.ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBox.FormattingEnabled = true;
			this.ComboBox.Location = new System.Drawing.Point(527, 12);
			this.ComboBox.Name = "ComboBox";
			this.ComboBox.Size = new System.Drawing.Size(313, 28);
			this.ComboBox.TabIndex = 4;
			// 
			// Label
			// 
			this.Label.AutoSize = true;
			this.Label.Location = new System.Drawing.Point(638, 150);
			this.Label.Name = "Label";
			this.Label.Size = new System.Drawing.Size(98, 20);
			this.Label.TabIndex = 2;
			this.Label.Text = "Масштаб: 1.0";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(852, 520);
			this.Controls.Add(this.ComboBox);
			this.Controls.Add(this.TrackBar);
			this.Controls.Add(this.Label);
			this.Controls.Add(this.Button);
			this.Controls.Add(this.PictureBox);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Eighth task";
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TrackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox PictureBox;
		private System.Windows.Forms.Button Button;
		private System.Windows.Forms.TrackBar TrackBar;
		private System.Windows.Forms.ComboBox ComboBox;
		private System.Windows.Forms.Label Label;
	}
}

