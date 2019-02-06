namespace Control.FlightStatus
{
    public class FlightStatusController
    {
        public FlightState FlightStatus { get; private set; }

        public FlightStatusController()
        {
            FlightStatus = FlightState.ON_PAD;
        }

        public FlightState Tick(SensorUpdate sensors)
        {
            FlightStatus += 1;
            return FlightStatus;
        }

        public bool RequestFlightStateChange(FlightState newState)
        {
            FlightStatus = newState;
            return true;
        }

    }
}
