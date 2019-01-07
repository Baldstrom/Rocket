using System;
using ALPHASim.SimMath;

namespace ALPHASim.Data
{
    public class LookupDataProvider<T> : DataProvider<T>
    {
        public override DataProviderDelegate Get { get { return mGet; } }
        public override DataProviderParamsDelegate GetParams { get { return mGetParams; } }

        private DataProviderDelegate mGet { get; set; }
        private DataProviderParamsDelegate mGetParams { get; set; }

        public override T GetOutput(params T[] key) { return Get(key); }

        public override T GetOutput() { return GetOutput(GetParams()); }

        public override void SetParamsProvider(DataProviderParamsDelegate del) { mGetParams = del; }

        public override void SetProvider(DataProviderDelegate del) { mGet = del; }

        public override void SetProvider(Equation<T> eq) { mGet = eq.GetDelegate(); }
    }
}
