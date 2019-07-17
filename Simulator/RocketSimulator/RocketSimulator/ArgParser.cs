using System;
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
                            ActionType thisActType = ActionType.TimeScale;
                            Type thisActValueType = typeof(float);
                            float thisActValue;
                            bool success = float.TryParse(arguments[i + 1], out thisActValue);
                            if (success) { actionList.Add(new Action(thisActType, thisActValue, thisActValueType)); }
                            else { warn.Add("WARNING: COULD NOT PARSE TIMESCALE ARGUMENT AS FLOAINNG POINT VALUE."); }
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
