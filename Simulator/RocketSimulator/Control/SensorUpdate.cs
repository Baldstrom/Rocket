using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    public struct SensorUpdate
    {
        public double AccelX, AccelY, AccelZ;
        public double RotX, RotY, RotZ;
        public double BarometricPressure;
    }
}
