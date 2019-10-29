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
        /// <summary> Gets the type of the STL File. </summary>
        public STLFileType Type { get; private set; }

        /// <summary> Filename of the STL file. </summary>
        private string filename;

        /// <summary>
        /// Initializes an STLFile object with given operating mode
        /// and filename.
        /// </summary>
        public STLFile(string filename, STLFileOperationMode operatingMode) 
        {
            
        }

        /// <summary>
        /// Gets the STL file header as a set of bytes.
        /// </summary>
        /// <exception cref="InvalidOperationException"> 
        /// If the STL file header cannot be read or is incomplete. 
        /// </exception>
        /// <returns> Header as an array of bytes. </returns>
        public byte[] GetHeaderAsBytes() 
        {
            throw new NotImplementedException();   
        }

        /// <summary>
        /// Gets the STL file header as an ASCII string.
        /// </summary>
        /// <exception cref="InvalidOperationException"> 
        /// If the STL file header cannot be read or is incomplete. 
        /// </exception>
        /// <returns> Header as interpreted as an ASCII string. </returns>
        public string GetHeaderAsASCII() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to determine the STL file type
        /// </summary>
        /// <exception cref="Exception"> 
        /// If an STL file type cannot be determined. 
        /// </exception>
        /// <returns>  </returns>
        private STLFileType GetSTLFileType()
        {
            throw new NotImplementedException();
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
