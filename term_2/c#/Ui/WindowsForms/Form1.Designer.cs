namespace WindowsForms
{
    partial class winForm
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
			this.comboBoxCurves = new System.Windows.Forms.ComboBox();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.buttonStart = new System.Windows.Forms.Button();
			this.plusSize = new System.Windows.Forms.Button();
			this.minusSize = new System.Windows.Forms.Button();
			this.labelSize = new System.Windows.Forms.Label();
			this.labelCurveChose = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBoxCurves
			// 
			this.comboBoxCurves.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCurves.Location = new System.Drawing.Point(13, 58);
			this.comboBoxCurves.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.comboBoxCurves.Name = "comboBoxCurves";
			this.comboBoxCurves.Size = new System.Drawing.Size(121, 24);
			this.comboBoxCurves.TabIndex = 0;
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(235, 12);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(600, 601);
			this.pictureBox.TabIndex = 1;
			this.pictureBox.TabStop = false;
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(13, 139);
			this.buttonStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(75, 28);
			this.buttonStart.TabIndex = 2;
			this.buttonStart.Text = "go";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// plusSize
			// 
			this.plusSize.Location = new System.Drawing.Point(12, 202);
			this.plusSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.plusSize.Name = "plusSize";
			this.plusSize.Size = new System.Drawing.Size(75, 23);
			this.plusSize.TabIndex = 3;
			this.plusSize.Text = "+ 0.1";
			this.plusSize.UseVisualStyleBackColor = true;
			this.plusSize.Click += new System.EventHandler(this.plusSize_Click);
			// 
			// minusSize
			// 
			this.minusSize.Location = new System.Drawing.Point(117, 202);
			this.minusSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.minusSize.Name = "minusSize";
			this.minusSize.Size = new System.Drawing.Size(75, 23);
			this.minusSize.TabIndex = 4;
			this.minusSize.Text = "- 0.1";
			this.minusSize.UseVisualStyleBackColor = true;
			this.minusSize.Click += new System.EventHandler(this.minusSize_Click);
			// 
			// labelSize
			// 
			this.labelSize.AutoSize = true;
			this.labelSize.Location = new System.Drawing.Point(35, 293);
			this.labelSize.Name = "labelSize";
			this.labelSize.Size = new System.Drawing.Size(55, 17);
			this.labelSize.TabIndex = 5;
			this.labelSize.Text = "Size: xx";
			// 
			// labelCurveChose
			// 
			this.labelCurveChose.AutoSize = true;
			this.labelCurveChose.Location = new System.Drawing.Point(13, 14);
			this.labelCurveChose.Name = "labelCurveChose";
			this.labelCurveChose.Size = new System.Drawing.Size(97, 17);
			this.labelCurveChose.TabIndex = 6;
			this.labelCurveChose.Text = "chose a curve";
			// 
			// winForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(855, 622);
			this.Controls.Add(this.labelCurveChose);
			this.Controls.Add(this.labelSize);
			this.Controls.Add(this.minusSize);
			this.Controls.Add(this.plusSize);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.comboBoxCurves);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MaximizeBox = false;
			this.Name = "winForm";
			this.Text = "Curves";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCurves;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button plusSize;
        private System.Windows.Forms.Button minusSize;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelCurveChose;
    }
}

