using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator.CLI
{
    public static class ArgParser
    {
        public static List<CLIAction> GetActions(string[] arguments, out string[] warnings, out string[] errors)
        {
            List<string> warn = new List<string>();
            List<string> err = new List<string>();
            List<CLIAction> actionList = new List<CLIAction>();
            for (int i = 0; i < arguments.Length; i++)
            {
                string arg = arguments[i];
                // Check for action
                if (arg.StartsWith("-"))
                {
                    switch(arg.Substring(1))
                    {
                        case "a":
                        case "argDbg":
                            // Expect no parameters
                            if (NumActionTypeInList(actionList, ActionType.ArgDebug) == 0)
                            {
                                actionList.Add(new CLIAction(ActionType.ArgDebug));
                            }
                            break;
                        case "t":
                        case "timescale":
                            // Expect 1 double
                            // Check if timescale action already exists in action set
                            // Only act if it is a new option
                            if (arguments.Count() <= i + 1) { err.Add("ERROR: LACKING TIMESCALE ARGUMENT"); break; }
                            if (NumActionTypeInList(actionList, ActionType.TimeScale) == 0)
                            {
                                // Create new timescale action
                                ActionType thisActType = ActionType.TimeScale;
                                Type thisActValueType = typeof(float);
                                bool success = float.TryParse(arguments[i + 1], out float thisActValue);
                                if (success) { actionList.Add(new CLIAction(thisActType, thisActValue, thisActValueType)); }
                                else { warn.Add("WARNING: COULD NOT PARSE TIMESCALE ARGUMENT AS FLOAINNG POINT VALUE."); }
                            }
                            else
                            {
                                warn.Add("WARNING: DETECTED MULTIPLE TIMESCALE MODIFICATION PARAMETERS. USED FIRST IDENTIFIED VALUE.");
                            }
                            // Increment i because this action takes one timescale
                            // Perhaps only incrememnt i if arguments[i + 1] does not start with '-'?
                            i++;
                            break;
                        case "stl":
                            // Expect 1 filename '..' will indicate relative file
                            // Check if stl action already exists
                            if (NumActionTypeInList(actionList, ActionType.LoadStl) == 0)
                            {
                                // Create STL action and add to actionList
                                if (arguments.Count() <= i + 1) { err.Add("ERROR: LACKING STL FILE PATH PARAMETER"); break; }
                                string filePath = arguments[i + 1];
                                if (filePath.Trim().Equals(string.Empty))
                                {
                                    err.Add("ERROR: NO STL FILE PATH GIVEN");
                                }

                                if (File.Exists(filePath))
                                {
                                    actionList.Add(new CLIAction(ActionType.LoadStl, filePath, typeof(string)));
                                }
                                else
                                {
                                    err.Add("ERROR: GIVEN STL FILE COULD NOT BE FOUND.");
                                }
                            }
                            else
                            {
                                warn.Add("WARNING: DETECTED MULTIPLE STL FILE PARAMETERS. USED FIRST IDENTIFIED VALUE.");
                            }
                            // Increment i because this action takes one timescale
                            // Perhaps only incrememnt i if arguments[i + 1] does not start with '-'?
                            i++;
                            break;
                        case "csv":
                            // No parameters
                            if (NumActionTypeInList(actionList, ActionType.PrintCSVs) == 0)
                            {
                                actionList.Add(new CLIAction(ActionType.PrintCSVs));
                            }
                            break;
                        case "te":
                        case "test":
                            StringBuilder newStr = new StringBuilder();
                            string nxtStr = arguments[i + 1];
                            while (!nxtStr.StartsWith("-"))
                            {
                                newStr.Append(nxtStr);
                                newStr.Append(" ");
                                i++;
                                nxtStr = arguments[i + 1];
                            }
                            actionList.Add(new CLIAction(ActionType.Test, newStr.ToString(), typeof(string)));
                            break;
                        case "wb":
                        case "writeback":
                            if (NumActionTypeInList(actionList, ActionType.PrintCSVs) == 0)
                            {
                                newStr = new StringBuilder();
                                if (arguments.Length > (i + 1))
                                {
                                    nxtStr = arguments[i + 1];
                                    // Determine name
                                    while (!nxtStr.StartsWith("-"))
                                    {
                                        newStr.Append(nxtStr);
                                        newStr.Append(" ");
                                        i++;
                                        nxtStr = arguments[i + 1];
                                    }
                                }
                                actionList.Add(new CLIAction(ActionType.Writeback, newStr.ToString(), typeof(string)));
                            }
                            break;
                        case "wbe":
                        case "extwrite":
                            if (NumActionTypeInList(actionList, ActionType.PrintCSVs) == 0)
                            {
                                newStr = new StringBuilder();
                                if (arguments.Length > (i + 1))
                                {
                                    nxtStr = arguments[i + 1];
                                    // Determine name
                                    while (!nxtStr.StartsWith("-"))
                                    {
                                        newStr.Append(nxtStr);
                                        newStr.Append(" ");
                                        i++;
                                        nxtStr = arguments[i + 1];
                                    }
                                }
                                actionList.Add(new CLIAction(ActionType.WritebackExterior, newStr.ToString(), typeof(string)));
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
            warnings = warn.ToArray();
            errors = err.ToArray();
            return actionList;
        }

        /// <summary>
        /// TODO: Implement O(n) of this
        /// </summary>
        /// <param name="list">List to search</param>
        /// <param name="searchType">Type to search for in list.</param>
        /// <returns>Number of times searchType appears as an action in the list</returns>
        private static int NumActionTypeInList(List<CLIAction> list, ActionType searchType)
        {
            if (list.Count > 0)
            {
                int count = 0;
                foreach (CLIAction action in list)
                {
                    if (action.act.Equals(searchType))
                    {
                        count++;
                    }
                }
                return count;
            }
            else { return 0; }
        }
    }

    public struct CLIAction
    {
        public readonly ActionType act;
        public readonly object actionValue;
        public readonly Type actionValueType;

        public CLIAction(ActionType act, object actionValue, Type actionValueType)
        {
            this.act = act;
            this.actionValue = actionValue;
            this.actionValueType = actionValueType;
        }

        public CLIAction(ActionType act)
        {
            this.act = act;
            actionValue = null;
            actionValueType = null;
        }

    }

    public enum ActionType
    {
        LoadStl,
        PrintCSVs,
        TimeScale,
        ArgDebug,
        Test,
        Writeback,
        WritebackExterior,
    }
}
