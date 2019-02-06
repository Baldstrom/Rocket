using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator
{
    public static class ArgParser
    {
        public static List<Action> GetActions(string[] arguments)
        {
            List<Action> actionList = new List<Action>();
            return actionList;
        }
    }

    public struct Action
    {
        ActionType act;
        object actionValue;
    }

    public enum ActionType
    {

    }
}
