using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal class AccelerometerReading
    {
        public float X;
        public float Y;
        public float Z;
        public long Time;

        public AccelerometerReading(string x, string y, string z, string time)
        {
            X = float.Parse(x);
            Y = float.Parse(y);
            Z = float.Parse(z);
            Time = long.Parse(time);
        }

        public string ToCSVRow()
        {
            return $"{X},{Y},{Z},{Time}";
        }
    }
}
