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
    using RocketSimulator.RayTracing;

    /// <summary>
    /// Contains structure for defining a simulated 
    /// surface on or inside the rocket.
    /// </summary>
    public class Surface
    {
        /// <summary> Maximum number of vertices possibly contained in a Surface. </summary>
        public const int NUM_VERTICES_MAX = 4;

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
        /// <exception cref="ArgumentException">
        /// If you have added more vertices than the max number of vertices allowable. 
        /// </exception>
        public void AddVertex(Vector3D<float> newVertex)
        {
            this.Vertices.Add(newVertex);
        }

        /// <summary> Adds a vertex to the polygon. </summary>
        /// <param name="x"> X Coordinate of vertex. </param>
        /// <param name="y"> Y Coordinate of vertex. </param>
        /// <param name="z"> Z Coordinate of vertex. </param>
        /// <exception cref="ArgumentException"> 
        /// If you have added more vertices than the max number of vertices allowable. 
        /// </exception>
        public void AddVertex(float x, float y, float z)
        {
            if (this.Vertices.Count < NUM_VERTICES_MAX)
            {
                this.AddVertex(new Vector3D<float>(x, y, z));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Finds the surface area of the surface polygon given the order of
        /// the polygon. 
        /// </summary>
        /// <exception cref="ArgumentException"> If polygonOrder cannot be interpreted. </exception>
        /// <param name="polygonOrder"> The order of the polygon. </param>
        public void FindSurfaceArea(SurfacePolygonOrder polygonOrder = SurfacePolygonOrder.ThreeVertex)
        {
            // Switch on the order of the polygon
            switch (polygonOrder)
            {
                case SurfacePolygonOrder.OneVertex:
                case SurfacePolygonOrder.TwoVertex:
                    // If it doesn't have at least 3 sides, there is no surface area.
                    this.SurfaceArea = 0.0f;
                    break;
                case SurfacePolygonOrder.ThreeVertex:
                    this.SurfaceArea = Surface.FindSurfaceAreaThreeVertex(this);
                    break;
                case SurfacePolygonOrder.FourVertex:
                    this.SurfaceArea = Surface.FindSurfaceAreaFourVertex(this);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Finds the surface area of a surface using the first
        /// three vertices in it's vertex list.
        /// </summary>
        /// <param name="surface"> The surface to calculate surface area of. </param>
        /// <exception cref="ArgumentException"> If surface contains fewer than 3 vertices. </exception>
        /// <returns> Surface area of given surface. </returns>
        private static float FindSurfaceAreaThreeVertex(Surface surface)
        {
            // Check if the vertex count is high enough.
            if (surface.Vertices.Count < 3) { throw new ArgumentException(); }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Find the surface area of a surface using the first
        /// four vertices in it's vertex list.
        /// </summary>
        /// <param name="surface"> The surface to calculate surface area of. </param>
        /// <exception cref="ArgumentException"> If surface contains fewer than 4 vertices. </exception>
        /// <returns> Surface area of given surface. </returns>
        private static float FindSurfaceAreaFourVertex(Surface surface)
        {
            // Check if the vertex count is high enough
            if (surface.Vertices.Count < 4) { throw new ArgumentException(); }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines orders of polygons for the Surface object.
        /// </summary>
        public enum SurfacePolygonOrder
        {
            OneVertex,
            TwoVertex,
            ThreeVertex,
            FourVertex,
        }

        /// <summary>
        /// Determines if a ray collides with this surface
        /// </summary>
        /// <param name="checkRay"> Ray to check. </param>
        /// <returns> Whether or not the ray hits the surface. </returns>
        public bool RayCollides(Ray checkRay)
        {
            throw new NotImplementedException();
        }
    }
}
