using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator
{
    public static class ArgParser
    {
        public static List<Action> GetActions(string[] arguments, out string[] warnings, out string[] errors)
        {
            List<string> warn = new List<string>();
            List<string> err = new List<string>();
            List<Action> actionList = new List<Action>();
            for (int i = 0; i < arguments.Length; i++)
            {
                string arg = arguments[i];
                // Check for action
                if (arg.StartsWith("-"))
                {
                    switch(arg.Skip(1).ToString())
                    {
                        case "t":
                        case "timescale":
                            // Expect 1 double
                            // Check if timescale action already exists in action set
                            // Only act if it is a new option
                            if (NumActionTypeInList(actionList, ActionType.TimeScale) == 0)
                            {
                                // Create new timescale action
                                ActionType thisActType = ActionType.TimeScale;
                                Type thisActValueType = typeof(float);
                                float thisActValue;
                                bool success = float.TryParse(arguments[i + 1], out thisActValue);
                                if (success) { actionList.Add(new Action(thisActType, thisActValue, thisActValueType)); }
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
                                Action newAction;
                                string filePath = arguments[i + 1];
                                if (File.Exists(filePath))
                                {
                                    actionList.Add(new Action(ActionType.LoadStl, filePath, typeof(string)));
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
                                actionList.Add(new Action(ActionType.PrintCSVs));
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
        private static int NumActionTypeInList(List<Action> list, ActionType searchType)
        {
            if (list.Count > 0)
            {
                int count = 0;
                foreach (Action action in list)
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

    public struct Action
    {
        public readonly ActionType act;
        public readonly object actionValue;
        public readonly Type actionValueType;

        public Action(ActionType act, object actionValue, Type actionValueType)
        {
            this.act = act;
            this.actionValue = actionValue;
            this.actionValueType = actionValueType;
        }

        public Action(ActionType act)
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

    }
}
