using IrrKlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialABFServer
{
    internal static class GeometryHelper
    {
        public static float DegToRad(int degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public static Vector3D AngleToVector3D (float angle, float radius)
        {
            float x = (float)Math.Cos(angle)*radius;
            float z = (float)Math.Sin(angle)*radius;

            return new Vector3D(x, 0, z);
        }
    }
}
