using ALPHASim.SimMath;
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
            Run();
        }

        private static void Run()
        {
            RocketController rocket = new RocketController();
            ControlOutput currentControl = new ControlOutput();
            SensorUpdate sensorPackage;
            while (rocket.GetFlightStatus() != FlightState.MISSION_ENDED)
            {
                sensorPackage = GetDynamics(currentControl);
                currentControl = rocket.Tick(sensorPackage);
            }
        }

        private static ControlOutput InitializeControlOutput()
        {
            ControlOutput initialControl = new ControlOutput();
            initialControl.Thrust = new Vector3D<double>(0, 0, 0);
            initialControl.PyroOutputs = 0x00;
            return initialControl;
        }

        private static SensorUpdate InitialSensorReadings()
        {

        }

        private static SensorUpdate GetDynamics(ControlOutput currentControl)
        {
            // Calculate Thrust Vector

            // Calculate Aerodynamic Drag

            // Calculate Acceleration Vectors

            // Calculate Gyro Vectors

            // Calculate Barometric Pressure

            return new SensorUpdate();
        }
    }
}
