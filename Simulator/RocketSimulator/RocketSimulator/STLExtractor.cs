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
        public const int STL_BIN_HEADER_LENGTH = 80;
        public const int STL_BIN_FACET_LENGTH = 50;
        public const int STL_BIN_NUM_FACET_LENGTH = 8;

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
            if (!IsValid)
            {
                throw new InvalidOperationException("Cannot make rocket from invalid STL file. Check file validity and try again.");
            }
            
            // Set configuration
            RocketConfiguration config = new RocketConfiguration()
            {
                CenterOfMass = new Vector3D<double>(),
                CenterOfPressure = new Vector3D<double>(),
                CenterOfThrust = new Vector3D<double>(),
            };
            // Set surfaces
            List<Surface> surfaces = STL.Surfaces;

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
            public StlUnits Units;

            public List<Surface> Surfaces;

            private FileStream fileStream;
            private StreamReader reader;
            private long NumFacets;

            public STLFile(FileStream fileStream, StlUnits units)
            {
                // Set vars
                Surfaces = new List<Surface>();
                Units = units;
                this.fileStream = fileStream;
                reader = new StreamReader(fileStream);

                // Determine type and name of STL
                GetTypeAndName();
                // Correct reader now at end of header element

                // Get vertices, surface normal vectors, triangles
                // I.E. Get Surfaces
                GetSurfaces();
            }

            private void GetTypeAndName()
            {
                // Read 80 bytes or until "solid"
                char[] header = new char[STL_BIN_HEADER_LENGTH];
                string solid = "solid ";
                int lookIndex = 0;
                char lookingFor = solid[lookIndex];
                char lookingAt = (char)fileStream.ReadByte();
                bool FileIsASCII = false;
                while (lookingFor == lookingAt)
                {
                    // Set loop characteristics
                    lookIndex++;
                    if (lookIndex == solid.Length)
                    {
                        FileIsASCII = true;
                        break;
                    }
                    else
                    {
                        lookingAt = (char)fileStream.ReadByte();
                        header[lookIndex] = lookingAt;
                        lookingFor = solid[lookIndex];
                    }
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
                        header[i] = (char)fileStream.ReadByte();
                    }
                    Name = header.ToString();
                    Type = StlType.Binary;
                }
            }

            private void GetSurfaces()
            {
                Logging.Print("DECODING STL FILE...");
                if (this.Type == StlType.ASCII) { GetASCIISurfaces(); }
                else { GetBinarySurfaces(); }
                DetermineExteriorSurfaces();
            }

            // Use ray tracing for determination
            private void DetermineExteriorSurfaces()
            {
                Logging.Print("ANALYZING STL FILE...");
                foreach (Surface thisSurface in this.Surfaces)
                {
                    // Foreach surface see if another surface collides with 
                    // normal vector in either direction.
                    if (!SurfaceCollidesWithOther(thisSurface))
                    {
                        // If a direction does not result in collision, it is an exterior surface
                        // So do as such
                        thisSurface.IsExterior = true;
                    }
                }
                Logging.Print("FINISHED STL ANALYSIS.");
            }

            private bool SurfaceCollidesWithOther(Surface testSurface)
            {
                // Loop on all other surfaces and check for collision
                foreach (Surface checkSurface in this.Surfaces)
                {
                    if (checkSurface != testSurface)
                    {
                        // Find intersection of normal of testSurface to checkSurface 
                        // (assuming infinite plane)

                        // Check if point of intersection is within the vertices of the surface
                        // If it is, it has not collided, return false

                        // If it did collide, check other normal direction
                        Vector3D<double> otherNormal = testSurface.Normal.Multiply(-1);

                    }
                }

                return false;
            }

            private void GetASCIISurfaces()
            {
                string nextLine = ReadNextASCIILine();
                Vector3D<double> normal = new Vector3D<double>();
                Vector3D<double> curVertex = new Vector3D<double>();
                float[] xyz = new float[3];
                string[] str;
                int curXYZ = 0;
                int numVerticesAdded = 0;
                bool started = false;
                Surface surface = new Surface();
                while (nextLine != string.Empty && !nextLine.Contains("endsolid"))
                {
                    if (numVerticesAdded == 3)
                    {
                        if (started) { Surfaces.Add(surface); }
                        else { started = true; }
                        numVerticesAdded = 0;
                        surface = new Surface();
                    }
                    str = nextLine.Split(' ', '\t');
                    curXYZ = 0;
                    foreach(string s in str)
                    {
                        if (float.TryParse(s, out xyz[curXYZ])) { curXYZ++; }
                    }
                    if (nextLine.Contains("facet normal"))
                    {
                        normal.X = xyz[0];
                        normal.Y = xyz[1];
                        normal.Z = xyz[2];
                        surface.SetNormal(normal);
                    }
                    else if (nextLine.Contains("vertex"))
                    {
                        curVertex.X = xyz[0];
                        curVertex.Y = xyz[1];
                        curVertex.Z = xyz[2];

                        surface.AddVertex(curVertex);

                        numVerticesAdded++;
                    }
                    // Next Line
                    nextLine = ReadNextASCIILine();
                }
            }

            private void GetBinarySurfaces()
            {
                byte[] numFacets = new byte[STL_BIN_NUM_FACET_LENGTH];
                // Skip header
                fileStream.Seek(STL_BIN_HEADER_LENGTH, SeekOrigin.Begin);
                // Read facets
                fileStream.Read(numFacets, 0, STL_BIN_NUM_FACET_LENGTH / 2);
                // Facets count is little endian
                if (!BitConverter.IsLittleEndian)
                {
                    // TODO: Transpose bytes into big endian
                }

                NumFacets = BitConverter.ToInt64(numFacets, 0);
                byte[] facet = new byte[STL_BIN_FACET_LENGTH];
                Vector3D<double> normal = new Vector3D<double>();
                Vector3D<double> vX = new Vector3D<double>();
                Vector3D<double> vY = new Vector3D<double>();
                Vector3D<double> vZ = new Vector3D<double>();
                for (long i = 0; i < NumFacets; i++)
                {
                    fileStream.Read(facet, 0, STL_BIN_FACET_LENGTH);
                    if (!BitConverter.IsLittleEndian)
                    {
                        // TODO: Transpose to big endian
                    }

                    // Normal
                    normal.X = BitConverter.ToSingle(facet, 0);
                    normal.Y = BitConverter.ToSingle(facet, 4);
                    normal.Z = BitConverter.ToSingle(facet, 8);
                    // X
                    vX.X = BitConverter.ToSingle(facet, 12);
                    vX.Y = BitConverter.ToSingle(facet, 16);
                    vX.Z = BitConverter.ToSingle(facet, 20);
                    // Y
                    vY.X = BitConverter.ToSingle(facet, 24);
                    vY.Y = BitConverter.ToSingle(facet, 28);
                    vY.Z = BitConverter.ToSingle(facet, 32);
                    // Z
                    vZ.X = BitConverter.ToSingle(facet, 36);
                    vZ.Y = BitConverter.ToSingle(facet, 40);
                    vZ.Z = BitConverter.ToSingle(facet, 44);
                    // Byte Count (ignored, 2 bytes)

                    // Set surface
                    Surface surface = new Surface();

                    surface.SetNormal(normal);
                    surface.AddVertex(vX);
                    surface.AddVertex(vY);
                    surface.AddVertex(vZ);

                    // Add surface to list
                    Surfaces.Add(surface);
                }
            }

            /// <summary>
            /// Hopefully we do not need this.
            /// </summary>
            /// <returns></returns>
            private Vector3D<double> GetPositionFromVertices()
            {
                throw new NotImplementedException();
            }

            private string ReadNextASCIILine()
            {
                try { return reader.ReadLine(); }
                catch (IOException) { throw new IOException("Could not read ASCII STL."); }
            }

            public enum StlType
            {
                Binary,
                ASCII
            }

        }

    }
}
