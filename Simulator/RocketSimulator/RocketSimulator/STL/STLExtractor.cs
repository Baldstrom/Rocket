using ALPHASim.SimMath;
using RocketSimulator.CLI;
using RocketSimulator.Parts;
using RocketSimulator.STL;
using System;
using System.Collections.Generic;
using System.IO;
using static RocketSimulator.Parts.Rocket;

namespace RocketSimulator.STL
{
    public class STLExtractor
    {
        // Max length of an STL header
        public const int STL_BIN_HEADER_LENGTH = 80;
        public const int STL_BIN_FACET_LENGTH = 50;
        public const int STL_BIN_NUM_FACET_LENGTH = 8;

        private const int MAX_BIN_STL_FACETS_PRINT_LOADING = 300000;

        public bool IsValid { get; private set; }
        public FileValidity ValidState { get; private set; }
        
        private STLFile STL;

        public STLExtractor(string stlFileLocation, STLInfo.STLUnits units = STLInfo.STLUnits.Centimeters)
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
            
            return new Rocket(config, STL.Surfaces);
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
        
        private class STLFile
        {
            public string Name;
            public STLInfo.STLType Type;
            public STLInfo.STLUnits Units;

            public List<Surface> Surfaces;

            private FileStream fileStream;
            private StreamReader reader;
            private long NumFacets;

            public STLFile(FileStream fileStream, STLInfo.STLUnits units)
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
                    Type = STLInfo.STLType.ASCII;
                }
                else
                {
                    // Read the rest of the header, call that the name
                    for (int i = lookIndex; i < header.Length; i++)
                    {
                        header[i] = (char)fileStream.ReadByte();
                    }
                    Name = header.ToString();
                    Type = STLInfo.STLType.Binary;
                }
            }

            private void GetSurfaces()
            {
                Logging.Print("DECODING STL FILE...");
                if (this.Type == STLInfo.STLType.ASCII) { GetASCIISurfaces(); }
                else { GetBinarySurfaces(); }
                DetermineExteriorSurfaces();
            }

            // Use ray tracing for determination
            private void DetermineExteriorSurfaces()
            {
                int percentIndMax = this.Surfaces.Count;
                int percentInd = 0;
                Logging.Print("ANALYZING STL FILE...");
                Logging.OpenPercentageIndicator(percentIndMax, true);

                foreach (Surface thisSurface in this.Surfaces)
                {
                    percentInd++;
                    Logging.UpdatePercentageIndicator(percentInd);
                    // Foreach surface see if another surface collides with 
                    // normal vector in either direction.
                    if (!SurfaceIsInterior(thisSurface))
                    {
                        // If a direction does not result in collision, it is an exterior surface
                        // So do as such
                        thisSurface.IsExterior = true;
                    }
                }
                Logging.ClosePercentageIndicator();
                Logging.Print("FINISHED STL ANALYSIS.");
            }

            private bool SurfaceIsInterior(Surface testSurface)
            {
                int posDirCnt = 0;
                int negDirCnt = 0;
                // Loop on all other surfaces and check for collision
                foreach (Surface checkSurface in this.Surfaces)
                {
                    if (checkSurface != testSurface)
                    {
                        // Find intersection of normal of testSurface to checkSurface 
                        // (assuming infinite plane)
                        // Check if point of intersection is within the vertices of the surface
                        // If it is, it has collided, return false
                        bool collides = PointOnSurface(CollisionPoint(testSurface, checkSurface, out int direction), checkSurface);
                        if (collides)
                        {
                            if (direction >= 0) { posDirCnt++; }
                            else { negDirCnt++; }
                        }
                        if (posDirCnt > 0 && negDirCnt > 0) { return true; }
                    }
                }
                return false;
            }

