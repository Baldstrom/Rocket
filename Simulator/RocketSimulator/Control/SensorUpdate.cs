using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    public struct SensorUpdate
    {
        public float AccelX, AccelY, AccelZ; // m/s/s
        public float RotX, RotY, RotZ; // deg/s
        public float BarometricPressure; // kPa
    }
}
