namespace Task9WF
{
    partial class MainForm
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
            this.connectionsMainPanel = new System.Windows.Forms.Panel();
            this.connectionsPanel = new System.Windows.Forms.Panel();
            this.connectionsTextBox = new System.Windows.Forms.RichTextBox();
            this.addConnectionButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.messagingMainPanel = new System.Windows.Forms.Panel();
            this.messagesTextBox = new System.Windows.Forms.RichTextBox();
            this.newMessageTextBox = new System.Windows.Forms.TextBox();
            this.autoRefresh = new System.Windows.Forms.Timer(this.components);
            this.connectionsMainPanel.SuspendLayout();
            this.connectionsPanel.SuspendLayout();
            this.messagingMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionsMainPanel
            // 
            this.connectionsMainPanel.Controls.Add(this.connectionsPanel);
            this.connectionsMainPanel.Controls.Add(this.nameTextBox);
            this.connectionsMainPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.connectionsMainPanel.Location = new System.Drawing.Point(0, 0);
            this.connectionsMainPanel.Name = "connectionsMainPanel";
            this.connectionsMainPanel.Size = new System.Drawing.Size(200, 444);
            this.connectionsMainPanel.TabIndex = 0;
            // 
            // connectionsPanel
            // 
            this.connectionsPanel.Controls.Add(this.connectionsTextBox);
            this.connectionsPanel.Controls.Add(this.addConnectionButton);
            this.connectionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionsPanel.Location = new System.Drawing.Point(0, 0);
            this.connectionsPanel.Name = "connectionsPanel";
            this.connectionsPanel.Size = new System.Drawing.Size(200, 422);
            this.connectionsPanel.TabIndex = 1;
            // 
            // connectionsTextBox
            // 
            this.connectionsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.connectionsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionsTextBox.Location = new System.Drawing.Point(0, 0);
            this.connectionsTextBox.Name = "connectionsTextBox";
            this.connectionsTextBox.ReadOnly = true;
            this.connectionsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.connectionsTextBox.Size = new System.Drawing.Size(200, 399);
            this.connectionsTextBox.TabIndex = 0;
            this.connectionsTextBox.Text = "";
            this.connectionsTextBox.Enter += new System.EventHandler(this.ConnectionsTextBox_Enter);
            this.connectionsTextBox.Leave += new System.EventHandler(this.ConnectionsTextBox_Leave);
            // 
            // addConnectionButton
            // 
            this.addConnectionButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addConnectionButton.Location = new System.Drawing.Point(0, 399);
            this.addConnectionButton.Name = "addConnectionButton";
            this.addConnectionButton.Size = new System.Drawing.Size(200, 23);
            this.addConnectionButton.TabIndex = 1;
            this.addConnectionButton.Text = "add connection";
            this.addConnectionButton.UseVisualStyleBackColor = true;
            this.addConnectionButton.Click += new System.EventHandler(this.AddConnectionButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.nameTextBox.Location = new System.Drawing.Point(0, 422);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 22);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Enter += new System.EventHandler(this.NameTextBox_Enter);
            this.nameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameTextBox_KeyDown);
            this.nameTextBox.Leave += new System.EventHandler(this.NameTextBox_Leave);
            // 
            // messagingMainPanel
            // 
            this.messagingMainPanel.Controls.Add(this.messagesTextBox);
            this.messagingMainPanel.Controls.Add(this.newMessageTextBox);
            this.messagingMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagingMainPanel.Location = new System.Drawing.Point(200, 0);
            this.messagingMainPanel.Name = "messagingMainPanel";
            this.messagingMainPanel.Size = new System.Drawing.Size(716, 444);
            this.messagingMainPanel.TabIndex = 0;
            // 
            // messagesTextBox
            // 
            this.messagesTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.messagesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesTextBox.Location = new System.Drawing.Point(0, 0);
            this.messagesTextBox.Name = "messagesTextBox";
            this.messagesTextBox.ReadOnly = true;
            this.messagesTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.messagesTextBox.ShowSelectionMargin = true;
            this.messagesTextBox.Size = new System.Drawing.Size(716, 422);
            this.messagesTextBox.TabIndex = 1;
            this.messagesTextBox.Text = "";
            this.messagesTextBox.Enter += new System.EventHandler(this.MessagesTextBox_Enter);
            this.messagesTextBox.Leave += new System.EventHandler(this.MessagesTextBox_Leave);
            // 
            // newMessageTextBox
            // 
            this.newMessageTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.newMessageTextBox.Location = new System.Drawing.Point(0, 422);
            this.newMessageTextBox.Name = "newMessageTextBox";
            this.newMessageTextBox.Size = new System.Drawing.Size(716, 22);
            this.newMessageTextBox.TabIndex = 0;
            this.newMessageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewMessageTextBox_KeyDown);
            // 
            // autoRefresh
            // 
            this.autoRefresh.Enabled = true;
            this.autoRefresh.Interval = 250;
            this.autoRefresh.Tick += new System.EventHandler(this.Refresh_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 444);
            this.Controls.Add(this.messagingMainPanel);
            this.Controls.Add(this.connectionsMainPanel);
            this.MinimumSize = new System.Drawing.Size(540, 230);
            this.Name = "MainForm";
            this.Text = "Messager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.connectionsMainPanel.ResumeLayout(false);
            this.connectionsMainPanel.PerformLayout();
            this.connectionsPanel.ResumeLayout(false);
            this.messagingMainPanel.ResumeLayout(false);
            this.messagingMainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel connectionsMainPanel;
        private System.Windows.Forms.Panel messagingMainPanel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.RichTextBox messagesTextBox;
        private System.Windows.Forms.TextBox newMessageTextBox;
        private System.Windows.Forms.Panel connectionsPanel;
        private System.Windows.Forms.RichTextBox connectionsTextBox;
        private System.Windows.Forms.Button addConnectionButton;
        private System.Windows.Forms.Timer autoRefresh;
    }
}

