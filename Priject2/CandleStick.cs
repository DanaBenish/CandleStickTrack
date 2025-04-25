using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsProject1
{
    /// <summary>
    /// Represents a single candlestick in stock data with timestamp, open, high, low, close prices, and volume.
    /// </summary>
    public class CandleStick
    {
        public DateTime Data { get; set; } // The date and time of the candlestick
        public decimal Open { get; set; } // The opening price of the candlestick
        public decimal High { get; set; } // The highest price of the candlestick
        public decimal Low { get; set; } // The lowest price of the candlestick
        public decimal Close { get; set; }  // The closing price of the candlestick
        public decimal Volume { get; set; } // The trading volume of the candlestick

        /// <summary>
        /// Constructor for creating a CandleStick object its properties.
        /// </summary>
        /// <param name="data">Date and time of the candlestick</param>
        /// <param name="open">Opening price</param>
        /// <param name="high">Highest price</param>
        /// <param name="low">Lowest price</param>
        /// <param name="close">Closing price</param>
        /// <param name="volume">Trading volume</param>
        public CandleStick(DateTime data, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            Data = data; // Assigning date
            Open = open; // Assigning open price
            High = high; // Assigning highest price
            Low = low; // Assigning lowest price
            Close = close; // Assigning closing price
            Volume = volume; // Assigning trading volume
        }

        /// <summary>
        /// Constructor for creating a CandleStick object using a CSV string.
        /// </summary>
        /// <param name="data">CSV formatted string: Timestamp,Open,High,Low,Close,Volume</param>
        /// <exception cref="FormatException">Thrown if the input string does not contain exactly 6 comma-separated values.</exception>
        public CandleStick(string data)
        {
            var separators = new char[] { ',', '\"' }; // Define separators - comma and quotation mark
            var values = data.Split(separators, StringSplitOptions.RemoveEmptyEntries); // Split the input string into values

            // Validate that the CSV format has 6 values
            if (values.Length != 6)
            {
                throw new FormatException("Invalid candlestick format. Expected: Timestamp,Open,High,Low,Close,Volume"); // Throw exception otherwise
            }

            Data = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture); // Parsing the timestamp
            Open = Math.Round(decimal.Parse(values[1]), 2); // Parsing and rounding the open price
            High = Math.Round(decimal.Parse(values[2]), 2); // Parsing and rounding the high price
            Low = Math.Round(decimal.Parse(values[3]), 2); // Parsing and rounding the low price
            Close = Math.Round(decimal.Parse(values[4]), 2); // Parsing and rounding the close price
            Volume = ulong.Parse(values[5]); // Parsing the volume
        }

        /// <summary>
        /// Converts the CandleStick object into a string representation in CSV format.
        /// </summary>
        /// <returns>A CSV formatted string with the values of the CandleStick.</returns>
        public override string ToString()
        {
            // Return the object as a formatted string
            return $"{Data:yyyy-MM-dd HH:mm:ss},{Open},{High},{Low},{Close},{Volume}";
        }

        public bool IsBearishEngulfing()
        {
            return this.Close < this.Open &&
                   (this.High - this.Low) > (this.Open - this.Close) * (decimal)1.5;
        }

        public bool IsBullishEngulfing()
        {
            return this.Close > this.Open &&
                   (this.High - this.Low) > (this.Close - this.Open) * (decimal)1.5;
        }

        public bool IsNearPrice(decimal price, decimal tolerance)
        {
            return Math.Abs(High - price) <= tolerance ||
                   Math.Abs(Low - price) <= tolerance ||
                   Math.Abs(Open - price) <= tolerance ||
                   Math.Abs(Close - price) <= tolerance;
        }
    }
}
