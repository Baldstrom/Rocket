using ALPHASim.Utility;
using System;

namespace ALPHASim.SimMath
{
    public class Vector2D<T> : IComparable
    {
        #region Private Members

        private dynamic Xd { set { this.X = (T)value; } get { return (dynamic)this.X; } }
        private dynamic Yd { set { this.Y = (T)value; } get { return (dynamic)this.Y; } }

        #endregion

        #region Public Members

        public T X { get; set; }
        public T Y { get; set; }
        
        #endregion

        public Vector2D()
        {
            // Ensure that data is numerical
            DataMethods.EnsureIsNumeric(typeof(T));
        }

        public Vector2D(T X, T Y) : base()
        {
            this.X = X;
            this.Y = Y;
        }

        public void Set(T X, T Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2D<T> Multiply(Vector2D<T> other)
        {
            return new Vector2D<T>((T)(this.Xd * (dynamic)other.X), (T)(this.Yd * (dynamic)other.Y));
        }

        public Vector2D<T> Divide(Vector2D<T> other)
        {
            return new Vector2D<T>((T)(this.Xd / (dynamic)other.X), (T)(this.Yd / (dynamic)other.Y));
        }

        public Vector2D<T> Add(Vector2D<T> other)
        {
            return new Vector2D<T>((T)(this.Xd + (dynamic)other.X), (T)(this.Yd + (dynamic)other.Y));
        }

        public Vector2D<T> Subtract(Vector2D<T> other)
        {
            return new Vector2D<T>((T)(this.Xd - (dynamic)other.X), (T)(this.Yd - (dynamic)other.Y));
        }

        public T Dot(Vector2D<T> other)
        {
            return (T)((this.Xd * (dynamic)other.X) + (this.Yd * (dynamic)other.Y));
        }

        public T Magnitude() { return Math.Sqrt((this.Xd * this.Xd) + (this.Yd * this.Yd)); }

        public Vector2D<T> Normalize()
        {
            dynamic mag = Math.Sqrt((this.Xd * this.Xd) + (this.Yd * this.Yd));
            return new Vector2D<T>((T)(this.Xd / mag), (T)(this.Yd / mag));
        }

        public Vector3D<T> Extend() { return new Vector3D<T>(X, Y, default(T)); }

        /// <summary> Compares vectors by their magnitude. </summary>
        /// <param name="obj"> The vector to compare to. </param>
        /// <returns> this.Magnitude() - other.Magnitude() </returns>
        public int CompareTo(object obj)
        {
            Vector2D<T> other = (Vector2D<T>)obj;
            return (int)((dynamic)Magnitude() - other.Magnitude());
        }

        public static Vector2D<T> operator +(Vector2D<T> a, Vector2D<T> b) { return a.Add(b); }
        public static Vector2D<T> operator -(Vector2D<T> a, Vector2D<T> b) { return a.Subtract(b); }
        public static Vector2D<T> operator *(Vector2D<T> a, Vector2D<T> b) { return a.Multiply(b); }
        public static Vector2D<T> operator /(Vector2D<T> a, Vector2D<T> b) { return a.Divide(b); }
    }
}
