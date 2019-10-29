/// <summary>
/// Module Name: STLFile.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure for defining an STL file.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.STL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an existing or instantiates a new STLFile.
    /// </summary>
    public class STLFile
    {
        public STLFileType Type { get; private set; }

        /// <summary>
        /// TODO: Describe initialization of STLFile object.
        /// </summary>
        public STLFile(string filename, STLFileOperationMode operatingMode) 
        {
            
        }

        public byte[] GetHeaderAsBytes() 
        {
            
        }

        public string GetHeaderAsASCII() 
        {

        }
    }

    public enum STLFileOperationMode 
    {
        CREATE_NEW,
        ENSURE_EXISTS,
        GET_EXISTING,
    }

    public enum STLFileType 
    {
        BINARY,
        ASCII,
    }
}
