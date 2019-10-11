/// <summary>
/// Module Name: ArgParser.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds primary logic for parsing user arguments.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.CommandLine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parses arguments from the user and interprets them as CLIActions.
    /// </summary>
    public class ArgParser
    {
        /// <summary>
        /// Parses input as a string array into CLIAction objects to be invoked.
        /// </summary>
        /// <param name="commandLineInput"> Input CLI arguments from user. </param>
        /// <returns> A set of all CLI actions to be considered during runtime. </returns>
        public HashSet<CLIAction> GetActions(string[] commandLineInput)
        {
            throw new NotImplementedException();
        }
    }
}
