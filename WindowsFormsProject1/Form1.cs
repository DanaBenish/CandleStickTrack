using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsProject1
{
    public partial class Form1 : Form
    {
        // A private variable to hold the list of candlesticks
        private List<CandleStick> candlesticks;

        public Form1()
        {
            InitializeComponent(); // Initializes all the components of in the Form
        }

        /// <summary>
        /// Handles the click event for the Load Ticker button.
        /// Opens a file dialog to allow the user to select a CSV file containing candlestick data.
        /// </summary>
        /// <param name="sender">Load Ticker</param>
        /// <param name="e"></param>
        private void button_LoadTicker_Click(object sender, EventArgs e)
        {
            string stockSymbol = textBox_Symbol.Text.Trim(); // Getting the input symbol
            string candlePeriod = comboBox_Period.SelectedItem.ToString(); // Getting the input period

            string fileName = $"{stockSymbol}-{candlePeriod}.csv"; // Creating file name for dialog

            openFileDialog_LoadTicker.FileName = fileName; // Setting file name


            openFileDialog_LoadTicker.ShowDialog(this); // Shows the OpenFileDialog for csv file selection
        }

        /// <summary>
        /// Handles the FileOk event of the OpenFileDialog when the user selects a file and opens it.
        /// Loads the candlestick data from the selected file, filters it based on the selected start date,
        /// and then displays the candlestick chart.
        /// </summary>
        /// <param name="sender">OpenFileDialog</param>
        /// <param name="e">Event data for the FileOk event</param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Text = openFileDialog_LoadTicker.FileName; // Set the form's title to the selected file path/FileName

            List<CandleStick> listOfCandlesticks = loadTicker(openFileDialog_LoadTicker.FileName); // Load candlestick data from the selected file
            candlesticks = listOfCandlesticks; // Store the loaded data in the candlesticks list
            listOfCandlesticks = FilterCandleSticksByDate(candlesticks, dateTimePicker_StartDate.Value); // Filter candlesticks based on the selected start date

            displayChart(listOfCandlesticks); // Display the filtered candlestick chart

        }

        /// <summary>
        /// Loads candlestick data from the CSV file given by the file path/FileName.
        /// Uses the <see cref="StockReader"/> class to read the file and returns a list of <see cref="CandleStick"/> objects.
        /// </summary>
        /// <param name="filePath">The path of the CSV file containing the selected candlestick information.</param>
        /// <returns>A list of <see cref="CandleStick"/> objects representing the candlestick data loaded from the selected csv file.</returns>
        public List<CandleStick> loadTicker(string filePath)
        {
            StockReader reader = new StockReader(); // Create an instance of StockReader to read the selected file
            return reader.ReadCandleStickFromCsv(filePath); // Return the list of CandleStick objects from the CSV file using StockReader class
        }

        /// <summary>
        /// Filters the given list of candlesticks to only include dates greater or equal to the specified start date.
        /// Date is specified by the user or default setting
        /// </summary>
        /// <param name="candlesticks">A list of <see cref="CandleStick"/> objects to be filtered.</param>
        /// <param name="startDate">The start date that the candlesticks must be greater or equal to.</param>
        /// <returns>A filtered list of <see cref="CandleStick"/> objects where the <see cref="CandleStick.Data"/> is greater than or equal to the specified <paramref name="startDate"/>.</returns>
        private List<CandleStick> FilterCandleSticksByDate(List<CandleStick> candlesticks, DateTime startDate)
        {
            // Filter the candlesticks and return only those whose Data is greater than or equal to the startDate
            return candlesticks.Where(c => c.Data >= startDate).ToList(); 
        }

        /// <summary>
        /// Normalizes the Y-axis range of the chart by adjusting it based on the maximum and minimum values of the candlestick data.
        /// It adds a 2% margin above the maximum high and subtracts a 2% margin below the minimum low for better visibility of the chart data.
        /// </summary>
        /// <param name="candlesticks">A list of <see cref="CandleStick"/> objects that contains the data used for determining the range</param>
        private void normalize(List<CandleStick> candlesticks)
        { 
            decimal maxHigh = candlesticks.Max(c => c.High); // Find the maximum high
            decimal minLow = candlesticks.Min(c => c.Low); // Find the maximum low

            decimal maxMargin = maxHigh * 1.02m;  // Calculate and add 2% margin for max
            decimal minMargin = minLow * 0.98m;   // Calculate and subtracting 2% margin for max

            chart_Stock.ChartAreas[0].AxisY.Minimum = (double)minMargin; // Set the Y-axis minimum
            chart_Stock.ChartAreas[0].AxisY.Maximum = (double)maxMargin; // Set the Y-axis maximum
        }

        /// <summary>
        /// Displays the candlestick and a volume charts on the chart control using the provided list of candlesticks.
        /// This function binds the candlesticks to a DataGridView and updates the chart with OHLC and Volume data.
        /// </summary>
        /// <param name="candlesticks">A list of CandleStick objects that represent stock data for charting.</param>
        private void displayChart(List<CandleStick> candlesticks)
        {
            dataGridView_StockData.DataSource = null; // Clear any previous data
            dataGridView_StockData.DataSource = candlesticks; // Bind the candlestick list to the DataGridView

            chart_Stock.Series.Clear(); // Clear existing series when reloading
            chart_Stock.ChartAreas.Clear(); // Clear existing chart areas when reloading

            // Create two chart areas for OHLC and one for volume
            var ohlChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("OHLC"); // Create chart area for OHLC
            var volumeChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("Volume"); // Create chart area for Volume
            chart_Stock.ChartAreas.Add(ohlChartArea); // Add OHLC chart to the chartAreas property
            chart_Stock.ChartAreas.Add(volumeChartArea); // Add Volume chart to the chartAreas property

            // Create Series for OHLC
            var ohlcSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Candlestick")
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick, // Specifies that the chart type for this series will be Candlestick
                XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime, // // Specifies that the X-values for this series will be dates 
                YValuesPerPoint = 4 // Candlestick requires 4 values: Open, High, Low, Close
            };

            // Add data points to OHLC and apply colors
            foreach (var candle in candlesticks)
            {
                var point = new System.Windows.Forms.DataVisualization.Charting.DataPoint // Create a new data point for the chart
                {
                    XValue = candle.Data.ToOADate(),  // Convert DateTime to the OLE Automation that is used by charts
                    YValues = new double[] { (double)candle.High, (double)candle.Low, (double)candle.Open, (double)candle.Close } // Set the y values to the Open, High, Low, and Close prices of the candlestick
                };

                // Apply Green for price increase, red for price decrease
                if (candle.Close >= candle.Open) // If close price is bigger than open - green
                {
                    point.Color = System.Drawing.Color.Green;  // Bullish - green
                }
                else // If close price is smaller than open - Red
                {
                    point.Color = System.Drawing.Color.Red;  // Bearish - red
                }

                ohlcSeries.Points.Add(point); // Add the data point to the OHLC series for plotting on the chart
            }

            ohlcSeries.ChartArea = "OHLC"; // Add the OHLC series to the OHLC chart area
            chart_Stock.Series.Add(ohlcSeries); // Add the OHLC series to the chart to display it on the chart

            // Create Series for Volume
            var volumeSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Volume")
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column, // Set the chart type for the Volume series to Column chart
                XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime, // Specify that the X axis values for Volume will be dates
                YValuesPerPoint = 1 // Set the Ya xis to hold a single value
            };

            // Add data points for Volume
            foreach (var candle in candlesticks)
            {
                volumeSeries.Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint // Add a new data point to the volume series
                {
                    XValue = candle.Data.ToOADate(),  // Convert DateTime to OLE Automation
                    YValues = new double[] { (double)candle.Volume }  // Set Y values as Volume data
                });
            }

            volumeSeries.ChartArea = "Volume"; // Assign the Volume series to the Volume chart area
            chart_Stock.Series.Add(volumeSeries); // Add the Volume series to the chart to display the volume data

            chart_Stock.ChartAreas["OHLC"].AxisY.LabelStyle.Format = "{0:C}"; // Format OHLC Y-axis as currency
            chart_Stock.ChartAreas["Volume"].AxisY.LabelStyle.Format = "{0:N0}"; // Format Volume Y-axis as a number

            // Normalize Y-axis for OHLC candlestick chart
            normalize(candlesticks);

            // Refresh the chart to reflect all changes
            chart_Stock.Invalidate();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // Your code here
        }
    }
}
