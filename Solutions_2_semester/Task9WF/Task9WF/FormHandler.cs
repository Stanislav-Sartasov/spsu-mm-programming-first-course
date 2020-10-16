using System;
using System.Net;
using System.Windows.Forms;
using Task9WF.Interfaces;

namespace Task9WF
{
    class FormHandler : IDataConsumer
    {
        TaskManager tasks = null;
        object constDataLocker = new object();
        MainForm form = null;
        bool started = false;
        public bool Started
        {
            get
            {
                if (form == null || tasks == null)
                    return false;
                return form.FormShown;
            }
        }

        public TaskManager TaskManager
        {
            set
            {
                lock (constDataLocker)
                {
                    if (tasks == null)
                        tasks = value;
                }
            }
        }
        public event EventHandler<Message> NewInput;
        public event EventHandler Stopped;

        public int RequestStartPort()
        {
            if (form == null)
                return -1;
            return form.RequestStartPort();
        }

        public void AddMessage(object sender, string message)
        {
            if (form == null)
                return;
            form.AddMessage(sender, message);
        }

        public void ChangeConnection(object endPoint, string name)
        {
            if (form == null)
                return;
            if (endPoint.GetType().Name == "IPEndPoint")
                form.ChangeConnection((EndPoint)endPoint, name);
        }

        public void Start()
        {
            if (started || tasks == null)
                return;
            else
                started = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new MainForm(NewInput, Stopped, tasks);
            Application.Run(form);
        }
    }
}
