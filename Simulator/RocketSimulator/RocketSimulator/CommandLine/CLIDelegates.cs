/// <summary>
/// Module Name: CLIDelegates.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds delegate functions for all CLIActionType arguments.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains prototype for a CLIAction Delegate as well
    /// as all of the function delegates for the CLIActionTypes.
    /// </summary>
    public static class CLIDelegates
    {
        /// <summary>
        /// Standard action delegate for a CLIAction.
        /// </summary>
        /// <param name="action"> Action for delegate to invoke. </param>
        public delegate void CLIDelegatePrototype(CLIAction action);
    }
}
