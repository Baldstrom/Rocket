using ALPHASim.SimMath;
using Control;
using Control.FlightStatus;
using RocketSimulator.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator
{
    public class Program
    {
        private static Rocket simRocket;
        private static RocketController rocket;
        private static ControlOutput currentControl;
        private static SensorUpdate sensorPackage;

        public static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            rocket = new RocketController();
            simRocket = MakeRocket();
            currentControl = InitializeControlOutput();
            sensorPackage = InitialSensorReadings();
            while (rocket.GetFlightStatus() != FlightState.MISSION_ENDED)
            {
                sensorPackage = GetDynamics();
                currentControl = rocket.Tick(sensorPackage);
            }
        }

        private static SensorUpdate GetDynamics()
        {
            SensorUpdate newSense = new SensorUpdate();
            // Calculate Acceleration Vectors
            Vector3D<double> accel = new Vector3D<double>();
            Vector3D<double> dragForce = new Vector3D<double>();
            
            accel = currentControl.Thrust - dragForce;
            newSense.AccelX = accel.X;
            newSense.AccelY = accel.Y;
            newSense.AccelZ = accel.Z;
            // Calculate Gyro Vectors
            newSense.RotX = 0;
            newSense.RotY = 0;
            newSense.RotZ = 0;
            // Calculate Barometric Pressure
            newSense.BarometricPressure = sensorPackage.BarometricPressure + 0.1f;
            return newSense;
        }

        private static Rocket MakeRocket()
        {
            Rocket.RocketConfiguration config = new Rocket.RocketConfiguration() 
            {
                CenterOfMass = new Vector3D<double>     (0.0f, 0.0f, 0.0f),
                CenterOfPressure = new Vector3D<double> (0.0f, 0.0f, 0.0f),
                CenterOfThrust = new Vector3D<double>   (0.0f, 0.0f, 0.0f),
            };
            Rocket rocket = new Rocket(config);
            return rocket;
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
