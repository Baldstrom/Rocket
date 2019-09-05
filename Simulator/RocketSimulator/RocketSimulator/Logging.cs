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

        #region CSV Fields
        public static bool IsCSVSetup { get; private set; }
        private static bool tPrintCSVs;
        public static bool PrintCSVs
        {
            get { return tPrintCSVs; }
            set
            {
                if (value && !IsCSVSetup)
                {
                    PrintErr("ERROR: IGNORED REQUEST TO PRINT TO CSV. MUST SETUP CSV FIRST");
                }
                else { tPrintCSVs = value; }
            }
        }
        #endregion

        public static void CSVSetup(string CSVName, string[] headerRow)
        {

        }

        public static void CSVSetup(string CSVName, int cols)
        {

        }

        public static void PrintToCSV(string[] row)
        {

        }

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

        public static void Print(string print, PrintType type = PrintType.NORMAL)
        {
            Print(new string[] { print }, type);
        }

        public static void PrintWarn(string print)
        {
            Print(new string[] { print }, PrintType.WARNING);
        }

        public static void PrintErr(string print)
        {
            Print(new string[] { print }, PrintType.ERROR);
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
