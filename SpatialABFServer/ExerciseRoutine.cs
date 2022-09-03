using IrrKlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class ExerciseRoutine : TrialRoutine
    {
        SoundGenerator _soundGenerator;
        Server _server;

        DataLogger logger;

        bool recentre = false;
        bool trialActive = false;

        public ExerciseRoutine(SoundGenerator soundGenerator, Server server)
        {
            _soundGenerator = soundGenerator;
            _server = server;
            _server.DataReceived += OnAccelDataReceived;
        }

        public void Run()
        {
            Console.WriteLine("Started Exercise Routine");
            StimulusGenerator stimulusGenerator = new StimulusGenerator();
            // generate angles, evenly divided across quadrants
            Queue<int> testAngles = stimulusGenerator.GenerateTestAngles(3);

            //filename for log file
            Console.WriteLine("Enter filename:");
            string fileName = Console.ReadLine();

            logger = new DataLogger(3, 30, fileName);
            while (testAngles.Count > 0)
            {
                
                int currentAngle = testAngles.Dequeue();
                int stimulusNumber = 12 - testAngles.Count;
                Console.WriteLine($"Stimulus {stimulusNumber} of 12");
                logger.LogReading($"Stimulus {stimulusNumber} - {currentAngle} - target");
                Vector3D sourceVector = GeometryHelper.AngleToVector3D(GeometryHelper.DegToRad(currentAngle+90), 5f);
                _soundGenerator.SetAudioSourceLocation(sourceVector.X, sourceVector.Z);
                _soundGenerator.Unmute();
                trialActive = true;
                recentre = false;
                Console.WriteLine("Press any key to start recentring");
                Console.ReadKey();
                recentre = true;
                logger.LogReading($"Stimulus {stimulusNumber} - {currentAngle} - recentre");
                Console.WriteLine("Press any key to end recentring");
                Console.ReadKey();

                trialActive = false;
                recentre = false;
                _soundGenerator.Mute();

                Console.WriteLine($"\nPress any key to continue");
                Console.ReadKey();
            }

            logger.EndLogging();

        }

        private void OnAccelDataReceived(object sender, AccelerometerReading data)
        {

            if (recentre)
            {
                _soundGenerator.SetAudioSourceLocation(-data.X, -data.Z);
                float yMagnitude = Math.Min(Math.Max(data.Y, 5f),10f);
                float volume = (2f * (10f - yMagnitude))/10;
                _soundGenerator.SetVolume(volume);

            }
            if (trialActive)
            {
                logger.LogReading(data);
            }
            
        }
    }
}
