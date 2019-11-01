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

        /// <summary> Angle of position relative to YZ axis. Radians. </summary>
        public float Theta
        {
            get
            {
                if (!this.thetaSet) { return this.GetTheta(); }
                else { return this.Theta; }
            }

            set { this.Theta = value; }
        }

        /// <summary> Angle of position relative to XY axis. Radians. </summary>
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
        /// Preserves position vector magnitude.
        /// </summary>
        /// <param name="reference"> The ray to reference the change of angles from. </param>
        /// <param name="anchor"> The position that the new Ray's direction vector will point to. </param>
        /// <param name="dtheta"> The change in angle of theta between the reference ray and the new ray. </param>
        /// <param name="dphi"> The change angle of phi between the reference ray and the new ray. </param>
        /// <returns> A ray with a 3D angle offset (theta, phi) from the reference ray. </returns>
        public static Ray RayFromAngleOffset(Ray reference, Vector3D<float> anchor, float dtheta, float dphi)
        {
            // Calculate new position vector.
            Vector3D<float> newDirection;
            Vector3D<float> newPosition = new Vector3D<float>();

            float newPhi = reference.Phi + dphi;
            float newTheta = reference.Theta + dtheta;
            float d = reference.Position.Magnitude() * (float)Math.Cos(newTheta);

            newPosition.X = d * (float)Math.Cos(newPhi);
            newPosition.Y = d * (float)Math.Sin(newPhi);
            newPosition.Z = reference.Position.Magnitude() * (float)Math.Sin(newTheta);

            // Calculate lambda from new position
            float lambda_inv = 1.0f / Ray.DistanceOf(newPosition, anchor);
            // Direction = 1/lambda * A - 1/lambda * position
            newDirection = new Vector3D<float>().Add(newPosition.Multiply(lambda_inv)).Subtract(anchor.Multiply(lambda_inv));

            return new Ray(newPosition, newDirection);
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
                this.Theta = (float)Math.Atan2(this.Position.Z, this.Position.Y);
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
                this.Phi = (float)Math.Atan2(this.Position.Y, this.Position.X);
            }

            this.phiSet = true;
            return this.Phi;
        }

        /// <summary>
        /// Gets the distance between the tips of two vectors.
        /// </summary>
        /// <param name="vector1"> A vector. </param>
        /// <param name="vector2"> A vector. </param>
        /// <returns> Distance between vector1 and vector2. </returns>
        private static float DistanceOf(Vector3D<float> vector1, Vector3D<float> vector2)
        {
            float rootOf = (float)Math.Pow(vector1.X - vector2.X, 2);
            rootOf += (float)Math.Pow(vector1.Y - vector2.Y, 2);
            rootOf += (float)Math.Pow(vector1.Z - vector2.Z, 2);

            return (float)Math.Sqrt(rootOf);
        }

    }
}
