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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// startPortLabel
			// 
			this.startPortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.startPortLabel.AutoSize = true;
			this.startPortLabel.Location = new System.Drawing.Point(12, 51);
			this.startPortLabel.Name = "startPortLabel";
			this.startPortLabel.Size = new System.Drawing.Size(123, 17);
			this.startPortLabel.TabIndex = 0;
			this.startPortLabel.Text = "START AT PORT:";
			// 
			// portTextBox
			// 
			this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.portTextBox.Location = new System.Drawing.Point(141, 48);
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size(179, 22);
			this.portTextBox.TabIndex = 1;
			this.portTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PortTextBoxKeyDown);
			// 
			// startButton
			// 
			this.startButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.startButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.startButton.Location = new System.Drawing.Point(245, 76);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 2;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(180, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "Close for default port";
			// 
			// StartPort
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(332, 113);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.portTextBox);
			this.Controls.Add(this.startPortLabel);
			this.MaximumSize = new System.Drawing.Size(350, 160);
			this.MinimumSize = new System.Drawing.Size(350, 160);
			this.Name = "StartPort";
			this.Text = "StartPort";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label startPortLabel;
		private System.Windows.Forms.TextBox portTextBox;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Label label1;
	}
}