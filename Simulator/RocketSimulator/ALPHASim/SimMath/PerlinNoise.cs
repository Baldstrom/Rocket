using System;

namespace ALPHASim.SimMath
{
    public class PerlinNoise
    {
        public double AllowableDeviation { get; set; }

        private readonly Random RandomElement;
        private double LastOutput;

        public PerlinNoise(double allowableDeviation = 0.01)
        {
            LastOutput = 0;
            AllowableDeviation = allowableDeviation;
            RandomElement = new Random();
        }

        public double Noise(double gain = 1.0)
        {
            double output = LastOutput + AllowableDeviation * (2 * RandomElement.NextDouble() - 1);
            LastOutput = output;
            return gain * output;
        }
    }
}
