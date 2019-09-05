using System;
using System.IO;

namespace ALPHASim.Utility
{
    public class LogCSV
    {
        public const string DEFAULT_FILENAME = "ALPHASim";

        private StreamWriter stream;

        public LogCSV() : this(DEFAULT_FILENAME) { }

        public LogCSV(string CSVFilename)
        {
            bool Created = false;
            int Delimeter = 0;
            while(!Created)
            {
                try
                {
                    FileStream fileStream;
                    fileStream = File.Open(CSVFilename + "-" + Delimeter.ToString() + ".csv", FileMode.CreateNew);
                    stream = new StreamWriter(fileStream);
                    Created = true;
                }
                catch (IOException) { Created = false; }
                catch (UnauthorizedAccessException)
                {
                    throw new UnauthorizedAccessException("Error: ALPHASim does not have file write permissions.");
                }
            }
        }

        public void WriteLine(string line) { stream.WriteLine(line); }

        public void Write(string element, bool commaDelimit = true)
        {
            stream.Write(element + (commaDelimit ? "," : ""));
        }

        public void BreakLine() { stream.Write("\n"); }
    }
}
