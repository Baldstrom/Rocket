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
            bool success = false;
            // Write file if switch on stl type
            if (type == STLInfo.STLType.Binary)
            {
                if (WriteBinaryHeader(null, filename))
                {
                    success = WriteBinarySTL(null, surfaces);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            stream.Close();
            return success;
        }

        private static bool WriteASCIIHeader(FileStream file, string name)
        {
            throw new NotImplementedException();
        }

        private static bool WriteBinaryHeader(FileStream file, string name)
        {

            return false;
        }

        private static bool WriteASCIISTL(FileStream file, List<Surface> surfaces)
        {
            throw new NotImplementedException();
        }

        private static bool WriteBinarySTL(FileStream file, List<Surface> surfaces)
        {
            return false;
        }




    }
}
