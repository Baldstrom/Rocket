using RocketSimulator.Parts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator.STL
{
    public static class STLInserter
    {
        private const string STL_EXTENSION = ".stl";

        // Do units matter?
        public static bool CreateSTLFile(string filename, 
            List<Surface> surfaces, 
            STLInfo.STLType type = STLInfo.STLType.Binary, 
            STLInfo.STLUnits units = STLInfo.STLUnits.Centimeters)
        {
            // Create file.
            string filepath = filename + STL_EXTENSION;
            File.Create(filepath);
            FileStream stream = File.OpenWrite(filepath);

            if (type == STLInfo.STLType.Binary)
            {
                if (WriteBinaryHeader(null, filename))
                {
                    bool success = WriteBinarySTL(null, surfaces);
                } else { return false; }
            }
            else
            {
                if (WriteASCIIHeader(null, filename))
                {
                    bool success = WriteASCIISTL(null, surfaces);
                    
                } else { return false; }
            }
            return true;
        }

        private static bool WriteASCIIHeader(FileStream file, string name)
        {
            return false;
        }

        private static bool WriteBinaryHeader(FileStream file, string name)
        {
            return false;
        }

        private static bool WriteASCIISTL(FileStream file, List<Surface> surfaces)
        {
            return false;
        }

        private static bool WriteBinarySTL(FileStream file, List<Surface> surfaces)
        {
            return false;
        }




    }
}
