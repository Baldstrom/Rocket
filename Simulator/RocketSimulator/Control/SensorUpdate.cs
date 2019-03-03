using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    public struct SensorUpdate
    {
        public double AccelX, AccelY, AccelZ; // m/s/s
        public double RotX, RotY, RotZ; // deg/s
        public double BarometricPressure; // kPa
    }
}
