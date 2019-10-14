/// <summary>
/// Module Name: Ray.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure for defining and manipulating rays.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.RayTracing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ALPHASim.SimMath;

    /// <summary>
    /// Defines a Ray object with position and
    /// direction.
    /// </summary>
    public class Ray
    {
        /// <summary> Position of ray in XYZ plane. </summary>
        public Vector3D<float> Position { get; private set; }

        /// <summary> Direction of ray. </summary>
        public Vector3D<float> Direction { get; private set; }

        /// <summary> Angle of position relative to YZ axis. </summary>
        public float Theta
        {
            get
            {
                if (!this.thetaSet) { return this.GetTheta(); }
                else { return this.Theta; }
            }

            set { this.Theta = value; }
        }

        /// <summary> Angle of position relative to XY axis. </summary>
        public float Phi
        {
            get
            {
                if (!this.phiSet) { return this.GetPhi(); }
                else { return this.Phi; }
            }

            set { this.Phi = value; }
        }

        /// <summary> True if theta is set. </summary>
        private bool thetaSet;

        /// <summary> True if phi is set. </summary>
        private bool phiSet;

        /// <summary>
        /// Instantiates a ray object with given position and direction.
        /// Ray object is immutable.
        /// </summary>
        /// <param name="position"> Position of the ray in XYZ space. </param>
        /// <param name="direction"> Direction of the ray in XYZ space. </param>
        public Ray(Vector3D<float> position, Vector3D<float> direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        /// <summary>
        /// Returns a ray with position vector offset from the reference Ray by dtheta and dphi.
        /// The new direction vector will point to the anchor position.
        /// </summary>
        /// <param name="reference"> The ray to reference the change of angles from. </param>
        /// <param name="anchor"> The position that the new Ray's direction vector will point to. </param>
        /// <param name="dtheta"> The change in angle of theta between the reference ray and the new ray. </param>
        /// <param name="dphi"> The change angle of phi between the reference ray and the new ray. </param>
        /// <returns> A ray with a 3D angle offset (theta, phi) from the reference ray. </returns>
        public static Ray RayFromAngleOffset(Ray reference, Vector3D<float> anchor, float dtheta, float dphi)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a ray with position vector offset from this Ray by dtheta and dphi.
        /// The new direction vector will point to the anchor position.
        /// </summary>
        /// <param name="anchor"> The position that the new Ray's direction vector will point to. </param>
        /// <param name="dtheta"> The change in angle of theta between the reference ray and the new ray. </param>
        /// <param name="dphi"> The change angle of phi between the reference ray and the new ray. </param>
        /// <returns> A ray with a 3D angle offset (theta, phi) from this ray. </returns>
        public Ray RayFromAngleOffset(Vector3D<float> anchor, float dtheta, float dphi)
        {
            return Ray.RayFromAngleOffset(this, anchor, dtheta, dphi);
        }

        /// <summary>
        /// Sets and returns the value of theta computed from the direction vector.
        /// </summary>
        /// <returns> The set value of theta. </returns>
        private float GetTheta()
        {
            if (!this.thetaSet)
            {
            }

            this.thetaSet = true;
            return this.Theta;
        }

        /// <summary>
        /// Sets and returns the value of phi computed from the direction vector.
        /// </summary>
        /// <returns> The set value of phi. </returns>
        private float GetPhi()
        {
            if (!this.phiSet)
            {
            }

            this.phiSet = true;
            return this.Phi;
        }
    }
}
