using ALPHASim.SimMath;

namespace ALPHASim.Data
{
    public abstract class DataProvider<T>
    {
        public delegate T DataProviderDelegate(params T[] key);
        public delegate T[] DataProviderParamsDelegate();

        public abstract DataProviderDelegate Get { get; }
        public abstract DataProviderParamsDelegate GetParams { get; }

        public abstract T GetOutput();
        public abstract T GetOutput(params T[] key);

        public abstract void SetProvider(DataProviderDelegate del);
        public abstract void SetProvider(Equation<T> eq);

        public abstract void SetParamsProvider(DataProviderParamsDelegate del);
    }
}
