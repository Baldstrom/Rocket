/// <summary>
/// Module Name: Surface.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure defining surfaces.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.RocketParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ALPHASim.SimMath;

    /// <summary>
    /// Contains structure for defining a simulated 
    /// surface on or inside the rocket.
    /// </summary>
    public class Surface
    {
        /// <summary> Normal vector to surface of plane. </summary>
        public Vector3D<float> Normal { get; private set; } 

        /// <summary> List of vertices of the surface. </summary>
        public List<Vector3D<float>> Vertices { get; private set; }

        /// <summary> 
        /// Surface area in user-given units. 
        /// Typically same units as a Rocket STL file.
        /// </summary>
        public float SurfaceArea { get;  private set; }

        /// <summary> Initializes a new instance of the <see cref="Surface"/> class. </summary>
        public Surface()
        {
            this.Vertices = new List<Vector3D<float>>();
        }

        /// <summary> Adds a vertex to the polygon. </summary>
        /// <param name="newVertex"> The vertext to add. </param>
        public void AddVertex(Vector3D<float> newVertex)
        {
            this.Vertices.Add(newVertex);
        }

        /// <summary> Adds a vertex to the polygon. </summary>
        /// <param name="x"> X Coordinate of vertex. </param>
        /// <param name="y"> Y Coordinate of vertex. </param>
        /// <param name="z"> Z Coordinate of vertex. </param>
        public void AddVertex(float x, float y, float z)
        {
            this.AddVertex(new Vector3D<float>(x, y, z));
        }

        /// <summary>
        /// Finds the surface area of the surface polygon given the order of
        /// the polygon. 
        /// </summary>
        /// <param name="polygonOrder"> The order of the polygon. </param>
        public void FindSurfaceArea(SurfacePolygonOrder polygonOrder = SurfacePolygonOrder.ThreeSide)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines orders of polygons for the Surface object.
        /// </summary>
        public enum SurfacePolygonOrder
        {
            OneSide,
            TwoSide,
            ThreeSide,
            FourSide,
        }
    }
}
