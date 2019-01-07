using ALPHASim.SimMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator.Parts
{
    public class Surface
    {
        public double SurfaceArea { get; private set; }
        public Vector3D<double> DragCoefficient { get; private set; }

        public Surface(double area, Vector3D<double> dragCoefficient)
        {
            this.SurfaceArea = area;
            this.DragCoefficient = dragCoefficient;
        }
    }
}
