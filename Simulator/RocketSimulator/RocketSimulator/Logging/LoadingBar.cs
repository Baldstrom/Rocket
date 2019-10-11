/// <summary>
/// Module Name: LoadingBar.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure for handling loading bars in the CLI.
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Instantiates a new command line loading bar.
    /// </summary>
    public class LoadingBar
    {
        /// <summary> Reference to the current loading bar. </summary>
        public static LoadingBar CurrentLoadingBarStatus { get; private set; }
        
        /// <summary> Delegate for OnUpdate event. <see cref="OnUpdate"/> </summary>
        public delegate void LoadingBarUpdate();

        /// <summary> Whether or not this loading bar is open. </summary>
        public bool IsOpen { get; private set; }

        /// <summary> Subscribe to this event to trigger on the loading bar updates. </summary>
        public event LoadingBarUpdate OnUpdate;
        
        /// <summary> 
        /// Opens a new loading bar. 
        /// Define maximum input. By default 100.
        /// Percent Complete = currentProgress / maxInput
        /// <see cref="UpdateLoadingBar(int)"/>
        /// </summary>
        /// <param name="maxInput"> Maximum input for Update(). </param>
        /// <returns> Whether or not the loading bar was able to be opened. </returns>
        public bool TryOpenLoadingBar(int maxInput = 100)
        {
            this.IsOpen = true;
            return this.IsOpen;
        }

        /// <summary>
        /// Updates the loading bar with the new progress.
        /// Percent Complete = currentProgress / maxInput (given in OpenLoadingBar)
        /// Must open loading bar before calling. <see cref="TryOpenLoadingBar(int)"/>
        /// </summary>
        /// <param name="currentProgress"> The new loading bar progress. </param>
        /// <returns> True if update was successful. False if update failed. </returns>
        public bool UpdateLoadingBar(int currentProgress)
        {
            if (this.IsOpen)
            {
                // Invoke OnUpdate event
                this.OnUpdate?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> Closes this loading bar. </summary>
        public void CloseLoadingBar()
        {
            if (this.IsOpen)
            {
                throw new NotImplementedException();
            }
        }
    }
}
