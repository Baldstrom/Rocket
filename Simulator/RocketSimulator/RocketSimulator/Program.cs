using ALPHASim.SimMath;
using Control;
using Control.FlightStatus;
using RocketSimulator.Parts;
using RocketSimulator.CLI;
using System.Collections.Generic;
using System.Linq;
using RocketSimulator.STL;
using RocketSimulator.RuntimeTests;

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
            List<CLIAction> argParseActions = ArgParser.GetActions(args, out string[] argWarnings, out string[] argErrors);
            Logging.Print(argErrors, Logging.PrintType.ERROR);
            Logging.Print(argWarnings, Logging.PrintType.WARNING);

            bool writebackSTL = false;
            CLIAction writeBackAct = new CLIAction();
            bool writebackExteriorSTL = false;
            CLIAction writeBackExtAct = new CLIAction();

            bool InhibitSTLAnalysis = false;

            if (argErrors.Count() == 0)
            {
                foreach (CLIAction argParseAction in argParseActions)
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
                            STLExtractor primarySTL = new STLExtractor((string)argParseAction.actionValue, STLInfo.STLUnits.Centimeters, InhibitSTLAnalysis);
                            if (primarySTL.IsValid) { simRocket = primarySTL.RocketFromSTL(); }
                            break;
                        case ActionType.ArgDebug:
                            PrintArgs(argParseActions);
                            Logging.Print("");
                            break;
                        case ActionType.Test:
                            string testParams = (string)argParseAction.actionValue;
                            Logging.Print("TEST PROTOCOL INITIATED.");
                            Logging.Print("PARAMETERS: " + testParams);
                            TestProtocol.RunTests(testParams);
                            Logging.Print("TEST CONCLUDED.\n");
                            break;
                        case ActionType.Writeback:
                            writebackSTL = true;
                            writeBackAct = argParseAction;
                            break;
                        case ActionType.WritebackExterior:
                            writebackExteriorSTL = true;
                            writeBackExtAct = argParseAction;
                            break;
                        case ActionType.InhibitFullAnalysis:
                            InhibitSTLAnalysis = true;
                            break;
                        default:
                            break;
                    }
                }

                // STL File writeback
                if (writebackSTL)
                {
                    string filename = "Untitled";
                    if (writeBackAct.actionValue != null && !string.IsNullOrEmpty((string)writeBackAct.actionValue))
                    {
                        filename = (string)writeBackAct.actionValue;
                    }
                    STLInserter.CreateSTLFile(filename, simRocket.Surfaces, STLInfo.STLType.Binary);
                }

                // STL File Exterior writeback
                if (writebackExteriorSTL)
                {
                    string filename = "Untitled";
                    if (writeBackAct.actionValue != null)
                    {
                        filename = (string)writeBackAct.actionValue;
                    }
                    STLInserter.CreateSTLFile(filename, simRocket.ExteriorSurfaces, STLInfo.STLType.Binary);
                }

                Logging.Print("SIMULATION BEGINS.");
                Run();
            }
            Logging.Print("EXITING.");
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
            Vector3D<float> accel = new Vector3D<float>();
            Vector3D<float> velocity = new Vector3D<float>();
            Vector3D<float> dragForce = new Vector3D<float>();
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
                CenterOfMass = new Vector3D<float>     (0.0f, 0.0f, 0.0f),
                CenterOfPressure = new Vector3D<float> (0.0f, 0.0f, 0.0f),
                CenterOfThrust = new Vector3D<float>   (0.0f, 0.0f, 0.0f),
            };
            Rocket rocket = new Rocket(config);
            return rocket;
        }

        private static ControlOutput InitializeControlOutput()
        {
            ControlOutput initialControl = new ControlOutput
            {
                Thrust = new Vector3D<float>(0, 0, 0),
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
                BarometricPressure = 101.325f, // kPa
            };
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static void SetupCSV()
        {

        }

        private static void PrintArgs(List<CLIAction> argActions)
        {
            // TODO: Implement debugging arguments
            foreach (CLIAction act in argActions)
            {

            }
            Logging.Print("ACTION DBG PRINT");
        }
        
    }
}
