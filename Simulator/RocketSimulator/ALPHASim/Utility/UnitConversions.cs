using ALPHASim.SimMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALPHASim.Utility
{
    public static class UnitConversions
    {
        public const float mmHG_Bar_Conversions = 0.0f;

        public static double MmHGtoBar(double value) { return value * mmHG_Bar_Conversions; }
        public static Vector3D<double> MmHGtoBar(Vector3D<double> value) { return value.Multiply(mmHG_Bar_Conversions); }
        
    }
}
