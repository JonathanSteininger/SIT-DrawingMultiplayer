using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DrawingTogether.Net
{
    public class NetworkConnection : IDisposable
    {
        private string _ip;
        private int _port;

        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        /// <summary>
        /// Use to form a connection to a destination ip and port.
        /// </summary>
        /// <param name="ip">destination ip address</param>
        /// <param name="port">destination port number</param>
        public NetworkConnection(string ip, int port)
        {
            _ip = ip;
            _port = port;
            Connect();
        }
        /// <summary>
        /// Used to initialise a connection using an already connected client.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="Exception"></exception>
        public NetworkConnection(TcpClient client)
        {
            if (!client.Connected) throw new Exception("Client Must be connected.");
            _tcpClient = client;
            _stream = client.GetStream();
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
        }
        public bool IsConnected => (_tcpClient == null) ? false : _tcpClient.Connected;
        /// <summary>
        /// Form connection using the stored ip address and port number.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void Connect()
        {
            if (string.IsNullOrEmpty(_ip) || _port == default) throw new Exception("Missing port or ip.");
            if (IsConnected) throw new Exception("Already connected!");
            Connect(new TcpClient(_ip, _port));
            
        }
        /// <summary>
        /// form connection using an already connected client.
        /// </summary>
        /// <param name="client"></param>
        private void Connect(TcpClient client)
        {
            _tcpClient = client;
            _stream = _tcpClient.GetStream();
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
        }
        /// <summary>
        /// Reads json from the network stream and deserializes it into strict chosen type
        /// </summary>
        /// <typeparam name="T">type of object recived from stream</typeparam>
        /// <returns>object from stream</returns>
        public T Read<T>()
        {
            CheckConnected();
            return Deserialize<T>(_reader.ReadString());
        }
        /// <summary>
        /// Reads json from the network stream and deserializes automatically depending on data that was sent
        /// </summary>
        /// <returns>object from stream</returns>
        public object ReadAuto()
        {
            CheckConnected();
            return DeserializeAuto(_reader.ReadString());
        }
        /// <summary>
        /// Writes object to network stream, and then reads stream for response.
        /// </summary>
        /// <param name="ObjectSent">object being sent</param>
        /// <returns>response object</returns>
        public object RequestAuto(object sentObject)
        {
            Write(sentObject);
            return ReadAuto();
        }
        /// <summary>
        /// Converts object to json and sendts it to network stream.
        /// </summary>
        /// <param name="ObjectSent">object being sent to the stream</param>
        public void Write(object ObjectSent)
        {
            CheckConnected();
            _writer.Write(Serialize(ObjectSent));
        }
        /// <summary>
        /// Writes object to network stream, and then reads stream for response.
        /// </summary>
        /// <typeparam name="T">response object type</typeparam>
        /// <param name="ObjectSent">object being sent</param>
        /// <returns>response object</returns>
        public T Request<T>(object ObjectSent)
        {
            Write(ObjectSent);
            return Read<T>();
        }

        private void CheckConnected()
        {
            if (!IsConnected) throw new Exception("Not Connected to server");

        }

        private object DeserializeAuto(string json)
        {
            JObject obj = JObject.Parse(json);
            string Type = (string)obj["Type"];
            switch(Type)
            {
                case nameof(LineDataTransfer):
                    return Deserialize<LineDataTransfer>(json);
                case nameof(PlayerName):
                    return Deserialize<PlayerName>(json);
                case nameof(Command):
                    return Deserialize<Command>(json);

            }
            return null;

        }
        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        private string Serialize(object obj) => JsonConvert.SerializeObject(obj);

        public void Dispose()
        {
            _tcpClient?.Dispose();
            _stream?.Dispose();
            _reader?.Dispose();
            _writer?.Dispose();
            _tcpClient = null;
            _stream = null;
            _reader = null;
            _writer = null;
        }
    }
}
