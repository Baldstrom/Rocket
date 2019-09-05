using System;
using ALPHASim.Data;
using ALPHASim.Exceptions;

namespace ALPHASim.SimMath
{
    public class ConstantEquation<T> : Equation<T>
    {
        public override int ParamsLength { get; } = 0;
        public override T Gain { get { return mGain; } }
        public T Value { get; set; }

        public override Equation<T> Dependent { get { return null; } }

        private T mGain;

        public ConstantEquation(T value)
        {
            mGain = (T)(dynamic)1;
            Value = value;
        }

        public override DataProvider<T>.DataProviderDelegate GetDelegate() { return GetValue; }

        public override T GetValue(params T[] input)
        {
            if (input.Length != ParamsLength) { throw new InvalidParamsLength(); }
            return (dynamic)Gain * Value;
        }

        public override Equation<T> Derivative() { return new ConstantEquation<T>(default(T)); }

        public override void SetDependent(Equation<T> dependent) { throw new InvalidOperationException("Cannot set dependent equation to constant function."); }

        public override void SetGain(T gain) { mGain = gain; }

        public override object Clone()
        {
            ConstantEquation<T> Copy = new ConstantEquation<T>(Value);
            Copy.SetGain(Gain);
            Copy.SetDependent(Dependent);
            return Copy;
        }
    }
}
