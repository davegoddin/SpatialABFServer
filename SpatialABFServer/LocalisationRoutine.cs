using IrrKlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class LocalisationRoutine : TrialRoutine
    {
        SoundGenerator _soundGenerator;

        public LocalisationRoutine(SoundGenerator soundGenerator)
        {
            _soundGenerator = soundGenerator;
        }

        public void Run()
        {
            StimulusGenerator stimulusGenerator = new StimulusGenerator();
            // generate angles, evenly divided across quadrants
            Queue<int> testAngles = stimulusGenerator.GenerateTestAngles(3);

            //filename for log file
            Console.WriteLine("Enter filename:");
            string fileName = Console.ReadLine();

            DataLogger logger = new DataLogger(1, 12, fileName);

            _soundGenerator.PlayAudioAtCurrentSource();
            while (testAngles.Count > 0)
            {
                int currentAngle = testAngles.Dequeue();

                Vector3D sourceVector = GeometryHelper.AngleToVector3D(GeometryHelper.DegToRad(currentAngle), 5f);

                _soundGenerator.SetAudioSourceLocation(sourceVector.X, sourceVector.Z);
                _soundGenerator.Unmute();
                Thread.Sleep(5000);
                _soundGenerator.Mute();

                Console.WriteLine($"{currentAngle} - enter measured angle: ");
                string measuredAngle = Console.ReadLine();
                logger.LogReading($"{currentAngle},{measuredAngle}");
            }

            logger.EndLogging();

        }


        

    }
}
