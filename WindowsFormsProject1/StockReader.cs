using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsProject1
{
    /// <summary>
    /// Class for reading candlestick data from a CSV file.
    /// </summary>
    public class StockReader
    {
        /// <summary>
        /// Reads candlestick data from a CSV file and converts it into a list of CandleStick objects.
        /// </summary>
        /// <param name="filePath">The path to the CSV file containing data.</param>
        /// <returns>A list of CandleStick objects representing the data from the CSV file.</returns>
        public List<CandleStick> ReadCandleStickFromCsv(string filePath)
        {
            var candlesticks = new List<CandleStick>(); // Creates a new empty list to store CandleStick objects

            // Open the file for reading
            using (var reader = new StreamReader(filePath))
            {
                var header = reader.ReadLine(); // Ignore the header line

                // Read each line and add a the new CandleStick object to the list
                while (!reader.EndOfStream) 
                {
                    candlesticks.Add(new CandleStick(reader.ReadLine())); // Read a line from the file and convert it into a CandleStick object
                }
            }

            candlesticks.Reverse(); // Reverse the list to match chronological order
            return candlesticks; // Return the list of candlesticks
        }
    }
}
