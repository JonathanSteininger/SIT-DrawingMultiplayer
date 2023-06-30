using DrawingTogether.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDrawingTogether.Network.Client
{
    internal class Connection
    {
        ClientLocalWorker worker;
        LineManager lineManager;
        string PlayerName = "Guest";

        private int TickMS = 100;
        public Connection(int port, LineManager lineManager, string playerName, float UpdateFramerate)
        {
            this.PlayerName = playerName;
            this.lineManager = lineManager;
            NetworkConnection temp = new NetworkConnection("127.0.0.1", port);
            worker = new ClientLocalWorker(playerName, (int)(1000 *(1/UpdateFramerate)), lineManager, temp);
            StartUpdating();
        }

        private void StartUpdating()
        {
            Thread thread = new Thread(worker.Run);
            thread.Start();
        }
        public void Stop()
        {
            worker.Stop();
        }
    }
}
