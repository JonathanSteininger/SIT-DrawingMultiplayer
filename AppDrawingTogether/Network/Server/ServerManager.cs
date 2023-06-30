using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDrawingTogether.Network.Server
{
    internal class ServerManager
    {
        private bool IsMultiplayer = false;

        private TcpListener listner;

        LineManager LineManager;

        ClientWorkerManager ClientWorkerManager;

        string ip = "127.0.0.1";
        int port;
        public ServerManager()
        {

        } 
        public ServerManager(int port, LineManager lineManager) 
        {
            LineManager = lineManager;
            ClientWorkerManager = new ClientWorkerManager(lineManager);
            IsMultiplayer = true;
            this.port = port;
            IPAddress ad = IPAddress.Parse(ip);
            listner = new TcpListener(ad, this.port);

            listner.Start();
            StartServer();
        }

        public async void StartServer()
        {
            try
            {
                TcpClient client = await listner.AcceptTcpClientAsync();
                ClientWorker worker = new ClientWorker(client, ClientWorkerManager);
                Thread thread = new Thread(worker.Run);
                thread.Start();
            }catch (Exception ex)
            {

            }
        }

        public void Stop()
        {
            listner.Stop();
            ClientWorkerManager.StopAllWorkers();
        }

        



    }
}
