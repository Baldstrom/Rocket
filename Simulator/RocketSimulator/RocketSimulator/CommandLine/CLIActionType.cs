/// <summary>
/// Module Name: CLIActionType.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds the CLIActionType Enum
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.CommandLine
{
    /// <summary>
    /// CLIActionType enumerates types of actions
    /// that can be taken by the user in the
    /// command line interface.
    /// </summary>
    public enum CLIActionType
    {
        /// <summary>
        /// Defines the ArgDebug.
        /// </summary>
        ArgDebug,

        /// <summary>
        /// Defines the Test.
        /// </summary>
        Test,

        /// <summary>
        /// Defines the CSVSetup.
        /// </summary>
        CSVSetup,

        /// <summary>
        /// Defines the EnableCSV.
        /// </summary>
        EnableCSV,

        /// <summary>
        /// Defines the LoadSTL.
        /// </summary>
        LoadSTL,

        /// <summary>
        /// Defines the TimeScale.
        /// </summary>
        TimeScale,

        /// <summary>
        /// Defines the Writeback.
        /// </summary>
        Writeback,

        /// <summary>
        /// Defines the WritebackExterior.
        /// </summary>
        WritebackExterior,
    }
}