            private Vector3D<double> CollisionPoint(Surface surface1, Surface surface2, out int direction)
            {
                // We have all the information for a plane equation
                // Plane equation given as: (N=normal vector) <x0, y0, z0> a point on plane. (p0)
                // N.x * (x-x0) + N.y * (y-y0) + N.z * (z-z0) = 0

                // Find two points on the line to parameterize the line / normal vector
                // r(t) = p1 + t*(p2-p1)
                // r(t) = point1 + diff * t

                // Find intersection with other normal
                // Solve N.x * (r(t).X-x0) + N.Y * (r(t).Y-y0) + N.z * (r(t).Z-z0) = 0
                // Find, t, then solve for r(t) x = r(t).X

                // Expand
                // N.x * r(t).X - N.x*x0 + N.Y * r(t).Y - N.Y * y0 + N.z * r(t).Z - N.z * z0 = 0
                // N.x * r(t).X + N.y * r(t).y + N.z * r(t).z = dot(N, p0)
                // dot(N, r(t)) = dot(N, p0)
                // solve for t

                // Expand this further:
                // N.x * r(t).X + N.y * r(t).y + N.z * r(t).z = dot(N, p0)
                // r(t).X = p1.X + diff.X*t, r(t).Y = ...
                // N.X * (p1.X + diff.X*t) + N.Y * (p1.Y + diff.Y*t) + N.Z * (p1.Z + diff.Z*t) = dot(N, p0)
                // N.X*p1.X + N.X*diff.X*t + N.Y*p1.Y + N.Y*diff.Y*t + N.Z*p1.Z + N.Z*diff.Z*t = dot(N, p0)
                // dot(N,diff)*t = dot(N, p0) - dot(N,p1)
                // t = (dot(N, p0) - dot(N, p1)) / dot(N,diff)
                
                // t solved as follows:
                double t = (surface2.Normal.Dot(surface1.Position) - surface2.Normal.Dot(surface2.Position)) / surface2.Normal.Dot(surface2.Normal);
                direction = t >= 0 ? 1 : -1;
                return surface2.Position.Add(surface2.Normal.Multiply(t));
            }

            private bool PointOnSurface(Vector3D<double> point, Surface plane)
            {
                bool inside = PointOnSameSide(point, plane.Vertices[0], plane.Vertices[1], plane.Vertices[2]);
                inside &= PointOnSameSide(point, plane.Vertices[1], plane.Vertices[0], plane.Vertices[2]);
                inside &= PointOnSameSide(point, plane.Vertices[2], plane.Vertices[0], plane.Vertices[1]);
                return inside;
            }

            private bool PointOnSameSide(Vector3D<double> p1, Vector3D<double> p2, Vector3D<double> A, Vector3D<double> B)
            {
                return (B - A).Cross(p1 - A).Dot((B - A).Cross(p2 - A)) >= 0;
            }
            private void GetASCIISurfaces()
            {
                Logging.Print("DETECTED ASCII STL");

                string nextLine = ReadNextASCIILine();
                Vector3D<double> normal = new Vector3D<double>();
                Vector3D<double> curVertex = new Vector3D<double>();
                float[] xyz = new float[3];
                string[] str;
                int curXYZ = 0;
                int numVerticesAdded = 0;
                bool started = false;
                Surface surface = new Surface();

                int ThisPercentageIndicator = 5;
                int CurrentPercentageIndicator = 0;

                Logging.OpenPercentageIndicator(ThisPercentageIndicator, false);

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
                    CurrentPercentageIndicator++;
                    Logging.UpdatePercentageIndicator(CurrentPercentageIndicator % ThisPercentageIndicator);
                }
                Logging.ClosePercentageIndicator();
            }

            private void GetBinarySurfaces()
            {
                Logging.Print("DETECTED BINARY STL");

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

                if (NumFacets > MAX_BIN_STL_FACETS_PRINT_LOADING)
                {
                    Logging.OpenPercentageIndicator(NumFacets, true);
                }

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
                    if (Logging.IsPercentageIndicatorOpen)
                    {
                        Logging.UpdatePercentageIndicator(i);
                    }
                }
                if (Logging.IsPercentageIndicatorOpen)
                {
                    Logging.ClosePercentageIndicator();
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

        }

    }
}
