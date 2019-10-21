/// <summary>
/// Module Name: CLIAction.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds the CLIAction Class
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.CommandLine
{
    using System.Collections.Generic;

    /// <summary>
    /// A CLIAction is an object representing an action to be taken
    /// as a result of a user input into the command line.
    /// </summary>
    public class CLIAction
    {
        /// <summary>
        /// Gets the ActionType.
        /// </summary>
        public CLIActionType ActionType { get; private set; }

        /// <summary>
        /// Gets the ActionModifiers.
        /// </summary>
        public HashSet<CLIActionModifier> ActionModifiers { get; private set; }

        /// <summary> Contains action value. Never used outside get/set of ActionValue. </summary>
        private object _ActionValue;

        /// <summary>
        /// Gets or sets the ActionValue.
        /// Action value if required. Null by default. Non-overwrite-able.
        /// </summary>
        public object ActionValue
        {
            get
            {
                return this._ActionValue;
            }

            set
            {
                if (this._ActionValue == null) 
                { 
                    this._ActionValue = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CLIAction"/> class.
        /// </summary>
        /// <param name="actionType"> Action type for this value. </param>
        public CLIAction(CLIActionType actionType)
        {
            this.ActionValue = null;
            this.ActionModifiers = new HashSet<CLIActionModifier>();
            this.ActionType = actionType;
        }

        /// <summary>
        /// Adds a modifier to the CLIAction.
        /// </summary>
        /// <param name="newModifier">
        /// New Modifier to add to the CLIAction. 
        /// </param>
        /// <returns> 
        /// The <see cref="bool"/> indicates whether or not the action modifier
        /// was added to the CLIAction. 
        /// </returns>
        public bool AddActionModifier(CLIActionModifier newModifier)
        {
            return this.ActionModifiers.Add(newModifier);
        }
    }
}
