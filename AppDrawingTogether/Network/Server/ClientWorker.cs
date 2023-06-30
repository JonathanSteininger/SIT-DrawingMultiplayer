using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using DrawingTogether.Net;
using DrawingTogether;

namespace AppDrawingTogether.Network.Server
{
    internal class ClientWorker
    {
        private NetworkConnection _conn;

        private string playerName = "Guest";

        private ClientWorkerManager _workerManager;
        public ClientWorker(TcpClient client, ClientWorkerManager ParentManager) {
            _conn = new NetworkConnection(client);
            _workerManager = ParentManager;
        }

        private bool _completed = false;
        public bool Completed { get { return _completed; } }

        Thread _workerThread;

        public void Start()
        {
            _workerThread = Thread.CurrentThread;
        }
        public void Update()
        {
            ReadClientStream();
        }

        private void ReadClientStream()
        {
            object obj = _conn.ReadAuto();
            if (obj is PlayerName) ServerResponse(obj as PlayerName);
            if (obj is LineDataTransfer) ServerResponse(obj as LineDataTransfer);
            if (obj is Command) ServerResponse(obj as Command);
        }

        private void ServerResponse(Command recivedObject)
        {
            if (recivedObject == null) return;
            if(recivedObject.CommandType == GameCommand.Quit)
            {
                Stop();
            }
        }
        private void ServerResponse(LineDataTransfer recivedObject)
        {
            if (recivedObject == null) return;
            _workerManager.UpdateLines(recivedObject.Portions);
            _workerManager.RequestToSendLines(this);
        }

        public void SendLinesToClient(List<LinePortion> lines)
        {
            _conn.Write(new LineDataTransfer(lines));
        }

        private void ServerResponse(PlayerName recivedObject)
        {
            if (recivedObject == null) return;
            playerName = recivedObject.Name;
        }

        public void Run()
        {
            Start();
            while (!_completed)
            {
                Update();
            }
        }
        public void Stop()
        {
            _completed = true;
            _workerManager.RemoveWorker(this);
            _workerThread.Abort();
        }
    }
}
