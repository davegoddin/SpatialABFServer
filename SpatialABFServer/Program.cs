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
    static Server server;
    static TrialRoutine[] routines = new TrialRoutine[2];

    static void Main(string[] args)
    {
        // server set-up
        Console.WriteLine("Enter server port number: ");
        int port;

        if (Int32.TryParse(Console.ReadLine(), out port) && TestPortNumber(port))
        {
            server = new Server(port);
            //server.ReceivingData += ClientConnected;
            //server.DataReceived += AccelDataReceived;

            server.PublishIP();
            server.StartServer();

            Thread soundGen = new Thread(soundGenerator.PlayAudioAtCurrentSource);
            soundGen.Start();

            routines[0] = new LocalisationRoutine(soundGenerator);
            routines[1] = new ExerciseRoutine(soundGenerator, server);

            bool exit = false;

            do
            {
                Console.WriteLine("Select routine [0] Localisation [1] Exercise");
                int routineSelect;
                if (Int32.TryParse(Console.ReadLine(), out routineSelect) && routineSelect >= 0 && routineSelect < 2)
                {
                    Console.WriteLine("Selected: " + routineSelect);
                    routines[routineSelect].Run();
                }
                else
                {
                    exit = true;
                }

            } while (!exit);
            

        }
        else
        {
            Console.WriteLine("Error: not a valid entry or port not available");
        }
        
        
        
        
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

    //public static void AccelDataReceived(object sender, AccelerometerReading data)
    //{
    //    soundGenerator.SetAudioSourceLocation(data.X, data.Z);
    //    dataLogger.LogReading(data);
    //}

    //public static void ClientConnected(object sender, EventArgs e)
    //{
        
    //}


}

