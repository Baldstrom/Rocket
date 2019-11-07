/// <summary>
/// Module Name: ExteriorSurfaceAnalyzer.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure for determining the exterior surfaces
/// of a rocket
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.RayTracing
{
    using ALPHASim.SimMath;
    using RocketSimulator.RocketParts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains structure to determine exterior surfaces and
    /// their properties.
    /// </summary>
    public class ExteriorSurfaceAnalyzer
    {
        /// <summary>
        /// Angle Resolution of analysis in radians. Smaller, the more resolution in 
        /// the analysis.
        /// </summary>
        public float AngleResolution;

        /// <summary>
        /// Same as AngleResolution property, but accessible in degrees.
        /// </summary>
        public float AngleResolutionDegrees
        {
            get { return AngleResolution * 180.0f / (float) Math.PI; }
            set { AngleResolution = value * (float) Math.PI / 180.0f; }
        }

        /// <summary>
        /// Delegate for recieving analysis results.
        /// </summary>
        /// <param name="exteriorSurfaces"> Surfaces that are exterior. </param>
        public delegate void Analysis(List<Surface> exteriorSurfaces);

        /// <summary>
        /// Delegate for receiving a change in process percentage.
        /// </summary>
        /// <param name="percentComplete"> Percent complete. </param>
        public delegate void ProgressChanged(double percentComplete);

        /// <summary>
        /// Event called when surface analysis completed.
        /// </summary>
        public event Analysis AnalysisCompleted;

        /// <summary>
        /// Event called when surface analysis progress changes.
        /// </summary>
        public event ProgressChanged ProgressMade;

        /// <summary> Surfaces for analysis. </summary>
        private readonly List<Surface> Surfaces;

        /// <summary>
        /// Initializes a new instance of <see cref="ExteriorSurfaceAnalyzer"/> class.
        /// </summary>
        /// <param name="rocket"> Rocket to analyze exterior surfaces of. </param>
        public ExteriorSurfaceAnalyzer(List<Surface> surfaces)
        {
            // Ensure acting on a new list. To avoid needing lock.
            this.Surfaces = new List<Surface>(surfaces);
        }

        /// <summary>
        /// Runs an analysis of an STL file.
        /// Designed to run in thread.
        /// </summary>
        public void RunAnalysis()
        {
            float currentPhi = 0.0f;
            float currentTheta = 0.0f;
            Vector3D<float> initialDirectionVector;
            Vector3D<float> initialPositionVector;
            Vector3D<float> anchor;
            List<Surface> exteriorSurfaces;
            Queue<Surface> helper;

            exteriorSurfaces = new List<Surface>();
            helper = new Queue<Surface>(this.Surfaces);
            anchor = new Vector3D<float>(0.0f, 0.0f, 0.0f);
            initialDirectionVector = new Vector3D<float>(-1.0f, 0.0f, 0.0f);
            initialPositionVector = new Vector3D<float>(1.0f, 0.0f, 0.0f);
            Ray testRay = new Ray(initialPositionVector, initialDirectionVector);

            while (currentPhi < (2 * Math.PI))
            {
                while (currentTheta < (2 * Math.PI))
                {
                    int queueCount = helper.Count;
                    if (queueCount == 0) { break; }
                    for (int i = 0; i < queueCount; i++)
                    {
                        Surface testSurface = helper.Dequeue();
                        if (!testSurface.RayCollides(testRay))
                        {
                            helper.Enqueue(testSurface);
                        }
                        else
                        {
                            exteriorSurfaces.Add(testSurface);
                        }
                    }
                    currentTheta += this.AngleResolution;
                    testRay = testRay.RayFromAngleOffset(anchor, this.AngleResolution, 0.0f);
                }
                currentPhi += this.AngleResolution;
                testRay = testRay.RayFromAngleOffset(anchor, 0.0f, this.AngleResolution);
                ProgressMade?.Invoke(currentPhi / (2 * Math.PI));
            }
            AnalysisCompleted?.Invoke(exteriorSurfaces);
        }
    }
}
