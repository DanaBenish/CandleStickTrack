using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsProject1;

namespace Priject2
{
    public class Wave
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StartPrice { get; set; }  // Start Price (High for Up, Low for Down)
        public double EndPrice { get; set; }    // End Price (Low for Up, High for Down)
        public bool IsUpWave { get; set; }      // True for UpWave, False for DownWave

        /// <summary>
        /// Constructor for creating a Wave instance.
        /// Initializes the wave's start date, end date, start price, end price, and wave direction.
        /// </summary>
        /// <param name="startDate">The start date of the wave.</param>
        /// <param name="endDate">The end date of the wave.</param>
        /// <param name="startPrice">The start price of the wave.</param>
        /// <param name="endPrice">The end price of the wave.</param>
        /// <param name="isUpWave">Indicates whether the wave is an up wave (true) or a down wave (false).</param>
        public Wave(DateTime startDate, DateTime endDate, double startPrice, double endPrice, bool isUpWave)
        {
            StartDate = startDate;
            EndDate = endDate;
            StartPrice = startPrice;
            EndPrice = endPrice;
            IsUpWave = isUpWave;
        }

        /// <summary>
        /// Creates a list of waves based on the peaks and valleys detected in the given list of PeakVally objects.
        /// </summary>
        /// <param name="peakValleyList">A list of PeakVally objects that contain the peaks and valleys of stock data.</param>
        /// <returns>A list of Wave objects representing the up and down waves detected in the given peak-valley data.</returns>
        public static List<Wave> GetWaves(List<PeakVally> peakValleyList)
        {
            // Create a list to store the generated waves
            List<Wave> waves = new List<Wave>();

            // Loop through the peakValleyList to create waves based on pairs of valleys and peaks
            for (int i = 0; i < peakValleyList.Count - 1; i++)
            {
                // Get the current peak-valley pair
                var peakValley = peakValleyList[i];

                // Check if there's a valley and the next item in the list has a peak (for an Up Wave)
                if (peakValley.Valley != null && peakValleyList[i + 1].Peak != null)
                {
                    // Create an Up Wave using the Valley -> Peak
                    Wave upWave = new Wave(
                        peakValley.Valley.Data, // Start date (Valley date)
                        peakValleyList[i + 1].Peak.Data, // End date (Peak date)
                        (double)peakValley.Valley.Low, // Start price (Valley low)
                        (double)peakValleyList[i + 1].Peak.High, // End price (Peak high)
                        true // UpWave flag is true
                    );
                    // Add the generated Up Wave to the list of waves
                    waves.Add(upWave);
                }

                // Check if there's a Peak and the next item has a Valley (for a Down Wave)
                if (i + 1 < peakValleyList.Count)
                {
                    var nextPeakValley = peakValleyList[i + 1];
                    if (peakValley.Peak != null && nextPeakValley.Valley != null)
                    {
                        // Create a Down Wave using the Peak -> Valley
                        Wave downWave = new Wave(
                            peakValley.Peak.Data, // Start date (Peak date)
                            nextPeakValley.Valley.Data, // End date (Valley date)
                            (double)peakValley.Peak.High, // Start price (Peak high)
                            (double)nextPeakValley.Valley.Low, // End price (Valley low)
                            false // UpWave flag is false for DownWave
                        );
                        // Add the generated Down Wave to the list of waves
                        waves.Add(downWave);
                    }
                }
            }

            // Return the list of generated waves
            return waves;
        }
    }
 }
