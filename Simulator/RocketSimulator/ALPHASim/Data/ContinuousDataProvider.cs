using ALPHASim.SimMath;
using ALPHASim.Utility;
using System;

namespace ALPHASim.Data
{
    public class ContinuousDataProvider<T> : DataProvider<T>
    {
        public override DataProviderDelegate Get { get { return mGet; } }
        public override DataProviderParamsDelegate GetParams { get { return mGetParams; } }

        private DataProviderDelegate mGet;
        private DataProviderParamsDelegate mGetParams;

        public ContinuousDataProvider(Equation<T> equation)
        {
            DataMethods.EnsureIsNumeric(typeof(T));
            SetProvider(equation);
        }

        public ContinuousDataProvider(DataProviderDelegate del)
        {
            DataMethods.EnsureIsNumeric(typeof(T));
            SetProvider(del);
        }

        public override T GetOutput(params T[] key) { return Get(key); }

        public override T GetOutput() { return GetOutput(GetParams()); }

        public override void SetProvider(DataProviderDelegate del) { mGet = del; }
        public override void SetProvider(Equation<T> eq) { mGet = eq.GetValue; }

        public override void SetParamsProvider(DataProviderParamsDelegate del) { mGetParams = del; }
    }
}
