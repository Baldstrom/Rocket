using ALPHASim.Data;
using ALPHASim.Exceptions;

namespace ALPHASim.SimMath
{
    public class MultiplicativeEquation<T> : Equation<T>
    {
        public override int ParamsLength { get; } = 1;

        public override Equation<T> Dependent { get { return mDependent; } }
        private Equation<T> mDependent;

        public override T Gain { get { return mGain; } }
        private T mGain;

        private Equation<T> a, b;

        public MultiplicativeEquation(Equation<T> a, Equation<T> b)
        {
            this.a = a;
            this.b = b;
        }

        public override Equation<T> Derivative() { return (a.Derivative() * b) + (a * b.Derivative()); }

        public override DataProvider<T>.DataProviderDelegate GetDelegate() { return GetValue; }

        public override T GetValue(params T[] input)
        {
            if (input.Length != ParamsLength) { throw new InvalidParamsLength(); }
            T aInput = input[0];
            if (Dependent != null) { aInput = Dependent.GetValue(input); }
            return Gain * ((dynamic)a.GetValue(aInput) * b.GetValue(aInput));
        }

        public override void SetDependent(Equation<T> dependent) { mDependent = dependent; }

        public override void SetGain(T gain) { mGain = gain; }

        public override object Clone()
        {
            MultiplicativeEquation<T> Copy = new MultiplicativeEquation<T>(a, b);
            Copy.SetGain(Gain);
            Copy.SetDependent(Dependent);
            return Copy;
        }
    }
}
