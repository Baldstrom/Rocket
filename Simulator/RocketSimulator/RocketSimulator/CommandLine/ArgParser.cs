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
    /// Standard action delegate for a CLIAction.
    /// </summary>
    /// <param name="action"> Action for delegate to invoke. </param>
    public delegate void CLIDelegatePrototype(CLIAction action);

    /// <summary>
    /// Parses arguments from the user and interprets them as CLIActions.
    /// </summary>
    public class ArgParser
    {
        /// <summary>
        /// Parses input as a string array into CLIAction objects to be invoked.
        /// </summary>
        /// <param name="commandLineInput"> Input CLI arguments from user. </param>
        /// <returns> A list of all CLI actions to be considered during runtime. </returns>
        public List<CLIAction> GetActions(string[] commandLineInput)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes given actions through a Delegate system
        /// </summary>
        /// <param name="actions"></param>
        public void InvokeActions(List<CLIAction> actions)
        {
            CLIDelegates.DoCLIActions(actions);
        }

        /// <summary>
        /// Invokes given actions as command line arguments.
        /// Attempts conversion between CLI arguements and
        /// CLIActions.
        /// </summary>
        /// <param name="commandLineInput"> Input CLI Arguemnts from user. </param>
        public void InvokeActions(string[] commandLineInput)
        {
            this.InvokeActions(this.GetActions(commandLineInput));
        }

        public void AddDelegate(CLIActionType delegateTarget, CLIDelegatePrototype delegateCLI)
        {
            CLIDelegates.AddDelegate(delegateTarget, delegateCLI);
        }
    }
}
