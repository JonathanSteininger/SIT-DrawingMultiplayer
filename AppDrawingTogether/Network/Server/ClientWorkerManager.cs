using DrawingTogether.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDrawingTogether.Network.Server
{
    internal class ClientWorkerManager
    {
        
        public List<ClientWorker> Workers;

        public LineManager LineManager;

        public ClientWorkerManager(LineManager lineManager)
        {
            Workers = new List<ClientWorker>();
            LineManager = lineManager;
        }
        public void AddWorker(ClientWorker worker)
        {
            Workers.Add(worker);
        }
        internal void RequestToSendLines(ClientWorker sender)
        {
            LineManager.LinesToClients(sender);
        }

        public void RemoveWorker(ClientWorker worker)
        {
            Workers.Remove(worker);
        }

        internal void UpdateLines(List<DrawingTogether.LinePortion> lines)
        {
            LineManager.AddLinesFromClient(lines);
        }

        internal void StopAllWorkers()
        {
            foreach(ClientWorker worker in Workers)
            {
                worker.Stop();
            }
        }
    }
}
