using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    public class Time
    {
        public const float DEFAULT_FLIGHT_RESOLUTION = 1.0f;
        public static float FLIGHT_RESOLUTION; // seconds / tick

        static Time()
        {
            if (FLIGHT_RESOLUTION <= 0.0f) { FLIGHT_RESOLUTION = DEFAULT_FLIGHT_RESOLUTION; }
        }

        public static void SetResolution(float newResolution) { FLIGHT_RESOLUTION = newResolution; }
    }
}
