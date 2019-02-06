using System;
using Control.FlightStatus;
using Control.PyroDischarge;
using Control.ThrustVectoring;

namespace Control
{
    public class RocketController
    {
        private PyroController pyroControl;
        private TVCController tvcControl;
        private FlightStatusController flightStatusController;

        public RocketController()
        {
            flightStatusController = new FlightStatusController();
            tvcControl = new TVCController();
            pyroControl = new PyroController(flightStatusController);
        }

        public ControlOutput Tick(SensorUpdate values)
        {
            ControlOutput control = new ControlOutput();
            FlightState state = flightStatusController.Tick(values);
            control.Thrust = tvcControl.Tick(values, state);
            control.PyroOutputs = pyroControl.Tick(values, state);

            return control;
        }

        public FlightState GetFlightStatus() { return flightStatusController.FlightStatus; }

        public void Launch() { flightStatusController.RequestFlightStateChange(FlightState.LAUNCH); }
        public void Abort() { flightStatusController.RequestFlightStateChange(FlightState.ABORT); }
    }

}
