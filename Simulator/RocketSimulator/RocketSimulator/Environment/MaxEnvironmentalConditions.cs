using ALPHASim.SimMath;

namespace RocketSimulator.Environment {
    public static class MaxEnvironmentalConditions
    {
        // DEFAULTS
        public static Vector3D<double> DefaultMaxWindSpeed { get; private set; }
        
        // WIND SPEED
        private static Vector3D<double> tMaxWindSpeed;
        private static bool MaxWindSpeedSet;
        public static Vector3D<double> MaxWindSpeed 
        { 
            get 
            { 
                if (MaxWindSpeedSet) { return tMaxWindSpeed; } 
                else { return DefaultMaxWindSpeed; } 
            } 
            set 
            { 
                tMaxWindSpeed = value;
                MaxWindSpeedSet = true;
            } 
        }

        static MaxEnvironmentalConditions()
        {
            DefaultMaxWindSpeed = new Vector3D<double>(1.0, 1.0, 1.0);
        }

    }

}