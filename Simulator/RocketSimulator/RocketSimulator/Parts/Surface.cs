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
            this.Vertices = new List<Vector3D<double>>();
            this.SurfaceArea = area;
            this.DragCoefficient = dragCoefficient;
        }

        public void SetDragCoefficients(Vector3D<double> drag) { this.DragCoefficient = drag; }
        public void SetSurfaceArea(Vector3D<double> surfaceArea) { this.SurfaceArea = surfaceArea; }
        public void SetPosition(Vector3D<double> position) { this.Position = position; }
        public void AddVertex(Vector3D<double> vertex) { this.Vertices.Add(vertex); }
        public void SetNormal(Vector3D<double> normal) { this.Normal = normal; }

    }
}
