using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALPHASim.SimMath;

namespace RocketSimulator.Environment
{
    /// <summary>
    /// Should be a vector field, some function of position vector and time.
    /// </summary>
    public class Wind
    {
        public Equation<Vector3D<double>> WindField { get; private set; }

        /// <summary>
        /// Creates a Wind Vector field of constant random conditions within the flight specification
        /// </summary>
        public Wind()
        {
            PerlinNoise perlin = new PerlinNoise(MaxEnvironmentalConditions.MaxWindSpeed.Normalize().Magnitude());
            Vector3D<double> perlinVector = new Vector3D<double>(perlin.Noise(), perlin.Noise(), perlin.Noise());
            WindField = new ConstantEquation<Vector3D<double>>(perlinVector);
        }

        /// <summary>
        /// Creates a Wind Vector field of constant conditions
        /// </summary>
        public Wind(Vector3D<double> wind)
        {
            WindField = new ConstantEquation<Vector3D<double>>(wind);
        }

        public Vector3D<double> GetWind(double x, double y, double z)
        {
            return GetWind(new Vector3D<double>(x, y, z));
        }

        public Vector3D<double> GetWind(Vector3D<double> position)
        {
            return WindField.GetValue(position);
        }

    }
}
