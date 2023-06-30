using DrawingTogether.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDrawingTogether.Network.Client
{
    internal class ClientLocalWorker
    {
        private string Name;
        private int time = 100;
        private Thread _startThead;
        LineManager _lineManager;

        NetworkConnection _connection;


        public ClientLocalWorker(string name, int TickLengthMS, LineManager lineManager, NetworkConnection Conn)
        {
            Name = name;
            time = TickLengthMS;
            _lineManager = lineManager;
            _connection = Conn;
        }


        public void Start()
        {
            _startThead = Thread.CurrentThread;

        }
        public void Update()
        {
            try
            {

            _lineManager.SendLinesToServer(this);   
            }catch (Exception e)
            {
                Stop();
            }
        }


        public void Run()
        {
            Start();
            while(!_completed)
            {
                Update();
                Thread.Sleep(time);
            }
        }
        private bool _completed = false;

        public void Stop()
        {
            _connection.Write(new Command(GameCommand.Quit));
            _completed = true;
            _startThead?.Abort();
        }

        internal void SendToServer(List<DrawingTogether.LinePortion> linesToServer)
        {
            _lineManager.AddLinesFromClient( _connection.Request<LineDataTransfer>(new LineDataTransfer(linesToServer)).Portions);
        }
    }
}
