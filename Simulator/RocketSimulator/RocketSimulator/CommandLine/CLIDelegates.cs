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
    internal static class CLIDelegates
    {

        /// <summary> Container for ActionType to ActionDelegate. </summary>
        private static Dictionary<CLIActionType, List<CLIDelegatePrototype>> delegates;

        /// <summary>
        /// Initializes CLIDelegates structure.
        /// </summary>
        static CLIDelegates()
        {
            CLIDelegates.delegates = new Dictionary<CLIActionType, List<CLIDelegatePrototype>>();
            CLIDelegates.AddDefaultCLIDelegates();
        }

        /// <summary>
        /// Adds the default CLI Delegates to the CLIDelegates container
        /// </summary>
        private static void AddDefaultCLIDelegates()
        {
            CLIDelegates.AddDelegate(CLIActionType.ArgDebug, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.CSVSetup, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.EnableCSV, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.Help, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.LoadSTL, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.Test, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.TimeScale, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.Writeback, EmptyActionDelegate);
            CLIDelegates.AddDelegate(CLIActionType.WritebackExterior, EmptyActionDelegate);
        }

        #region Default Delegates

        /// <summary>
        /// Action delegate to do absolutely nothing
        /// </summary>
        /// <param name="action"> Action to do nothing with. </param>
        private static void EmptyActionDelegate(CLIAction action) { /* NOP */ }

        #endregion

        /// <summary>
        /// Invokes CLIDeleages for each command line action.
        /// </summary>
        /// <exception cref="ArgumentException"> If CLIAction does not have a delegate. </exception>
        /// <exception cref="Exception"> Internally failed to obtain delegate prototypes. </exception>
        /// <param name="actions"> Actions to invoke. </param>
        internal static void DoCLIActions(List<CLIAction> actions)
        {
            foreach (CLIAction action in actions)
            {
                if (CLIDelegates.delegates.ContainsKey(action.ActionType))
                {
                    // Invoke actions
                    bool success = CLIDelegates.delegates.TryGetValue(action.ActionType, out List<CLIDelegatePrototype> acts);

                    if (success)
                    {
                        foreach(CLIDelegatePrototype actionPrototype in acts)
                        {
                            actionPrototype?.Invoke(action);
                        }
                    } 
                    else { throw new Exception(); }
                }
                else { throw new ArgumentException(); }
            }
        }

        /// <summary>
        /// Adds a delegate to the list of delegates.
        /// </summary>
        /// <exception cref="Exception"> If failed to add delegate prototype to actionTargetElement. </exception>
        /// <param name="actionTarget"> ActionType to target delegate to. </param>
        /// <param name="prototype"> Delegate to call action for when receiving target </param>
        internal static void AddDelegate(CLIActionType actionTarget, CLIDelegatePrototype prototype)
        {
            if (CLIDelegates.delegates.ContainsKey(actionTarget))
            {
                bool success = CLIDelegates.delegates.TryGetValue(actionTarget, out List<CLIDelegatePrototype> thesePrototypes);

                if (success)
                {
                    thesePrototypes.Add(prototype);
                    // TODO: Determine if these lines are necessary
                    // I.E. Does adding above add to the reference within the dictionary?
                    CLIDelegates.delegates.Remove(actionTarget);
                    CLIDelegates.delegates.Add(actionTarget, thesePrototypes);
                }
                else { throw new Exception(); }
            }
            else
            {
                List<CLIDelegatePrototype> newDelList = new List<CLIDelegatePrototype>
                {
                    prototype
                };

                CLIDelegates.delegates.Add(actionTarget, newDelList);
            }
        }
    }
}
