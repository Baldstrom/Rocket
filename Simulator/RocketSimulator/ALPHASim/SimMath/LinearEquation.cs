using ALPHASim.Data;
using ALPHASim.Exceptions;
using ALPHASim.Utility;

namespace ALPHASim.SimMath
{
    public class LinearEquation<T> : Equation<T>
    {
        #region Public Members

        public override Equation<T> Dependent { get { return mDependent; } }

        /// <summary> Expected length of Params </summary>
        public override int ParamsLength { get; } = 1;
        /// <summary> Linear equation slope </summary>
        public T M { get; private set; }
        /// <summary> Linear equation y-intercept. </summary>
        public T B { get; private set; }
        /// <summary> EquationOutput = Gain * OriginalOutput </summary>
        public override T Gain { get { return mGain; } }
        private T mGain;

        #endregion

        private Equation<T> mDependent;

        /// <summary>
        /// Initialize a linear equation in the form
        /// y = input1*x + input2, where x is the input variable
        /// and y is the output variable. [ Slope-Intercept form ]
        /// </summary>
        /// <param name="m"> Slope of linear equation </param>
        /// <param name="b"> y-intercept of linear equation </param>
        public LinearEquation(T m, T b)
        {
            DataMethods.EnsureIsNumeric(typeof(T));
            SetSlopeIntercept(m, b);
            mGain = (T)(dynamic)1;
        }

        /// <summary>
        /// Initialize a linear equation in the form
        /// y - y1 = m * (x - x1) [ Point-Slope form ]
        /// </summary>
        /// <param name="y1"> y-coordinate of Point-Slope form </param>
        /// <param name="x1"> x-coordinate of Point-Slope form </param>
        /// <param name="m"> slope of Point-Slope form </param>
        public LinearEquation(T y1, T x1, T m)
        {
            DataMethods.EnsureIsNumeric(typeof(T));
            SetPointSlope(y1, x1, m);
        }

        /// <summary>
        /// Sets a linear equation in the form
        /// y = input1*x + input2, where x is the input variable
        /// and y is the output variable. [ Slope-Intercept form ]
        /// </summary>
        /// <param name="m"> Slope of linear equation </param>
        /// <param name="b"> y-intercept of linear equation </param>
        public void SetSlopeIntercept(T m, T b)
        {
            this.M = m;
            this.B = b;
        }

        /// <summary>
        /// Sets a linear equation in the form
        /// y - y1 = m * (x - x1) [ Point-Slope form ]
        /// </summary>
        /// <param name="y1"> y-coordinate of Point-Slope form </param>
        /// <param name="x1"> x-coordinate of Point-Slope form </param>
        /// <param name="m"> slope of Point-Slope form </param>
        public void SetPointSlope(T y1, T x1, T m)
        {
            this.M = m;
            this.B = y1 - (dynamic)m * x1;
        }

        public override DataProvider<T>.DataProviderDelegate GetDelegate() { return GetValue; }

        /// <summary> Returns the value from the linear equation given an input for `x` </summary>
        /// <param name="input">`x` in the y = mx + b equation </param>
        /// <returns> The output `y` of the linear equation </returns>
        public override T GetValue(params T[] input)
        {
            if (input.Length != ParamsLength) { throw new InvalidParamsLength(); }
            T aInput = input[0];
            if (Dependent != null) { aInput = Dependent.GetValue(aInput); }
            return Gain * (aInput * (dynamic)this.M + this.B);
        }

        /// <summary> Returns the derivative equation of the equation </summary>
        // TODO: Deal with dependents here.
        public override Equation<T> Derivative()
        {
            return new ConstantEquation<T>(this.M) * Dependent.Derivative();
        }

        public override void SetDependent(Equation<T> dependent) { mDependent = dependent; }

        public override void SetGain(T gain) { mGain = gain; }

        public override object Clone()
        {
            LinearEquation<T> Copy = new LinearEquation<T>(this.M, this.B);
            Copy.SetDependent(Dependent);
            Copy.SetGain(Gain);
            return Copy;
        }
    }
}
