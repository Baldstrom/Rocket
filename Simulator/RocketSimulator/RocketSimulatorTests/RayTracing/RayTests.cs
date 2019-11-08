/// <summary>
/// Module Name: RayTests.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure for testing Ray class validity
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.RayTracing.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ALPHASim.SimMath;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RocketSimulator.RayTracing;

    [TestClass]
    public class RayTests
    {
        [TestMethod]
        public void RayTestConstruction()
        {
            // Random position vector
            Vector3D<float> testPosition = new Vector3D<float>(0.1f, 0.4f, 0.3f);
            Vector3D<float> testDirection = new Vector3D<float>(0.1f, 0.2f, 0.4f);

            Ray rut = new Ray(testPosition, testDirection);

            for (int i = 0; i < 100; i++)
            {
                testPosition = RandomVectorGenerator(5.0f);
                testDirection = RandomVectorGenerator(4.0f);

                rut = new Ray(testPosition, testDirection);

                Assert.AreEqual(testPosition, rut.Position);
                Assert.AreEqual(testDirection.Normalize(), rut.Direction.Normalize());
            }
        }

        [TestMethod]
        public void RayTestGetAngles()
        {
            // Random position vector
            Vector3D<float> testPosition = new Vector3D<float>(0.1f, 0.4f, 0.3f);
            Vector3D<float> testDirection = new Vector3D<float>(0.1f, 0.2f, 0.4f);

            Ray rut = new Ray(testPosition, testDirection);

            Assert.IsNotNull(rut.Phi);
            Assert.IsNotNull(rut.Theta);

            Vector3D<float> unitVector = new Vector3D<float>(1.0f, 0.0f, 0.0f);
            Vector3D<float> unitDirection = new Vector3D<float>(-1.0f, 0.0f, 0.0f);
            Vector3D<float> zeroAnchor = new Vector3D<float>(0.0f, 0.0f, 0.0f);

            rut = new Ray(unitVector, unitDirection);

            float testAngle = 0.0f;
            float testResolution = 0.2f;
            
            while (testAngle < 2 * Math.PI)
            {
                rut.RayFromAngleOffset(zeroAnchor, testResolution, 0);
                Assert.IsTrue(Equals(rut.Position.X, (float) Math.Cos(testAngle)));
                Assert.IsTrue(Equals(rut.Position.Y, (float) Math.Sin(testAngle)));
                testAngle += testResolution;
            }
        }

        private Vector3D<float> RandomVectorGenerator(float maxElementSize)
        {
            Random r = new Random();

            return new Vector3D<float>((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()).Multiply(maxElementSize);
        }
    }
}