using Control;
using Control.FlightStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RocketController rocket = new RocketController();
            ControlOutput currentControl = new ControlOutput();
            SensorUpdate sensorPackage;
            while(rocket.GetFlightStatus() != FlightState.MISSION_ENDED)
            {
                sensorPackage = GetDynamics(currentControl);
                currentControl = rocket.Tick(sensorPackage);
            }
        }

        private static SensorUpdate GetDynamics(ControlOutput currentControl)
        {
            throw new NotImplementedException();
        }
    }
}
