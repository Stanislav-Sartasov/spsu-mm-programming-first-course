namespace UI.WF
{
    partial class MainWindow
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
            this.panel = new System.Windows.Forms.Panel();
            this.p = new System.Windows.Forms.TextBox();
            this.textBoxP = new System.Windows.Forms.TextBox();
            this.b = new System.Windows.Forms.TextBox();
            this.textBoxB = new System.Windows.Forms.TextBox();
            this.a = new System.Windows.Forms.TextBox();
            this.textBoxA = new System.Windows.Forms.TextBox();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.windowScale = new System.Windows.Forms.TextBox();
            this.chooseCurve = new System.Windows.Forms.ComboBox();
            this.Clear = new System.Windows.Forms.Button();
            this.Build = new System.Windows.Forms.Button();
            this.drawingArea = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoSize = true;
            this.panel.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel.Controls.Add(this.p);
            this.panel.Controls.Add(this.textBoxP);
            this.panel.Controls.Add(this.b);
            this.panel.Controls.Add(this.textBoxB);
            this.panel.Controls.Add(this.a);
            this.panel.Controls.Add(this.textBoxA);
            this.panel.Controls.Add(this.textBoxScale);
            this.panel.Controls.Add(this.windowScale);
            this.panel.Controls.Add(this.chooseCurve);
            this.panel.Controls.Add(this.Clear);
            this.panel.Controls.Add(this.Build);
            this.panel.Location = new System.Drawing.Point(633, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(167, 452);
            this.panel.TabIndex = 0;
            // 
            // p
            // 
            this.p.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.p.Dock = System.Windows.Forms.DockStyle.Left;
            this.p.Location = new System.Drawing.Point(120, 66);
            this.p.Name = "p";
            this.p.Size = new System.Drawing.Size(25, 13);
            this.p.TabIndex = 10;
            // 
            // textBoxP
            // 
            this.textBoxP.BackColor = System.Drawing.SystemColors.GrayText;
            this.textBoxP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxP.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxP.Location = new System.Drawing.Point(95, 66);
            this.textBoxP.Name = "textBoxP";
            this.textBoxP.ReadOnly = true;
            this.textBoxP.Size = new System.Drawing.Size(25, 13);
            this.textBoxP.TabIndex = 9;
            this.textBoxP.Text = " p =";
            // 
            // b
            // 
            this.b.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.b.Dock = System.Windows.Forms.DockStyle.Left;
            this.b.Location = new System.Drawing.Point(70, 66);
            this.b.Name = "b";
            this.b.Size = new System.Drawing.Size(25, 13);
            this.b.TabIndex = 8;
            // 
            // textBoxB
            // 
            this.textBoxB.BackColor = System.Drawing.SystemColors.GrayText;
            this.textBoxB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxB.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxB.Location = new System.Drawing.Point(45, 66);
            this.textBoxB.Name = "textBoxB";
            this.textBoxB.ReadOnly = true;
            this.textBoxB.Size = new System.Drawing.Size(25, 13);
            this.textBoxB.TabIndex = 7;
            this.textBoxB.Text = " b =";
            // 
            // a
            // 
            this.a.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.a.Dock = System.Windows.Forms.DockStyle.Left;
            this.a.Location = new System.Drawing.Point(22, 66);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(23, 13);
            this.a.TabIndex = 6;
            // 
            // textBoxA
            // 
            this.textBoxA.BackColor = System.Drawing.SystemColors.GrayText;
            this.textBoxA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxA.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxA.Location = new System.Drawing.Point(0, 66);
            this.textBoxA.Name = "textBoxA";
            this.textBoxA.ReadOnly = true;
            this.textBoxA.Size = new System.Drawing.Size(22, 13);
            this.textBoxA.TabIndex = 5;
            this.textBoxA.Text = " a = ";
            // 
            // textBoxScale
            // 
            this.textBoxScale.BackColor = System.Drawing.SystemColors.GrayText;
            this.textBoxScale.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxScale.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxScale.Location = new System.Drawing.Point(0, 419);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.ReadOnly = true;
            this.textBoxScale.Size = new System.Drawing.Size(167, 13);
            this.textBoxScale.TabIndex = 4;
            this.textBoxScale.Text = "Scale (from 50 to 200)";
            this.textBoxScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // windowScale
            // 
            this.windowScale.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.windowScale.Location = new System.Drawing.Point(0, 432);
            this.windowScale.Name = "windowScale";
            this.windowScale.Size = new System.Drawing.Size(167, 20);
            this.windowScale.TabIndex = 3;
            // 
            // chooseCurve
            // 
            this.chooseCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chooseCurve.FormattingEnabled = true;
            this.chooseCurve.Location = new System.Drawing.Point(3, 85);
            this.chooseCurve.Name = "chooseCurve";
            this.chooseCurve.Size = new System.Drawing.Size(161, 21);
            this.chooseCurve.TabIndex = 2;
            // 
            // Clear
            // 
            this.Clear.Dock = System.Windows.Forms.DockStyle.Top;
            this.Clear.Location = new System.Drawing.Point(0, 30);
            this.Clear.Name = "Clear";
            this.Clear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Clear.Size = new System.Drawing.Size(167, 36);
            this.Clear.TabIndex = 1;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // Build
            // 
            this.Build.Dock = System.Windows.Forms.DockStyle.Top;
            this.Build.Location = new System.Drawing.Point(0, 0);
            this.Build.Name = "Build";
            this.Build.Size = new System.Drawing.Size(167, 30);
            this.Build.TabIndex = 0;
            this.Build.Text = "BuildFunction";
            this.Build.UseVisualStyleBackColor = true;
            this.Build.Click += new System.EventHandler(this.Build_Click);
            // 
            // drawingArea
            // 
            this.drawingArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawingArea.BackColor = System.Drawing.SystemColors.Window;
            this.drawingArea.Location = new System.Drawing.Point(0, 0);
            this.drawingArea.Name = "drawingArea";
            this.drawingArea.Size = new System.Drawing.Size(630, 452);
            this.drawingArea.TabIndex = 5;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.drawingArea);
            this.Controls.Add(this.panel);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button Build;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.TextBox windowScale;
        private System.Windows.Forms.ComboBox chooseCurve;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Panel drawingArea;
        private System.Windows.Forms.TextBox textBoxA;
        private System.Windows.Forms.TextBox p;
        private System.Windows.Forms.TextBox textBoxP;
        private System.Windows.Forms.TextBox b;
        private System.Windows.Forms.TextBox textBoxB;
        private System.Windows.Forms.TextBox a;
    }
}

