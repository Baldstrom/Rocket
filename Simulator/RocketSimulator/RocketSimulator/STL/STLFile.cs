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
    using System.IO;
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

        /// <summary> Stream for the STL file. </summary>
        private FileStream STLFileStream;

        /// <summary>
        /// Initializes an STLFile object with given operating mode
        /// and filename.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Throws if a file exists when using an operating mode that 
        /// creates one, or if a file does not exist when the operating
        /// mode expects it.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If operating mode is invalid (not in enum).
        /// </exception>
        public STLFile(string filename, STLFileOperationMode operatingMode) 
        {
            switch (operatingMode)
            {
                case STLFileOperationMode.CreateNew:
                    if (!File.Exists(filename)) { this.STLFileStream = File.Create(filename); } 
                    else { throw new InvalidOperationException(); }
                    break;
                case STLFileOperationMode.EnsureExists:
                    if (File.Exists(filename)) { this.STLFileStream = File.Open(filename, FileMode.OpenOrCreate); }
                    break;
                case STLFileOperationMode.GetExisting:
                    if (File.Exists(filename)) { this.STLFileStream = File.Open(filename, FileMode.Open); } 
                    else { throw new InvalidOperationException(); }
                    break;
                default:
                    throw new ArgumentException();
            }
            this.Type = this.GetSTLFileType();
        }

        /// <summary>
        /// Writes the header of the STL File.
        /// </summary>
        /// <param name="newHeader"> New header information to inject into STL. </param>
        public void WriteHeader(byte[] newHeader)
        {

        }

        /// <summary>
        /// Writes to the body content of STL File.
        /// </summary>
        /// <param name="newBody"> New body content to add. </param>
        public void WriteBody(byte[] newBody)
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
        /// Attempts to determine the STL file type.
        /// </summary>
        /// <exception cref="Exception"> 
        /// If an STL file type cannot be determined. 
        /// </exception>
        /// <returns> The file type of the STL. </returns>
        private STLFileType GetSTLFileType()
        {
            throw new NotImplementedException();
        }
    }

    public enum STLFileOperationMode 
    {
        CreateNew,
        EnsureExists,
        GetExisting,
    }

    public enum STLFileType 
    {
        Binary,
        ASCII,
    }
}
