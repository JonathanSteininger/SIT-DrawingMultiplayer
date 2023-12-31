﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DrawingTogether;
using AppDrawingTogether.Game;
using DrawingTogether.Net;
using AppDrawingTogether.Network.Server;
using AppDrawingTogether.Network.Client;

namespace AppDrawingTogether.Network
{
    internal class LineManager
    {
        public object lockObject = new object();
        public object lockObject2 = new object();
        public List<LinePortion> LinesFromClient = new List<LinePortion>();

        public GameManager _game;
        public LineManager(GameManager game)
        {
            _game = game;
        }

        public void AddLinesFromClient(List<LinePortion> lines)
        {
            lock (lockObject)
            {
                LinesFromClient.AddRange(lines);
            }
        }
        public List<LinePortion> ReadLinesFromClient()
        {
            lock (lockObject)
            {
                List<LinePortion> temp = LinesFromClient;
                LinesFromClient = new List<LinePortion>();
                return temp;
            }
        }
        List<LinePortion> LinesToSend = new List<LinePortion>();
        public void AddAllLinesFromGame(List<LinePortion> lines)
        {
            lock (lockObject2) {
                LinesToSend = lines;
            }
        }

        public void LinesToClients(ClientWorker worker)
        {
            lock (lockObject2)
            {
                worker.SendLinesToClient(LinesToSend);
            }
        }

        List<LinePortion> LinesToServer = new List<LinePortion>();
        public object lockObject3 = new object();
        public void AddLinesToSendToServer(List<LinePortion> lines)
        {
            lock (lockObject3)
            {
                LinesToServer.AddRange(lines);
            }
        }
        public void SendLinesToServer(ClientLocalWorker worker)
        {
            lock (lockObject3)
            {
                worker.SendToServer(LinesToServer);
                LinesToServer = new List<LinePortion>();
            }
        }
        
        
    }
}
