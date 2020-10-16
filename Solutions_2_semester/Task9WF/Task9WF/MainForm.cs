using System;
using System.Collections;
using System.Net;
using System.Windows.Forms;

namespace Task9WF
{
    public partial class MainForm : Form
    {
        public MainForm(EventHandler<Message> newInput, EventHandler stop, TaskManager taskManager)
        {
            InitializeComponent();
            NewInput += newInput;
            Stop += stop;
            tasks = taskManager;
            nameTextBox.Text = "Your name:";
        }

        public int RequestStartPort()
        {
            int port = -1;
            MethodInvoker methodInvoker = delegate ()
            {
                StartPort startPortForm = new StartPort();
                if (startPortForm.ShowDialog() == DialogResult.OK)
                    port = startPortForm.Port;
            };

            Invoke(methodInvoker);

            return port;
        }

        public bool FormShown { get; private set; } = false;
        private void MainForm_Shown(object sender, EventArgs e)
        {
            FormShown = true;
        }

        TaskManager tasks;
        event EventHandler<Message> NewInput;
        event EventHandler Stop;

        bool messagesChanged = true;
        bool connectionsChanged = true;
        bool messagesSelected = false;
        bool connectionsSelected = false;
        public readonly int startPort;

        string Connections
        {
            get
            {
                string outString = "Connections:";
                foreach (EndPoint str in connectionsList.Keys)
                    outString += "\n" + str.ToString() + " : " + connectionsList[str];
                return outString;
            }
        }

        Hashtable connectionsList = new Hashtable();

        public void AddMessage(object sender, string message)
        {
            MethodInvoker methodInvoker = delegate ()
            {
                if (connectionsList.ContainsKey(sender))
                {
                    string name = (string)connectionsList[sender];
                    messagesTextBox.Text += "\n" + sender.ToString() + " " + name + ": " + message;
                    ChangeConnection(sender, name);
                }
                else
                    messagesTextBox.Text += "\n" + message;

                messagesChanged = true;
            };

            Invoke(methodInvoker);
        }

        public void ChangeConnection(object sender, string name)
        {
            try
            {
                if (name == null)
                    RemoveConnection(sender);
                else
                {
                    MethodInvoker methodInvoker = delegate ()
                    {
                        connectionsList[sender] = name;
                        connectionsChanged = true;
                    };
                    Invoke(methodInvoker);
                }
            }
            catch { }
        }

        void RemoveConnection(object endPoint)
        {
            MethodInvoker methodInvoker = delegate ()
            {
                connectionsList.Remove(endPoint);
                connectionsChanged = true;
            };
            Invoke(methodInvoker);
        }

        private void AddConnectionButton_Click(object sender, EventArgs e)
        {
            NewConnectionForm form = new NewConnectionForm();
            if (form.ShowDialog() == DialogResult.OK)
                tasks.Run(() => NewInput(this, new Message { Type = MessageType.Socket, Text = form.Address + ":" + form.Port }));
        }

        private void Refresh_Tick(object sender, EventArgs e)
        {
            if (messagesChanged && !messagesSelected)
            {
                messagesTextBox.Refresh();
                messagesTextBox.SelectionStart = messagesTextBox.Text.Length;
                messagesTextBox.ScrollToCaret();
                messagesChanged = false;
            }
            if (connectionsChanged && !connectionsSelected)
            {
                connectionsTextBox.Text = Connections;
                connectionsTextBox.Refresh();
                connectionsChanged = false;
            }
        }

        private void NewMessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (newMessageTextBox.Text != "")
                {
                    string input = newMessageTextBox.Text;
                    newMessageTextBox.Text = "";
                    newMessageTextBox.Refresh();
                    tasks.Run(() => NewInput(this, new Message { Type = MessageType.Message, Text = input }));
                }
            }
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (nameTextBox.Text != "" && !nameTextBox.ReadOnly)
                {
                    string input = nameTextBox.Text;
                    nameTextBox.Text = "Name: " + input;
                    nameTextBox.ReadOnly = true;
                    nameTextBox.Refresh();
                    messagesTextBox.Focus();
                    tasks.Run(() => NewInput(this, new Message { Type = MessageType.Name, Text = input }));
                }
            }
        }

        private void NameTextBox_Enter(object sender, EventArgs e)
        {
            if (!nameTextBox.ReadOnly)
            {
                nameTextBox.Text = "";
                nameTextBox.Refresh();
            }
        }

        private void NameTextBox_Leave(object sender, EventArgs e)
        {
            if (!nameTextBox.ReadOnly)
            {
                nameTextBox.Text = "Your name:";
                nameTextBox.Refresh();
            }
        }

        private void MessagesTextBox_Enter(object sender, EventArgs e)
        {
            messagesSelected = true;
        }

        private void MessagesTextBox_Leave(object sender, EventArgs e)
        {
            messagesSelected = false;
        }

        private void ConnectionsTextBox_Enter(object sender, EventArgs e)
        {
            connectionsSelected = true;
        }

        private void ConnectionsTextBox_Leave(object sender, EventArgs e)
        {
            connectionsSelected = false;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop(this, EventArgs.Empty);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop(this, EventArgs.Empty);
        }
    }
}
