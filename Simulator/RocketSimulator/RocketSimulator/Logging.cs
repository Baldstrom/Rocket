using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator
{
    public static class Logging
    {
        public enum PrintType
        {
            NORMAL,
            ERROR,
            WARNING
        }

        private static PrintType CurrentConsoleConditions;

        public static void Print(string[] list, PrintType type = PrintType.NORMAL)
        {
            if (CurrentConsoleConditions != type)
            {
                switch(type)
                {
                    case PrintType.NORMAL:
                        SetConsoleNormal();
                        break;
                    case PrintType.WARNING:
                        SetConsoleWarning();
                        break;
                    case PrintType.ERROR:
                        SetConsoleError();
                        break;
                    default:
                        break;
                }
            }

            foreach(string println in list)
            {
                Console.WriteLine(println);
            }
        }

        private static void SetConsoleNormal()
        {

            CurrentConsoleConditions = PrintType.NORMAL;
        }

        private static void SetConsoleWarning()
        {

            CurrentConsoleConditions = PrintType.WARNING;
        }

        private static void SetConsoleError()
        {

            CurrentConsoleConditions = PrintType.ERROR;
        }
    }
}
