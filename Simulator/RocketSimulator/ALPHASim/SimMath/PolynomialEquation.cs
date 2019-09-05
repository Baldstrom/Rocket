using System;
using ALPHASim.Data;
using ALPHASim.Exceptions;

namespace ALPHASim.SimMath
{
    public class PolynomialEquation<T> : Equation<T>
    {
        public override int ParamsLength { get; } = 1;
        public override Equation<T> Dependent { get { return mDependent; } }
        public override T Gain { get { return mGain; } }

        public T[] Coefficients { get; }
        public int Order { get { return Coefficients.Length; } }
        
        private Equation<T> mDependent;
        private T mGain;

        public PolynomialEquation(params T[] Coefficients)
        {
            mGain = (T)(dynamic)1;
            this.Coefficients = Coefficients;
        }

        public override T GetValue(params T[] input)
        {
            if (input.Length != ParamsLength) { throw new InvalidParamsLength(); }
            T value = input[0];
            if (Dependent != null) { value = Dependent.GetValue(value); }
            return Gain * (dynamic)ComputeOutput(value);
        }

        private T ComputeOutput(T input)
        {
            T output = default(T);
            for (int i = 0; i < Order; i++)
            {
                output += ((dynamic)Coefficients[i] * Math.Pow((double)(dynamic)input, i));
            }
            return Gain * (dynamic)output;
        }

        public override void SetDependent(Equation<T> dependent) { mDependent = dependent; }

        public override DataProvider<T>.DataProviderDelegate GetDelegate() { return GetValue; }

        public override void SetGain(T gain) { mGain = gain; }

        public override Equation<T> Derivative()
        {
            T[] newCoefficients = new T[Order - 1];
            for(int i = Order; i > 0; i--) { newCoefficients[Order - i] = Coefficients[Order - i] * (dynamic)i; }
            PolynomialEquation<T> newPoly = new PolynomialEquation<T>(newCoefficients);
            newPoly.SetGain(Gain);
            newPoly.SetDependent(Dependent);
            return newPoly * Dependent.Derivative();
        }

        public override object Clone()
        {
            PolynomialEquation<T> Copy = new PolynomialEquation<T>(Coefficients);
            Copy.SetGain(Gain);
            Copy.SetDependent(Dependent);
            return Copy;
        }
    }
}
