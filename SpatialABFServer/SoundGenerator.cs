using IrrKlang;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class SoundGenerator
    {
        const string AUDIO_SOURCE_FILE = "D:/SpatialABF/SpatialABFServer/SpatialABFServer/wideband-200-1200Hz.wav";
        const float RADIUS = 1f;

        ISoundEngine engine = new ISoundEngine();
        ISound audio;

        Vector3D audioSourceLocation = new Vector3D(0,0,0);

        public SoundGenerator()
        {
            // initialise audio source to 0,0,0 and play looped
            audio = engine.Play3D(AUDIO_SOURCE_FILE, audioSourceLocation.X, audioSourceLocation.Y, audioSourceLocation.Z, true);
            // set listener to 0,0,0 and facing forward (0,0,1)
            engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            audio.MinDistance = 1f;
            Mute();
        }

        public void SetAudioSourceLocation(float x, float z)
        {
            
            audioSourceLocation = new Vector3D(-RADIUS*x, 0, RADIUS*z);
            //Console.WriteLine(audioSourceLocation);
        }

        public void PlayAudioAtCurrentSource()
        {
            while (true)
            {
                audio.Position = audioSourceLocation;
                Thread.Sleep(100);
            }
        }

        public void Mute()
        {
            audio.Volume = 0f;
        }

        public void Unmute()
        {
            audio.Volume = 1f;
        }
        
        public void Stop()
        {
            audio.Stop();
        }

        public void SetVolume(float volume)
        {
            audio.Volume = volume;
        }
    }
}
