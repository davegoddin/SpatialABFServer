using CSCore.SoundOut;
using IrrKlang;
using SpatialABFServer;
using System.Net.NetworkInformation;
using VisioForge.Libs.NAudio.Wave;

namespace SpatialABFServer;

class Program
{   

    //static generator and loggers
    static SoundGenerator soundGenerator = new SoundGenerator();
    static DataLogger dataLogger;

    static void Main(string[] args)
    {
        // server set-up
        Console.WriteLine("Enter server port number: ");
        int port;

        if (Int32.TryParse(Console.ReadLine(), out port) && TestPortNumber(port))
        {
            Server server = new Server(port);
            server.ReceivingData += ClientConnected;
            server.DataReceived += AccelDataReceived;

            server.PublishIP();
            server.StartServer();
        }
        else
        {
            Console.WriteLine("Error: not a valid entry or port not available");
        }

                
        string fileName = "test";
        dataLogger = new DataLogger(3, 50, fileName);

        
        
        
        
    }

    static bool TestPortNumber(int pPort)
    {
        bool isAvailable = true;
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
        foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
        {
            if (tcpi.LocalEndPoint.Port == pPort)
            {
                isAvailable = false;
                break;
            }
        }
        return isAvailable;
    }

    public static void AccelDataReceived(object sender, AccelerometerReading data)
    {
        soundGenerator.SetAudioSourceLocation(data.X, data.Z);
        dataLogger.LogReading(data);
    }

    public static void ClientConnected(object sender, EventArgs e)
    {
        Thread soundGen = new Thread(soundGenerator.PlayAudioAtCurrentSource);
        soundGen.Start();
    }

    public static void SetMode(int mode)
    {

    }


}

