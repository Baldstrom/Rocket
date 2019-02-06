using Control.FlightStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control.PyroDischarge
{
    public class PyroController
    {
        public byte Pyros { get; private set; }
        public FlightState lastState;
        public FlightStatusController flightStatusController;

        public PyroController(FlightStatusController controller)
        {
            Pyros = 0x00;
            flightStatusController = controller;
        }

        public byte Tick(SensorUpdate sensors, FlightState state)
        {
            if (state != lastState)
            {
                // Trigger ignition with state change
                if (state == FlightState.LAUNCH)
                {
                    flightStatusController.RequestFlightStateChange(FlightState.ASCENT_PHASE);
                    lastState = state;
                    return Ignition(0x01, true);
                }
            }
            switch (state)
            {
                case FlightState.DESCENT_PHASE:
                    return HandleDescent(sensors);
                case FlightState.ASCENT_PHASE:
                case FlightState.LAUNCH:
                case FlightState.MISSION_ENDED:
                case FlightState.ON_PAD:
                default:
                    return Pyros;
            }

        }

        public byte HandleDescent(SensorUpdate sensors)
        {
            return 0x00;
        }

        public byte Ignition(byte stage, bool reset = false)
        {
            if (reset)
            {
                Reset();
                return stage;
            }
            else
            {
                Pyros |= stage;
                return Pyros;
            }
        }

        public byte DeployChute(bool reset = false)
        { 
            if (reset)
            {
                Reset();
                return 0x80;
            }
            else
            {
                Pyros |= 0x80;
                return Pyros;
            }
        }

        public void Reset() { Pyros = 0x00; }
    }
}
