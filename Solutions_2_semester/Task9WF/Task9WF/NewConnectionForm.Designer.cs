namespace Task9WF
{
	partial class NewConnectionForm
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
			this.topPanell = new System.Windows.Forms.Panel();
			this.addressLabel = new System.Windows.Forms.Label();
			this.addressTextBox = new System.Windows.Forms.TextBox();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.tryConnectButton = new System.Windows.Forms.Button();
			this.portLabel = new System.Windows.Forms.Label();
			this.portTextBox = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.mainPanel.SuspendLayout();
			this.topPanell.SuspendLayout();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.mainPanel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(428, 286);
			this.panel1.TabIndex = 0;
			// 
			// mainPanel
			// 
			this.mainPanel.ColumnCount = 1;
			this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainPanel.Controls.Add(this.topPanell, 0, 0);
			this.mainPanel.Controls.Add(this.bottomPanel, 0, 1);
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(0, 0);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.RowCount = 2;
			this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainPanel.Size = new System.Drawing.Size(428, 286);
			this.mainPanel.TabIndex = 4;
			// 
			// topPanell
			// 
			this.topPanell.Controls.Add(this.addressLabel);
			this.topPanell.Controls.Add(this.addressTextBox);
			this.topPanell.Dock = System.Windows.Forms.DockStyle.Fill;
			this.topPanell.Location = new System.Drawing.Point(3, 3);
			this.topPanell.Name = "topPanell";
			this.topPanell.Size = new System.Drawing.Size(422, 137);
			this.topPanell.TabIndex = 1;
			// 
			// addressLabel
			// 
			this.addressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.addressLabel.AutoSize = true;
			this.addressLabel.Location = new System.Drawing.Point(3, 115);
			this.addressLabel.Name = "addressLabel";
			this.addressLabel.Size = new System.Drawing.Size(78, 17);
			this.addressLabel.TabIndex = 0;
			this.addressLabel.Text = "ADDRESS:";
			// 
			// addressTextBox
			// 
			this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.addressTextBox.Location = new System.Drawing.Point(87, 112);
			this.addressTextBox.Name = "addressTextBox";
			this.addressTextBox.Size = new System.Drawing.Size(332, 22);
			this.addressTextBox.TabIndex = 1;
			this.addressTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressTextBoxKeyDown);
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.tryConnectButton);
			this.bottomPanel.Controls.Add(this.portLabel);
			this.bottomPanel.Controls.Add(this.portTextBox);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bottomPanel.Location = new System.Drawing.Point(3, 146);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(422, 137);
			this.bottomPanel.TabIndex = 2;
			// 
			// tryConnectButton
			// 
			this.tryConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tryConnectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.tryConnectButton.Location = new System.Drawing.Point(279, 31);
			this.tryConnectButton.MaximumSize = new System.Drawing.Size(140, 25);
			this.tryConnectButton.MinimumSize = new System.Drawing.Size(140, 25);
			this.tryConnectButton.Name = "tryConnectButton";
			this.tryConnectButton.Size = new System.Drawing.Size(140, 25);
			this.tryConnectButton.TabIndex = 4;
			this.tryConnectButton.Text = "TRY_CONNECT";
			this.tryConnectButton.UseVisualStyleBackColor = true;
			this.tryConnectButton.Click += new System.EventHandler(this.TryConnectButtonClick);
			// 
			// portLabel
			// 
			this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.portLabel.AutoSize = true;
			this.portLabel.Location = new System.Drawing.Point(3, 6);
			this.portLabel.Name = "portLabel";
			this.portLabel.Size = new System.Drawing.Size(51, 17);
			this.portLabel.TabIndex = 2;
			this.portLabel.Text = "PORT:";
			// 
			// portTextBox
			// 
			this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.portTextBox.Location = new System.Drawing.Point(87, 3);
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size(332, 22);
			this.portTextBox.TabIndex = 3;
			this.portTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PortTextBoxKeyDown);
			// 
			// NewConnectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(428, 286);
			this.Controls.Add(this.panel1);
			this.MaximumSize = new System.Drawing.Size(446, 333);
			this.MinimumSize = new System.Drawing.Size(446, 333);
			this.Name = "NewConnectionForm";
			this.Text = "NewConnection";
			this.panel1.ResumeLayout(false);
			this.mainPanel.ResumeLayout(false);
			this.topPanell.ResumeLayout(false);
			this.topPanell.PerformLayout();
			this.bottomPanel.ResumeLayout(false);
			this.bottomPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label addressLabel;
		private System.Windows.Forms.TextBox portTextBox;
		private System.Windows.Forms.Label portLabel;
		private System.Windows.Forms.TextBox addressTextBox;
		private System.Windows.Forms.TableLayoutPanel mainPanel;
		private System.Windows.Forms.Panel topPanell;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Button tryConnectButton;
	}
}