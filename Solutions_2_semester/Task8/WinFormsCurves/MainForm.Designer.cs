namespace WinFormsCurves
{
	partial class SimpleCurves
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.SettingsPanel = new System.Windows.Forms.Panel();
			this.CurveSize = new System.Windows.Forms.TrackBar();
			this.FormulaBox = new System.Windows.Forms.GroupBox();
			this.BLabel = new System.Windows.Forms.Label();
			this.ALabel = new System.Windows.Forms.Label();
			this.BTextBox = new System.Windows.Forms.TextBox();
			this.ATextBox = new System.Windows.Forms.TextBox();
			this.CurveComboBox = new System.Windows.Forms.ComboBox();
			this.CurveArea = new System.Windows.Forms.Panel();
			this.SelectedCurve = new System.Windows.Forms.BindingSource(this.components);
			this.MainPanel.SuspendLayout();
			this.SettingsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.CurveSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectedCurve)).BeginInit();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.SettingsPanel);
			this.MainPanel.Controls.Add(this.CurveArea);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(662, 423);
			this.MainPanel.TabIndex = 0;
			// 
			// SettingsPanel
			// 
			this.SettingsPanel.BackColor = System.Drawing.SystemColors.ControlLight;
			this.SettingsPanel.Controls.Add(this.CurveSize);
			this.SettingsPanel.Controls.Add(this.FormulaBox);
			this.SettingsPanel.Controls.Add(this.BLabel);
			this.SettingsPanel.Controls.Add(this.ALabel);
			this.SettingsPanel.Controls.Add(this.BTextBox);
			this.SettingsPanel.Controls.Add(this.ATextBox);
			this.SettingsPanel.Controls.Add(this.CurveComboBox);
			this.SettingsPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.SettingsPanel.Location = new System.Drawing.Point(491, 0);
			this.SettingsPanel.Name = "SettingsPanel";
			this.SettingsPanel.Size = new System.Drawing.Size(171, 423);
			this.SettingsPanel.TabIndex = 0;
			// 
			// CurveSize
			// 
			this.CurveSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CurveSize.BackColor = System.Drawing.SystemColors.Window;
			this.CurveSize.Location = new System.Drawing.Point(3, 364);
			this.CurveSize.Name = "CurveSize";
			this.CurveSize.Size = new System.Drawing.Size(165, 56);
			this.CurveSize.TabIndex = 3;
			this.CurveSize.Scroll += new System.EventHandler(this.DrawCurve);
			this.CurveSize.Leave += new System.EventHandler(this.DrawCurve);
			// 
			// FormulaBox
			// 
			this.FormulaBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FormulaBox.BackColor = System.Drawing.SystemColors.Window;
			this.FormulaBox.Location = new System.Drawing.Point(3, 89);
			this.FormulaBox.Name = "FormulaBox";
			this.FormulaBox.Size = new System.Drawing.Size(165, 269);
			this.FormulaBox.TabIndex = 4;
			this.FormulaBox.TabStop = false;
			this.FormulaBox.Tag = "";
			// 
			// BLabel
			// 
			this.BLabel.AutoSize = true;
			this.BLabel.Location = new System.Drawing.Point(3, 64);
			this.BLabel.Name = "BLabel";
			this.BLabel.Size = new System.Drawing.Size(16, 17);
			this.BLabel.TabIndex = 5;
			this.BLabel.Text = "b";
			// 
			// ALabel
			// 
			this.ALabel.AutoSize = true;
			this.ALabel.Location = new System.Drawing.Point(3, 36);
			this.ALabel.Name = "ALabel";
			this.ALabel.Size = new System.Drawing.Size(16, 17);
			this.ALabel.TabIndex = 4;
			this.ALabel.Text = "a";
			// 
			// BTextBox
			// 
			this.BTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BTextBox.Location = new System.Drawing.Point(25, 61);
			this.BTextBox.Name = "BTextBox";
			this.BTextBox.Size = new System.Drawing.Size(143, 22);
			this.BTextBox.TabIndex = 2;
			this.BTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BTextBoxKeyDown);
			this.BTextBox.Leave += new System.EventHandler(this.DrawCurve);
			// 
			// ATextBox
			// 
			this.ATextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ATextBox.Location = new System.Drawing.Point(25, 33);
			this.ATextBox.Name = "ATextBox";
			this.ATextBox.Size = new System.Drawing.Size(143, 22);
			this.ATextBox.TabIndex = 1;
			this.ATextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ATextBoxKeyDown);
			this.ATextBox.Leave += new System.EventHandler(this.DrawCurve);
			// 
			// CurveComboBox
			// 
			this.CurveComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CurveComboBox.Location = new System.Drawing.Point(3, 3);
			this.CurveComboBox.Name = "CurveComboBox";
			this.CurveComboBox.Size = new System.Drawing.Size(165, 24);
			this.CurveComboBox.TabIndex = 0;
			this.CurveComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CurveComboBoxKeyDown);
			this.CurveComboBox.Leave += new System.EventHandler(this.CurveComboBoxLeave);
			// 
			// CurveArea
			// 
			this.CurveArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CurveArea.BackColor = System.Drawing.SystemColors.Window;
			this.CurveArea.Location = new System.Drawing.Point(0, 0);
			this.CurveArea.Name = "CurveArea";
			this.CurveArea.Size = new System.Drawing.Size(488, 423);
			this.CurveArea.TabIndex = 0;
			// 
			// SelectedCurve
			// 
			this.SelectedCurve.DataSource = typeof(WinFormsCurves.SimpleCurves);
			// 
			// SimpleCurves
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(662, 423);
			this.Controls.Add(this.MainPanel);
			this.MinimumSize = new System.Drawing.Size(480, 280);
			this.Name = "SimpleCurves";
			this.Text = "SimpleCurves";
			this.Resize += new System.EventHandler(this.DrawCurve);
			this.MainPanel.ResumeLayout(false);
			this.SettingsPanel.ResumeLayout(false);
			this.SettingsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.CurveSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectedCurve)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Panel SettingsPanel;
		private System.Windows.Forms.Label ALabel;
		private System.Windows.Forms.TextBox BTextBox;
		private System.Windows.Forms.TextBox ATextBox;
		private System.Windows.Forms.ComboBox CurveComboBox;
		private System.Windows.Forms.Panel CurveArea;
		private System.Windows.Forms.Label BLabel;
		private System.Windows.Forms.GroupBox FormulaBox;
		private System.Windows.Forms.TrackBar CurveSize;
		private System.Windows.Forms.BindingSource SelectedCurve;
	}
}

