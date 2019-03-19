using ALPHASim.SimMath;
using RocketSimulator.Parts;
using System;
using System.Collections.Generic;
using System.IO;
using static RocketSimulator.Parts.Rocket;

namespace RocketSimulator
{
    public class STLExtractor
    {
        // Max length of an STL header
        public const int STL_HEADER_LENGTH = 80;

        public bool IsValid { get; private set; }
        public FileValidity ValidState { get; private set; }
        
        private STLFile STL;

        public STLExtractor(string stlFileLocation, StlUnits units = StlUnits.Centimeters)
        {
            if (File.Exists(stlFileLocation))
            {
                ValidState = FileValidity.Valid;
                IsValid = true;
                try { STL = new STLFile(File.Open(stlFileLocation, FileMode.Open), units);  }
                catch (ArgumentNullException) { IsValid = false; ValidState = FileValidity.FileNameNull; }
                catch (UnauthorizedAccessException) { IsValid = false; ValidState = FileValidity.UnauthorizedAccess; }
                catch (ArgumentException) { IsValid = false; ValidState = FileValidity.InvalidFile; }
                catch (PathTooLongException) { IsValid = false; ValidState = FileValidity.PathTooLong; }
                catch (NotSupportedException) { IsValid = false; ValidState = FileValidity.UnsupportedFileOperation; }
            } 
            else
            {
                ValidState = FileValidity.WrongPath;
                IsValid = false;
            }
        }

        public Rocket RocketFromSTL()
        {
            if (!IsValid) { throw new InvalidOperationException("Cannot make rocket from invalid STL file. Check file validity and try again."); }
            
            // Set configuration
            RocketConfiguration config = new RocketConfiguration()
            {
                CenterOfMass = new Vector3D<double>(),
                CenterOfPressure = new Vector3D<double>(),
                CenterOfThrust = new Vector3D<double>(),
            };
            // Set surfaces
            List<Surface> surfaces = new List<Surface>();

            return new Rocket(config, surfaces);
        }

        public void SwapOrientation(OrientationVector newX, OrientationVector newY, OrientationVector newZ)
        {
            // Swap the orientation of the STL (So that the system is ambiguous of which way is z)

        }

        public enum OrientationVector { X, Y, Z }

        public enum FileValidity
        {
            Valid,
            WrongPath,
            UnauthorizedAccess,
            InvalidFile,
            FileNameNull,
            PathTooLong,
            UnsupportedFileOperation
        }

        public enum StlUnits
        {
            Millimeters,
            Centimeters,
            Micrometers,
            Meters,
            Mils,
            Inches,
        }


        private class STLFile
        {
            public string Name;
            public StlType Type;
            public StlUnits units;

            public List<Surface> Surfaces;

            private StreamReader reader;
            private long NumFacets;

            public STLFile(FileStream fileStream, StlUnits units)
            {
                // Set vars
                this.units = units;
                reader = new StreamReader(fileStream);

                // Determine type and name of STL
                GetTypeAndName(fileStream);
                // Correct reader now at end of header element

                // Get vertices, surface normal vectors, triangles
                // I.E. GetSurfaces
                GetSurfaces();
            }

            public void GetTypeAndName(FileStream file)
            {
                // Read 80 bytes or until "solid"
                char[] header = new char[STL_HEADER_LENGTH];
                string solid = "solid ";
                int lookIndex = 0;
                char lookingFor = solid[lookIndex];
                char lookingAt = (char)file.ReadByte();
                bool FileIsASCII = false;
                while (lookingFor == lookingAt)
                {
                    if (lookIndex == solid.Length) { FileIsASCII = true; }
                    // Set loop characteristics
                    lookIndex++;
                    lookingAt = (char)file.ReadByte();
                    header[lookIndex] = lookingAt;
                    lookingFor = solid[lookIndex];
                }

                if (FileIsASCII)
                {
                    // Use streamreader
                    // Read line to determine name
                    Name = reader.ReadLine();
                    Type =  StlType.ASCII;
                }
                else
                {
                    // Read the rest of the header, call that the name
                    for (int i = lookIndex; i < header.Length; i++)
                    {
                        header[i] = (char)file.ReadByte();
                    }
                    Name = header.ToString();
                    Type = StlType.Binary;
                }
            }

            public void GetSurfaces()
            {
                // Get number of facets
                // Add to surfaces the surface
            }
            
            public enum StlType
            {
                Binary,
                ASCII
            }
        }

    }
}
