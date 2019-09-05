using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control.FlightStatus
{
    public enum FlightState
    {
        ON_PAD,
        LAUNCH,
        ASCENT_PHASE,
        DESCENT_PHASE,
        ABORT,
        MISSION_ENDED,
    }
}
