namespace WF
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
            this.buildButton = new System.Windows.Forms.Button();
            this.collectionOfCurves = new System.Windows.Forms.ComboBox();
            this.graphArea = new System.Windows.Forms.Panel();
            this.currentScale = new System.Windows.Forms.Label();
            this.boxScale = new System.Windows.Forms.TextBox();
            this.plusScaleButton = new System.Windows.Forms.Button();
            this.minusScaleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buildButton
            // 
            this.buildButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buildButton.Location = new System.Drawing.Point(12, 372);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(151, 29);
            this.buildButton.TabIndex = 0;
            this.buildButton.Text = "Build";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // collectionOfCurves
            // 
            this.collectionOfCurves.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.collectionOfCurves.FormattingEnabled = true;
            this.collectionOfCurves.Location = new System.Drawing.Point(12, 20);
            this.collectionOfCurves.Name = "collectionOfCurves";
            this.collectionOfCurves.Size = new System.Drawing.Size(151, 28);
            this.collectionOfCurves.TabIndex = 1;
            // 
            // graphArea
            // 
            this.graphArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphArea.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.graphArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graphArea.Cursor = System.Windows.Forms.Cursors.Default;
            this.graphArea.Location = new System.Drawing.Point(362, 12);
            this.graphArea.Name = "graphArea";
            this.graphArea.Size = new System.Drawing.Size(341, 389);
            this.graphArea.TabIndex = 2;
            // 
            // currentScale
            // 
            this.currentScale.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.currentScale.AutoSize = true;
            this.currentScale.Location = new System.Drawing.Point(12, 173);
            this.currentScale.Name = "currentScale";
            this.currentScale.Size = new System.Drawing.Size(97, 20);
            this.currentScale.TabIndex = 4;
            this.currentScale.Text = "Current scale:";
            // 
            // boxScale
            // 
            this.boxScale.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.boxScale.Location = new System.Drawing.Point(113, 166);
            this.boxScale.Name = "boxScale";
            this.boxScale.ReadOnly = true;
            this.boxScale.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.boxScale.Size = new System.Drawing.Size(62, 27);
            this.boxScale.TabIndex = 3;
            this.boxScale.Text = "1";
            // 
            // plusScaleButton
            // 
            this.plusScaleButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.plusScaleButton.Location = new System.Drawing.Point(113, 200);
            this.plusScaleButton.Name = "plusScaleButton";
            this.plusScaleButton.Size = new System.Drawing.Size(28, 29);
            this.plusScaleButton.TabIndex = 5;
            this.plusScaleButton.Text = "+";
            this.plusScaleButton.UseVisualStyleBackColor = true;
            this.plusScaleButton.Click += new System.EventHandler(this.plusScaleButton_Click);
            // 
            // minusScaleButton
            // 
            this.minusScaleButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.minusScaleButton.Location = new System.Drawing.Point(147, 200);
            this.minusScaleButton.Name = "minusScaleButton";
            this.minusScaleButton.Size = new System.Drawing.Size(28, 29);
            this.minusScaleButton.TabIndex = 5;
            this.minusScaleButton.Text = "-";
            this.minusScaleButton.UseVisualStyleBackColor = true;
            this.minusScaleButton.Click += new System.EventHandler(this.minusScaleButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 413);
            this.Controls.Add(this.minusScaleButton);
            this.Controls.Add(this.plusScaleButton);
            this.Controls.Add(this.currentScale);
            this.Controls.Add(this.graphArea);
            this.Controls.Add(this.boxScale);
            this.Controls.Add(this.collectionOfCurves);
            this.Controls.Add(this.buildButton);
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "MainWindow";
            this.Text = "Main Window";
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.ComboBox collectionOfCurves;
        private System.Windows.Forms.Panel graphArea;
        private System.Windows.Forms.TextBox boxScale;
        private System.Windows.Forms.Label currentScale;
        private System.Windows.Forms.Button plusScaleButton;
        private System.Windows.Forms.Button minusScaleButton;
    }
}

