using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class Client
    {
        TcpClient tcpClient;
        NetworkStream clientStream;
        IPAddress ip;
        Guid clientID;

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            clientStream = tcpClient.GetStream();
            ip = IPAddress.Parse(((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString());
            clientID = Guid.NewGuid();
        }

        private void TimerMessage(string time)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer;
            buffer = encoder.GetBytes(time);
            ClientStream.Write(buffer, 0, buffer.Length);
            ClientStream.Flush();
        }



        public TcpClient TCPClient { get => tcpClient; }
        public NetworkStream ClientStream { get => clientStream; }
        public IPAddress IP { get => ip; }
        public Guid ClientID { get => clientID; }
    }
}
