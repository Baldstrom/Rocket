using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator.STL
{
    public class STLInfo
    {
        public const int STL_BIN_HEADER_LENGTH = 80;
        public const int STL_BIN_FACET_LENGTH = 50;
        public const int STL_BIN_NUM_FACET_LENGTH = 8;
        public const string STL_EXTENSION = ".stl";

        public enum STLType
        {
            Binary,
            ASCII
        }

        public enum STLUnits
        {
            Millimeters,
            Centimeters,
            Micrometers,
            Meters,
            Mils,
            Inches,
        }

    }
}
