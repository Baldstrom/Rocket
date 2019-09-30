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
        private static ConsoleColor DefaultConsoleColor;

        private static bool IsPercentageIndicatorOpen;
        private static long PercentageIndicatorMax;
        private static bool PercentageIndicatorTelemetryVisible;

        public static int PercentageWidth = 100;

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
            if (list.Count() > 0 && CurrentConsoleConditions != type)
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

        public static void OpenPercentageIndicator(long max, bool telemetryVisible = false)
        {
            if (!IsPercentageIndicatorOpen)
            {
                Console.CursorVisible = false;
                Console.Write("[");
                for(int i = 0; i < PercentageWidth; i++)
                {
                    Console.Write(" ");
                }
                Console.Write("]");
                if (telemetryVisible) { Console.Write(" (0 / " + max.ToString() + ")"); }
                PercentageIndicatorMax = max;
                IsPercentageIndicatorOpen = true;
                PercentageIndicatorTelemetryVisible = telemetryVisible;
            }
        }

        public static void UpdatePercentageIndicator(long current)
        {
            if (IsPercentageIndicatorOpen)
            {
                Console.Write("\r");
                Console.Write("[");
                for(int i = 0; i < PercentageWidth; i++)
                {
                    // Get Percentage 
                    long percent = (10000 * current) / (PercentageIndicatorMax * PercentageWidth);

                    if (i < percent)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("]");
                if (PercentageIndicatorTelemetryVisible)
                {
                    Console.Write(" (" + current.ToString() + " / " + PercentageIndicatorMax.ToString() + ")");
                }
            }
        }

        public static void ClosePercentageIndicator()
        {
            IsPercentageIndicatorOpen = false;
            Console.CursorVisible = true;
            Console.WriteLine();
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
            Console.ForegroundColor = DefaultConsoleColor;
            CurrentConsoleConditions = PrintType.NORMAL;
        }

        private static void SetConsoleWarning()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            CurrentConsoleConditions = PrintType.WARNING;
        }

        private static void SetConsoleError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            CurrentConsoleConditions = PrintType.ERROR;
        }

        static Logging()
        {
            DefaultConsoleColor = Console.ForegroundColor;
        }

        public static void Close()
        {
            SetConsoleNormal();
        }
    }
}
