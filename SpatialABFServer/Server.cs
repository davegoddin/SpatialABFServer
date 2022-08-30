using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class Server
    {
        private int port;
        private TcpListener tcpListener;
        private Thread listenThread;
        private List<Client> clients = new List<Client>();

        public event EventHandler<AccelerometerReading> DataReceived;
        public event EventHandler ReceivingData;

        public Server(int pPort)
        {
            port = pPort;
        }

        void HandleClientComm(object clientObj)
        {

            Client client = (Client)clientObj;

            ReceivingData?.Invoke(this, EventArgs.Empty);
            Console.WriteLine(client.IP + " has connected");

            byte[] buffer;
            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = client.ClientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    Console.WriteLine(client.IP + " has disconnected");
                    break;
                }
                ASCIIEncoding encoder = new ASCIIEncoding();
                string incomingMsg = encoder.GetString(message, 0, bytesRead);

                JsonNode dataNode = JsonNode.Parse(incomingMsg)!;


                AccelerometerReading accelerometerReading = new AccelerometerReading(dataNode["X"].ToString(), dataNode["Y"].ToString(), dataNode["Z"].ToString(), dataNode["Time"].ToString());

                EventArgs e = new EventArgs();

                DataReceived?.Invoke(this, accelerometerReading);

                client.ClientStream.Flush();
            }
            clients.Remove(client);
            client.TCPClient.Close();

        }

        void ListenForClients()
        {
            Console.WriteLine("Server started...");
            this.tcpListener.Start();

            while (true)
            {
                Client client = new Client(this.tcpListener.AcceptTcpClient());
                clients.Add(client);
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        public void PublishIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                Console.WriteLine("{0} : {1}", ip.AddressFamily.ToString(), ip.ToString());
            }
        }
        public void StartServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }
    }
}
