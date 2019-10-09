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
        public Vector3D<float> SurfaceArea { get; private set; }
        public Vector3D<float> DragCoefficient { get; private set; }
        public Vector3D<float> Position { get; private set; }
        public List<Vector3D<float>> Vertices { get; private set; }
        public Vector3D<float> Normal { get; private set; }
        public bool IsExterior { get; set; }

        public Surface(Vector3D<float> area, Vector3D<float> dragCoefficient) : this()
        {
            this.SurfaceArea = area;
            this.DragCoefficient = dragCoefficient;
        }

        public Surface()
        {
            this.Vertices = new List<Vector3D<float>>();
            this.DragCoefficient = new Vector3D<float>();
            this.Position = new Vector3D<float>();
            this.Normal = new Vector3D<float>();
            this.SurfaceArea = new Vector3D<float>();
        }

        public void SetDragCoefficients(Vector3D<float> drag) { this.DragCoefficient = drag; }
        public void SetSurfaceArea(Vector3D<float> surfaceArea) { this.SurfaceArea = surfaceArea; }
        public void SetPosition(Vector3D<float> position) { this.Position = position; }
        public void AddVertex(Vector3D<float> vertex) { this.Vertices.Add(vertex); }
        public void SetNormal(Vector3D<float> normal) { this.Normal = normal; }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
            // Tentative code
            /*
            byte[] result = new byte[48];

            byte[] normal = new byte[12];
            byte[] v1 = new byte[12];
            byte[] v2 = new byte[12];
            byte[] v3 = new byte[12];

            float[] normalFloat = new float[3];
            float[] v1Float = new float[3];
            float[] v2Float = new float[3];
            float[] v3Float = new float[3];

            normalFloat[0] = (float)this.Normal.X;
            normalFloat[1] = (float)this.Normal.Y;
            normalFloat[2] = (float)this.Normal.Z;
            Buffer.BlockCopy(normalFloat, 0, result, 0, 12);

            v1Float[0] = (float)this.Vertices[0].X;
            v1Float[1] = (float)this.Vertices[0].Y;
            v1Float[2] = (float)this.Vertices[0].Z;
            Buffer.BlockCopy(v1Float, 0, result, 12, 12);

            v2Float[0] = (float)this.Vertices[1].X;
            v2Float[1] = (float)this.Vertices[1].Y;
            v2Float[2] = (float)this.Vertices[1].Z;
            Buffer.BlockCopy(v2Float, 0, result, 24, 12);

            v3Float[0] = (float)this.Vertices[2].X;
            v3Float[1] = (float)this.Vertices[2].Y;
            v3Float[2] = (float)this.Vertices[2].Z;
            Buffer.BlockCopy(v3Float, 0, result, 36, 12);

            return result;
            */
        }

    }
}
