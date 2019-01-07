using ALPHASim.Data;
using System;

namespace ALPHASim.SimMath
{
   public abstract class Equation<T> : ICloneable
    {
        public abstract int ParamsLength { get; }
        public abstract Equation<T> Dependent { get; }
        public abstract DataProvider<T>.DataProviderDelegate GetDelegate();
        public abstract T GetValue(params T[] input);
        public abstract Equation<T> Derivative();
        public abstract void SetDependent(Equation<T> dependent);
        public abstract T Gain { get; }
        public abstract void SetGain(T gain);
        public abstract object Clone();

        public static Equation<T> operator +(Equation<T> a, Equation<T> b) { return new AdditiveEquation<T>(a, b); }

        public static Equation<T> operator *(Equation<T> a, Equation<T> b) { return new MultiplicativeEquation<T>(a, b); }
    }
}
