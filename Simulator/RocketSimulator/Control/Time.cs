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
        public static float FLIGHT_RESOLUTION // seconds / tick
        {
            get
            {
                if (RES_SET) { return nF_RES; }
                else { return DEFAULT_FLIGHT_RESOLUTION;  }
            }
            set
            {
                if (CheckNewRes(value))
                {
                    nF_RES = value;
                    RES_SET = true;
                }
            }
        } 

        private static float nF_RES;
        private static bool RES_SET;

        private static bool CheckNewRes(float newResolution) { return newResolution > 0.0f; }
    }
}
