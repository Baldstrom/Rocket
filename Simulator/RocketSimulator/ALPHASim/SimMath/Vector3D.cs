using ALPHASim.Utility;
using System;

namespace ALPHASim.SimMath
{
    public class Vector3D<T> : IComparable
    {
        #region Private Members

        private dynamic Xd { set { this.X = (T)value; } get { return (dynamic)this.X; } }
        private dynamic Yd { set { this.Y = (T)value; } get { return (dynamic)this.Y; } }
        private dynamic Zd { set { this.Z = (T)value; } get { return (dynamic)this.Z; } }

        #endregion

        #region Public Members

        public T X { get; private set; }
        public T Y { get; private set; }
        public T Z { get; private set; }

        #endregion

        public Vector3D()
        {
            // Ensure that data is numerical
            DataMethods.EnsureIsNumeric(typeof(T));
        }

        public Vector3D(T X, T Y, T Z) : base()
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public void Set(T X, T Y, T Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Vector3D<T> Multiply(Vector3D<T> other)
        {
            dynamic nX = other.X * this.Xd;
            dynamic nY = other.Y * this.Yd;
            dynamic nZ = other.Z * this.Zd;
            return new Vector3D<T>((T)nX, (T)nY, (T)nZ);
        }

        public Vector3D<T> Divide(Vector3D<T> other)
        {
            dynamic nX = this.Xd / other.X;
            dynamic nY = this.Yd / other.Y;
            dynamic nZ = this.Zd / other.Z;
            return new Vector3D<T>((T)nX, (T)nY, (T)nZ);
        }

        public Vector3D<T> Add(Vector3D<T> other)
        {
            dynamic nX = other.X + this.Xd;
            dynamic nY = other.Y + this.Yd;
            dynamic nZ = other.Z + this.Zd;
            return new Vector3D<T>((T)nX, (T)nY, (T)nZ);
        }

        public Vector3D<T> Subtract(Vector3D<T> other)
        {
            dynamic nX = this.Xd - other.X;
            dynamic nY = this.Yd - other.Y;
            dynamic nZ = this.Zd - other.Z;
            return new Vector3D<T>((T)nX, (T)nY, (T)nZ);
        }

        public Vector3D<T> Cross(Vector3D<T> other)
        {
            dynamic nX = (this.Yd * other.Z) - (this.Zd * other.Y);
            dynamic nY = (this.Xd * other.Z) - (this.Zd * other.X);
            dynamic nZ = (this.Xd * other.Y) - (this.Yd * other.X);
            return new Vector3D<T>((T)nX, (T)nY, (T)nZ);
        }

        public T Dot(Vector3D<T> other)
        {
            dynamic nX = other.X * this.Xd;
            dynamic nY = other.Y * this.Yd;
            dynamic nZ = other.Z * this.Zd;
            return nX + nY + nZ;
        }

        public Vector3D<T> Normalize()
        {
            dynamic mag = Math.Sqrt((this.Xd * this.Xd) + (this.Yd * this.Yd) + (this.Zd * this.Zd));
            return new Vector3D<T>((T)this.Xd / mag, (T)this.Yd / mag, (T)this.Zd / mag);
        }

        public T Magnitude()
        {
            return Math.Sqrt((this.Xd * this.Xd) + (this.Yd * this.Yd) + (this.Zd * this.Zd));
        }

        /// <summary> Compares vectors by their magnitude. </summary>
        /// <param name="obj"> The vector to compare to. </param>
        /// <returns> this.Magnitude() - other.Magnitude() </returns>
        public int CompareTo(object obj)
        {
            Vector3D<T> other = (Vector3D<T>)obj;
            return (int)((dynamic)Magnitude() - other.Magnitude());
        }

        public static Vector3D<T> operator +(Vector3D<T> a, Vector3D<T> b) { return a.Add(b); }
        public static Vector3D<T> operator -(Vector3D<T> a, Vector3D<T> b) { return a.Subtract(b); }
        public static Vector3D<T> operator *(Vector3D<T> a, Vector3D<T> b) { return a.Multiply(b); }
        public static Vector3D<T> operator /(Vector3D<T> a, Vector3D<T> b) { return a.Divide(b); }
    }
}
