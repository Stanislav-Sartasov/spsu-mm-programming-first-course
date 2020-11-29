namespace Task9WF
{
	partial class StartPort
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.startPortLabel = new System.Windows.Forms.Label();
			this.portTextBox = new System.Windows.Forms.TextBox();
			this.startButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// startPortLabel
			// 
			this.startPortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.startPortLabel.AutoSize = true;
			this.startPortLabel.Location = new System.Drawing.Point(12, 101);
			this.startPortLabel.Name = "startPortLabel";
			this.startPortLabel.Size = new System.Drawing.Size(87, 17);
			this.startPortLabel.TabIndex = 0;
			this.startPortLabel.Text = "Start at port:";
			// 
			// portTextBox
			// 
			this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.portTextBox.Location = new System.Drawing.Point(99, 98);
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size(221, 22);
			this.portTextBox.TabIndex = 1;
			this.portTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PortTextBoxKeyDown);
			// 
			// startButton
			// 
			this.startButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.startButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.startButton.Location = new System.Drawing.Point(245, 126);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 2;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			// 
			// StartPort
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(332, 213);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.portTextBox);
			this.Controls.Add(this.startPortLabel);
			this.MaximumSize = new System.Drawing.Size(350, 260);
			this.MinimumSize = new System.Drawing.Size(350, 260);
			this.Name = "StartPort";
			this.Text = "StartPort";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label startPortLabel;
		private System.Windows.Forms.TextBox portTextBox;
		private System.Windows.Forms.Button startButton;
	}
}