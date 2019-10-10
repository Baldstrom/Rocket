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
        // Do units matter?
        public static bool CreateSTLFile(string filename, 
            List<Surface> surfaces, 
            STLInfo.STLType type = STLInfo.STLType.Binary)
        {
            // Create file.
            string filepath = DetermineFilename(filename);
            FileStream stream = File.OpenWrite(filepath);

            bool success = false;
            // Write file if switch on stl type
            if (type == STLInfo.STLType.Binary)
            {
                if (WriteBinaryHeader(stream, filename))
                {
                    success = WriteBinarySTL(stream, surfaces);
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
            // TODO: Check if name begins with SOLID?
            // Write name
            byte[] headerBytes = new byte[STLInfo.STL_BIN_HEADER_LENGTH];
            byte space = Encoding.UTF8.GetBytes(" ")[0];
            // Fill header with spaces
            for (int i = 0; i < headerBytes.Length; i++) { headerBytes[i] = space; }

            // Fill header with name
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            if (nameBytes.Count() > STLInfo.STL_BIN_HEADER_LENGTH)
            {
                headerBytes = nameBytes.ToList().Take(80).ToArray();
            }

            else { nameBytes.CopyTo(headerBytes, 0); }
            file.Write(headerBytes, 0, STLInfo.STL_BIN_HEADER_LENGTH);
            return true;
        }

        private static bool WriteASCIISTL(FileStream file, List<Surface> surfaces)
        {
            throw new NotImplementedException();
        }

        private static bool WriteBinarySTL(FileStream file, List<Surface> surfaces)
        {
            // Write number of surfaces
            byte[] numSurfaces = BitConverter.GetBytes((uint)surfaces.Count());
            file.Write(numSurfaces, 0, numSurfaces.Length);

            byte[] result = new byte[50];

            byte[] normal = new byte[12];
            byte[] v1 = new byte[12];
            byte[] v2 = new byte[12];
            byte[] v3 = new byte[12];

            float[] normalFloat = new float[3];
            float[] v1Float = new float[3];
            float[] v2Float = new float[3];
            float[] v3Float = new float[3];

            byte[] attByteCount = { 0x00, 0x00 }; // Apparently most software doesn't understand anything else.

            byte[] thisFaceBytes = new byte[50];
            foreach (Surface face in surfaces)
            {
                normalFloat[0] = face.Normal.X;
                normalFloat[1] = face.Normal.Y;
                normalFloat[2] = face.Normal.Z;

                Buffer.BlockCopy(BitConverter.GetBytes(normalFloat[0]), 0, normal, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(normalFloat[1]), 0, normal, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(normalFloat[2]), 0, normal, 8, 4);

                Buffer.BlockCopy(normal, 0, result, 0, 12);

                v1Float[0] = face.Vertices[0].X;
                v1Float[1] = face.Vertices[0].Y;
                v1Float[2] = face.Vertices[0].Z;

                Buffer.BlockCopy(BitConverter.GetBytes(v1Float[0]), 0, v1, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(v1Float[1]), 0, v1, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(v1Float[2]), 0, v1, 8, 4);

                Buffer.BlockCopy(v1, 0, result, 12, 12);

                v2Float[0] = face.Vertices[1].X;
                v2Float[1] = face.Vertices[1].Y;
                v2Float[2] = face.Vertices[1].Z;

                Buffer.BlockCopy(BitConverter.GetBytes(v2Float[0]), 0, v2, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(v2Float[1]), 0, v2, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(v2Float[2]), 0, v2, 8, 4);

                Buffer.BlockCopy(v2, 0, result, 24, 12);

                v3Float[0] = face.Vertices[2].X;
                v3Float[1] = face.Vertices[2].Y;
                v3Float[2] = face.Vertices[2].Z;

                Buffer.BlockCopy(BitConverter.GetBytes(v3Float[0]), 0, v3, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(v3Float[1]), 0, v3, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(v3Float[2]), 0, v3, 8, 4);

                Buffer.BlockCopy(v3Float, 0, result, 36, 12);

                // Add att byte count
                Buffer.BlockCopy(attByteCount, 0, result, 48, 2);

                // Write bytes
                file.Write(result, 0, 50);
            }
            return true;
        }

        private static string DetermineFilename(string desiredName)
        {
            string result = desiredName;
            int iteration = 1;
            while(File.Exists(result + STLInfo.STL_EXTENSION))
            {
                if (iteration > 1)
                {
                    string numberAsStr = iteration.ToString();
                    int ItLength = numberAsStr.Length;
                    result = result.Remove(result.Length - (ItLength + 3), (ItLength + 3));
                }
                result += " (" + iteration.ToString() + ")";
                iteration++;
            }
            return result + STLInfo.STL_EXTENSION;
        }


    }
}
