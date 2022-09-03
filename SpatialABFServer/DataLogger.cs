using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class DataLogger
    {
        List<string>[] _buffers;
        string _outputFile;
        int _bufferSize;
        int _numBuffers;

        const string OUTPUT_DIRECTORY = "D:/SpatialABF/SpatialABFServer/SpatialABFServer/Logs/";

        int activeBuffer = 0;
        

        public DataLogger(int numBuffers, int bufferSize, string outputFile)
        {
            _buffers = new List<string>[numBuffers];
            _numBuffers = Math.Max(numBuffers, 1);
            for (int i = 0; i < _numBuffers; i++)
            {
                _buffers[i] = new List<string>();
            }
            _bufferSize = bufferSize;
            _outputFile = $"{OUTPUT_DIRECTORY}/{outputFile}.csv";

        }

        private async void WriteBufferToFile(List<string> buffer)
        {
            using StreamWriter file = new(_outputFile, append: true);
            foreach (string line in buffer)
            {
                await file.WriteLineAsync(line);
            }
            buffer.Clear();
        }

        public void LogReading(AccelerometerReading reading)
        {
            // write reading to active buffer
            _buffers[activeBuffer].Add(reading.ToCSVRow());
            UpdateActiveBuffer();
            
        }

        public void LogReading(string reading)
        {
            _buffers[activeBuffer].Add(reading);
            UpdateActiveBuffer();
        }

        private void UpdateActiveBuffer()
        {
            // check if buffer full
            if (_buffers[activeBuffer].Count == _bufferSize)
            {
                // write buffer to file
                WriteBufferToFile(_buffers[activeBuffer]);

                // change active to next free buffer
                if (activeBuffer == _numBuffers - 1)
                {
                    activeBuffer = 0;
                }
                else
                {
                    activeBuffer++;
                }
            }
        }

        public void EndLogging()
        {
            WriteBufferToFile(_buffers[activeBuffer]);
            _buffers[activeBuffer].Clear();
            activeBuffer = 0;
        }

    }
}
