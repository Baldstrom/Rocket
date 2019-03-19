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
        public Vector3D<double> SurfaceArea { get; private set; }
        public Vector3D<double> DragCoefficient { get; private set; }
        public Vector3D<double> Position { get; private set; }
        public List<Vector3D<double>> Vertices { get; private set; }
        public Vector3D<double> Normal { get; private set; }

        public Surface(Vector3D<double> area, Vector3D<double> dragCoefficient)
        {
            this.SurfaceArea = area;
            this.DragCoefficient = dragCoefficient;
        }

        public void SetDragCoefficients(Vector3D<double> drag) { this.DragCoefficient = drag; }
    }
}
