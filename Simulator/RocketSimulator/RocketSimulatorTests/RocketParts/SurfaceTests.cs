/// <summary>
/// Module Name: SurfaceTests.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds tests for the Surface object.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.RocketParts.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ALPHASim.SimMath;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RocketSimulator.RocketParts;

    /// <summary>
    /// Contains tests designed to test the validity of the Surface object.
    /// </summary>
    [TestClass]
    public class SurfaceTests
    {
        /// <summary>
        /// Tests validity of AddVertex() function within Surface object.
        /// Very basic confirmation tests.
        /// </summary>
        [TestMethod]
        public void AddVertexTest()
        {
            Surface surfaceUnderTest = new Surface();

            // Assert 0 vertices at beginning and that Vertices is not null.
            Assert.IsNotNull(surfaceUnderTest.Vertices);
            Assert.AreEqual(0, surfaceUnderTest.Vertices.Count);

            // Add a vertex
            surfaceUnderTest.AddVertex(new Vector3D<float>(0.1f, 0.1f, 0.1f));
            Assert.AreEqual(1, surfaceUnderTest.Vertices.Count); // Assert only one vertex added.

            // Add another vertex
            surfaceUnderTest.AddVertex(0.2f, 0.2f, 0.2f);
            Assert.AreEqual(2, surfaceUnderTest.Vertices.Count); // Assert two vertices now

            // Add another vertex
            surfaceUnderTest.AddVertex(0.1f, 0.2f, 0.3f);
            Assert.AreEqual(3, surfaceUnderTest.Vertices.Count);

            // Assert that the vertices contained within the object are equal to the values put in.
            Assert.AreEqual(0.1f, surfaceUnderTest.Vertices[0].X, float.Epsilon);
            Assert.AreEqual(0.1f, surfaceUnderTest.Vertices[0].Y, float.Epsilon);
            Assert.AreEqual(0.1f, surfaceUnderTest.Vertices[0].Z, float.Epsilon);

            Assert.AreEqual(0.2f, surfaceUnderTest.Vertices[1].X, float.Epsilon);
            Assert.AreEqual(0.2f, surfaceUnderTest.Vertices[1].Y, float.Epsilon);
            Assert.AreEqual(0.2f, surfaceUnderTest.Vertices[1].Z, float.Epsilon);

            Assert.AreEqual(0.1f, surfaceUnderTest.Vertices[2].X, float.Epsilon);
            Assert.AreEqual(0.2f, surfaceUnderTest.Vertices[2].Y, float.Epsilon);
            Assert.AreEqual(0.3f, surfaceUnderTest.Vertices[2].Z, float.Epsilon);
        }

        /// <summary>
        /// Tests validity of FindSurfaceArea() function within Surface object.
        /// Very basic confirmation tests.
        /// </summary>
        [TestMethod]
        public void FindSurfaceAreaTest()
        {
            Surface surfaceUnderTest = SurfaceTests.TestUnitSurfaceFactory(4);

            // Check the surface area in 2D
            surfaceUnderTest.FindSurfaceArea(Surface.SurfacePolygonOrder.OneVertex);
            Assert.AreEqual(0.0f, surfaceUnderTest.SurfaceArea, float.Epsilon);

            surfaceUnderTest.FindSurfaceArea(Surface.SurfacePolygonOrder.TwoVertex);
            Assert.AreEqual(0.0f, surfaceUnderTest.SurfaceArea, float.Epsilon);

            Assert.ThrowsException<NotImplementedException>(
                delegate 
                {
                    surfaceUnderTest.FindSurfaceArea(Surface.SurfacePolygonOrder.ThreeVertex);
                });

            Assert.ThrowsException<NotImplementedException>(
                delegate
                {
                    surfaceUnderTest.FindSurfaceArea(Surface.SurfacePolygonOrder.FourVertex);
                });

            Assert.ThrowsException<ArgumentException>(
                delegate
                {
                    // Test some random number as an enumeration
                    surfaceUnderTest.FindSurfaceArea((Surface.SurfacePolygonOrder)0x72);
                });
        }

        /// <summary>
        /// Generates a unit surface with given number of vertices.
        /// </summary>
        /// <param name="vertices"> Number of vertices to add to Surface. </param>
        /// <returns> A surface with given number of vertices. </returns>
        private static Surface TestUnitSurfaceFactory(int vertices = 1)
        {
            // Create surface and populate
            Surface surfaceUnderTest = new Surface();
            for (int i = 0; i < vertices; i++)
            {
                surfaceUnderTest.AddVertex(1.0f, 1.0f, 1.0f);
            }

            return surfaceUnderTest;
        }

        /// <summary>
        /// Generates a surface with vertices corresponding to a unit square.
        /// (All side lengths = 1.0f).
        /// </summary>
        /// <returns> A unit square surface. </returns>
        private static Surface TestUnitSquareFactory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a random valid surface with certain number of vertices.
        /// </summary>
        /// <param name="vertices"> Number of vertices to give surface. </param>
        /// <returns> A new surface object with given number of vertices. </returns>
        private static Surface RandomTestSurfaceFactory(int vertices = 1)
        {
            throw new NotImplementedException();
        }
    }
}