using System;
using System.Net;

namespace Task9WF.Interfaces
{
    public interface IListener
    {
        event EventHandler<Connection> NewConnection;
        bool Started { get; }
        EndPoint LocalAddress { get; }
        TaskManager TaskManager { set; }
        IMessager Messager { set; }
        bool Init();
        bool Init(int startPort);
        bool Init(int[] portsList);
        bool Init(int portMin, int portMax);
        void Start(int backlog);
        void Stop();
    }
}
