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
            ControlOutput currentControl = InitializeControlOutput();
            SensorUpdate sensorPackage = InitialSensorReadings();
            while (rocket.GetFlightStatus() != FlightState.MISSION_ENDED)
            {
                sensorPackage = GetDynamics(currentControl);
                currentControl = rocket.Tick(sensorPackage);
            }
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

        private static ControlOutput InitializeControlOutput()
        {
            ControlOutput initialControl = new ControlOutput();
            initialControl.Thrust = new Vector3D<double>(0, 0, 0);
            initialControl.PyroOutputs = 0x00;
            return initialControl;
        }

        private static SensorUpdate InitialSensorReadings()
        {
            return new SensorUpdate() 
            {
                AccelX = 0, // m/s/s
                AccelY = 0, // m/s/s
                AccelZ = 0, // m/s/s
                RotX = 0, // deg/S
                RotY = 0, // deg/S
                RotZ = 0, // deg/S
                BarometricPressure = 101.325, // kPa
            };
        }
    }
}
