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
        public delegate Rocket MakeRocket();

        private static Rocket simRocket;
        private static RocketController rocket;
        private static ControlOutput currentControl;
        private static SensorUpdate sensorPackage;
        private static MakeRocket rocketCreation;

        public static void Main(string[] args)
        {
            rocketCreation = DefaultMakeRocket;
            List<Action> argParseActions = ArgParser.GetActions(args, out string[] argWarnings, out string[] argErrors);
            Logging.Print(argErrors, Logging.PrintType.ERROR);
            Logging.Print(argWarnings, Logging.PrintType.WARNING);
            if (argErrors.Count() == 0)
            {
                foreach (Action argParseAction in argParseActions)
                {
                    // Do actions
                    switch (argParseAction.act)
                    {
                        case ActionType.PrintCSVs:
                            SetupCSV();
                            break;
                        case ActionType.TimeScale:
                            Time.FLIGHT_RESOLUTION = (float)argParseAction.actionValue;
                            break;
                        case ActionType.LoadStl:
                            STLExtractor primarySTL = new STLExtractor((string)argParseAction.actionValue);
                            if (primarySTL.IsValid) { simRocket = primarySTL.RocketFromSTL(); }
                            break;
                        case ActionType.ArgDebug:
                            PrintArgs(argParseActions);
                            break;
                        default:
                            break;
                    }
                }
                Run();
            }
            Logging.Close();
        }

        private static void Run()
        {
            rocket = new RocketController();
            DefaultMakeRocket();
            simRocket = rocketCreation();
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
            Vector3D<double> velocity = new Vector3D<double>();
            Vector3D<double> dragForce = new Vector3D<double>();
            // Compute drag force
            velocity = accel.Multiply(Time.FLIGHT_RESOLUTION);
            // dragForce = Cd * p * v^2 / 2 * A
            dragForce = simRocket.GetDragCoefficients().Multiply(sensorPackage.BarometricPressure);
            // RAY TRACING FOR DRAG ??? 
            dragForce = dragForce * velocity * velocity;// * simRocket.GetSurfaceAreas(); <-- Need to find a way to do this...
            dragForce = dragForce.Divide(2);

            // Set new acceleration vectors
            accel = currentControl.Thrust - dragForce;
            newSense.AccelX = accel.X;
            newSense.AccelY = accel.Y;
            newSense.AccelZ = accel.Z;

            // Calculate Gyro Vectors (no need for this right now)
            newSense.RotX = 0;
            newSense.RotY = 0;
            newSense.RotZ = 0;
            
            // Calculate Barometric Pressure (deltaH?)
            newSense.BarometricPressure = sensorPackage.BarometricPressure + 0.1f;
            return newSense;
        }

        private static Rocket DefaultMakeRocket()
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
            ControlOutput initialControl = new ControlOutput
            {
                Thrust = new Vector3D<double>(0, 0, 0),
                PyroOutputs = 0x00
            };
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
        
        /// <summary>
        /// 
        /// </summary>
        private static void SetupCSV()
        {

        }

        private static void PrintArgs(List<Action> argActions)
        {
            // TODO: Implement debugging arguments
            foreach (Action act in argActions)
            {

            }
            Logging.Print("ACTION DBG PRINT");
        }
        
    }
}
