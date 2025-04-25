using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using projict2;
using WindowsFormsProject1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Priject2
{
    public partial class Form_Display : Form
    {
        // Declare a string to store the file path where data might be loaded or saved
        public string filePath;

        // Declare a list to hold the candlestick data used for plotting on the chart
        List<CandleStick> candlesticks;

        // Declare a StockReader object to handle reading stock data from a file or other source
        StockReader stockreader;

        // Declare an integer to store the current value of the horizontal scroll bar
        private int scrollBarValue = 1;

        // Declare a list to store waves that represent upward trends (upWaves)
        private List<Wave> upWaves = new List<Wave>();

        // Declare a list to store waves that represent downward trends (downWaves)
        private List<Wave> downWaves = new List<Wave>();

        // Declare a list to hold Peak and Valley information, typically for identifying turning points in the data
        private List<PeakVally> peakValleys;


        private bool isDragging = false;
        private Point dragStartPoint;
        private Point dragEndPoint;

        private List<CandleStick> allCandles;
        private List<PeakVally> peakValleysN;


        private WaveSelection _waveSelection;
        private bool _isDraggingWave = false;

        private bool isSimulating = false;
        private decimal currentStep = 0.005m; // Default step size (0.5% of wave height)
        private Wave selectedWaveFromCombo = null;


        public Form_Display()
        {
            InitializeComponent();
        }

        public Form_Display(String originalFilePath, DateTime startDate, DateTime endDate)
        {
            InitializeComponent(); // Initialize

            stockreader = new StockReader(); // Set new stock reader

            filePath = originalFilePath; // Set file path
            dateTimePicker_EndDate.Value = endDate; // Set end date
            dateTimePicker_StartDate.Value = startDate; // set start date

            List<CandleStick> listOfCandlesticks = loadTicker(filePath); // Load candlestick data from the selected file
            candlesticks = listOfCandlesticks; // Store the loaded data in the candlesticks list
            listOfCandlesticks = FilterCandleSticksByDate(candlesticks, startDate, endDate); // Filter candlesticks based on the selected start date

            peakValleys = PeakVally.DetectPeaksAndValleysList(candlesticks, scrollBarValue); // Get PeakValleys

            displayChart(listOfCandlesticks); // Display the filtered candlestick chart

            _waveSelection = new WaveSelection();

            //Text = originalFilePath;
            Show(); // Show Form
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
        private List<CandleStick> FilterCandleSticksByDate(List<CandleStick> candlesticks, DateTime startDate, DateTime endDate)
        {
            // Filter the candlesticks and return only those whose Data is greater than or equal to the startDate
            return candlesticks.Where(c => c.Data >= startDate && c.Data <= endDate).ToList();
        }

        private List<Wave> FilterWavesByDate(List<Wave> waves, DateTime startDate, DateTime endDate)
        {
            // Filter the waves and return only those whose StartDate or EndDate is within the range
            return waves.Where(w => w.StartDate >= startDate && w.EndDate <= endDate).ToList();
        }

        /// <summary>
        /// Normalizes the Y-axis range of the chart by adjusting it based on the maximum and minimum values of the candlestick data.
        /// It adds a 2% margin above the maximum high and subtracts a 2% margin below the minimum low for better visibility of the chart data.
        /// </summary>
        /// <param name="candlesticks">A list of <see cref="CandleStick"/> objects that contains the data used for determining the range</param>
        private void normalize(List<CandleStick> candlesticks)
        {
            if (candlesticks.Any()) // Check if there are any candlesticks
            {
                decimal maxHigh = candlesticks.Max(c => c.High); // Find the maximum high
                decimal minLow = candlesticks.Min(c => c.Low);   // Find the minimum low

                decimal maxMargin = maxHigh * 1.02m;  // Calculate and add 2% margin for max
                decimal minMargin = minLow * 0.98m;   // Calculate and subtract 2% margin for max

                chart_Stock.ChartAreas[0].AxisY.Minimum = (double)minMargin; // Set the Y-axis minimum
                chart_Stock.ChartAreas[0].AxisY.Maximum = (double)maxMargin; // Set the Y-axis maximum
            }
            else
            {
                // Set default values if there are no candlesticks
                chart_Stock.ChartAreas[0].AxisY.Minimum = 0;
                chart_Stock.ChartAreas[0].AxisY.Maximum = 100;
            }
        }

        /// <summary>
        /// Displays the candlestick and a volume charts on the chart control using the provided list of candlesticks.
        /// This function binds the candlesticks to a DataGridView and updates the chart with OHLC and Volume data.
        /// </summary>
        /// <param name="candlesticks">A list of CandleStick objects that represent stock data for charting.</param>
        private void displayChart(List<CandleStick> candlesticks)
        {
            chart_Stock.Annotations.Clear(); // Clear old annotations
            int margin = scrollBarValue; // Set margin

            chart_Stock.Series.Clear(); // Clear existing series when reloading
            chart_Stock.ChartAreas.Clear(); // Clear existing chart areas when reloading

            // Create two chart areas for OHLC and one for volume
            var ohlChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("OHLC");
            var volumeChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("Volume");
            chart_Stock.ChartAreas.Add(ohlChartArea);
            chart_Stock.ChartAreas.Add(volumeChartArea);

            // Create Series for OHLC
            var ohlcSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Candlestick")
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick,
                XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime,
                YValuesPerPoint = 4
            };

            // Add data points to OHLC and apply colors
            foreach (var candle in candlesticks)
            {
                var point = new System.Windows.Forms.DataVisualization.Charting.DataPoint
                {
                    XValue = candle.Data.ToOADate(),  // Convert DateTime to OLE Automation
                    YValues = new double[] { (double)candle.High, (double)candle.Low, (double)candle.Open, (double)candle.Close }
                };

                if (candle.Close >= candle.Open) // If close price is bigger than open - green
                {
                    point.Color = System.Drawing.Color.Green;  // Bullish - green
                }
                else
                {
                    point.Color = System.Drawing.Color.Red;  // Bearish - red
                }

                //Text = $"{point.XValue}";
                ohlcSeries.Points.Add(point); // Add the data point to the OHLC series
            }

            // Set the ChartArea for the OHLC (Open, High, Low, Close) series to the "OHLC" chart area
            ohlcSeries.ChartArea = "OHLC";

            // Add the OHLC series to the chart's series collection
            chart_Stock.Series.Add(ohlcSeries);

            // Create a new Series for the volume chart (this will display volume as columns)
            var volumeSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Volume")
            {
                // Set the chart type to a column chart to represent volume
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column,

                // Set X values as DateTime, since volume is tied to specific dates
                XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime,

                // Set the number of Y-values per point to 1 (as each volume point has a single value)
                YValuesPerPoint = 1
            };

            // Loop through each candlestick to add data points to the volume series
            foreach (var candle in candlesticks)
            {
                // Add a new data point to the volume series
                volumeSeries.Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint
                {
                    // Set the X value (date) of the data point
                    XValue = candle.Data.ToOADate(),  // Convert DateTime to OLE Automation Date

                    // Set the Y value (volume) of the data point
                    YValues = new double[] { (double)candle.Volume }  // Convert volume to double
                });
            }

            // Set the ChartArea for the volume series to the "Volume" chart area
            volumeSeries.ChartArea = "Volume";

            // Add the volume series to the chart's series collection
            chart_Stock.Series.Add(volumeSeries);

            // Set the format for the Y-axis of the OHLC chart area (display prices as currency)
            chart_Stock.ChartAreas["OHLC"].AxisY.LabelStyle.Format = "{0:C}";

            // Set the format for the Y-axis of the Volume chart area (display volume as number without decimals)
            chart_Stock.ChartAreas["Volume"].AxisY.LabelStyle.Format = "{0:N0}";

            // Call a method to detect peaks and valleys in the candlestick data and annotate them on the chart
            PeakVally.DetectAndAnnotatePeaksAndValleys(candlesticks, margin, chart_Stock);

            // Call the method to generate waves and populate the combo boxes with the peak-valley data
            GenerateWavesAndPopulateCombos(peakValleys);  // Pass in your list of peak-valley objects

            // Normalize the Y-axis values for the OHLC chart to ensure proper scaling
            normalize(candlesticks); // Normalize Y-axis for OHLC candlestick chart

            // Refresh the chart to reflect all changes, including new series, annotations, and formatting
            chart_Stock.Invalidate();

        }

        /// <summary>
        /// This method handles the event when the horizontal scrollbar (hScrollBar1) is scrolled.
        /// It updates the scrollBarValue based on the current value of the scrollbar.
        /// </summary>
        /// <param name="sender">The object that raised the event (the horizontal scrollbar).</param>
        /// <param name="e">The event arguments related to the scroll event.</param>
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            // Update the scrollBarValue with the current value of the horizontal scrollbar
            scrollBarValue = hScrollBar1.Value;
            DateTime startDate = dateTimePicker_StartDate.Value;
            DateTime endDate = dateTimePicker_EndDate.Value;

            // Call the method to refresh the chart using the selected start and end dates
            RefreshChartWithSelectedDates(startDate, endDate);
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            scrollBarValue = hScrollBar1.Value;
            DateTime startDate = dateTimePicker_StartDate.Value;
            DateTime endDate = dateTimePicker_EndDate.Value;

            // Call the method to refresh the chart using the selected start and end dates
            RefreshChartWithSelectedDates(startDate, endDate);
        }

        /// <summary>
        /// This method handles the click event for the Refresh button.
        /// It retrieves the start and end dates selected by the user and refreshes the chart with the new data.
        /// </summary>
        /// <param name="sender">The object that raised the event (the Refresh button).</param>
        /// <param name="e">The event arguments related to the click event.</param>
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            // Get the selected start and end dates from the date time pickers
            DateTime startDate = dateTimePicker_StartDate.Value;
            DateTime endDate = dateTimePicker_EndDate.Value;

            // Call the method to refresh the chart using the selected start and end dates
            RefreshChartWithSelectedDates(startDate, endDate);
        }

        /// <summary>
        /// This method is responsible for handling the actual data refresh logic.
        /// It clears the previous data on the chart, filters the candlesticks by the selected date range, 
        /// and re-renders the chart with the updated data.
        /// </summary>
        /// <param name="startDate">The start date for filtering the data.</param>
        /// <param name="endDate">The end date for filtering the data.</param>
        private void RefreshChartWithSelectedDates(DateTime startDate, DateTime endDate)
        {
            // Clear previous data from the chart (series, chart areas, and annotations)
            chart_Stock.Series.Clear();
            chart_Stock.ChartAreas.Clear();
            chart_Stock.Annotations.Clear();

            // Filter the candlesticks based on the selected start and end dates
            var filteredData = FilterCandleSticksByDate(candlesticks, startDate, endDate);

            // Detect and update peak and valley information with the new scrollBarValue
            peakValleys = PeakVally.DetectPeaksAndValleysList(candlesticks, hScrollBar1.Value);

            // Update the label to display the current margin of the scrollbar
            label_scrollbar.Text = "Current Margin: " + hScrollBar1.Value.ToString();

            // Reload the chart with the filtered candlestick data
            displayChart(filteredData);

            _waveSelection = new WaveSelection();
        }


        /// <summary>
        /// This method handles the event when the selected index of the UpWaves ComboBox changes.
        /// It checks if a valid selection has been made and displays the selected wave using a green color (indicating an upward wave).
        /// </summary>
        /// <param name="sender">The object that raised the event (the ComboBox).</param>
        /// <param name="e">The event arguments.</param>
        private void comboBox_UpWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_UpWaves.SelectedIndex >= 0)
            {
                // Keep existing display code
                selectedWaveFromCombo = upWaves[comboBox_UpWaves.SelectedIndex];
                DisplaySelectedWave(new List<Wave> { selectedWaveFromCombo }, Color.Green);

                // Add initialization
                InitializeWaveSelectionForSimulation(selectedWaveFromCombo);
                ToggleSimulationControls(true);
            }
        }

        private void InitializeWaveSelectionForSimulation(Wave wave)
        {
            if (wave == null) return;

            _waveSelection = new WaveSelection();
            _waveSelection.InitializeFromWave(
                wave,
                candlesticks,
                chart_Stock.ChartAreas["OHLC"]);

            chart_Stock.Invalidate();
        }

        private void ToggleSimulationControls(bool enable)
        {
            button_startstop.Enabled = enable;
            button_Plus.Enabled = enable;
            button_Minus.Enabled = enable;

            if (!enable && isSimulating)
            {
                isSimulating = false;
                timer_Tick.Stop();
                button_startstop.Text = "Start";
            }
        }

        /// <summary>
        /// This method handles the event when the selected index of the DownWaves ComboBox changes.
        /// It checks if a valid selection has been made and displays the selected wave using a red color (indicating a downward wave).
        /// </summary>
        /// <param name="sender">The object that raised the event (the ComboBox).</param>
        /// <param name="e">The event arguments.</param>
        private void comboBox_DownWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_DownWaves.SelectedIndex >= 0)
            {
                // Keep existing display code
                selectedWaveFromCombo = downWaves[comboBox_DownWaves.SelectedIndex];
                DisplaySelectedWave(new List<Wave> { selectedWaveFromCombo }, Color.Red);

                // Add initialization
                InitializeWaveSelectionForSimulation(selectedWaveFromCombo);
                ToggleSimulationControls(true);
            }
        }


        /// <summary>
        /// This method generates a list of waves from the provided peak-valley data, filters them based on the selected date range,
        /// separates the waves into upward and downward waves, and populates the ComboBoxes with the wave's start and end dates.
        /// </summary>
        /// <param name="peakValleyList">A list of <see cref="PeakVally"/> objects representing the detected peaks and valleys from which waves will be generated.</param>
        private void GenerateWavesAndPopulateCombos(List<PeakVally> peakValleyList)
        {
            // Generate a list of waves from the given peak-valley data using the Wave.GetWaves method
            List<Wave> waves = Wave.GetWaves(peakValleyList);

            // Get the start date from the date picker control
            DateTime startDate = dateTimePicker_StartDate.Value;

            // Get the end date from the date picker control
            DateTime endDate = dateTimePicker_EndDate.Value;

            // Get the filtered list of waves based on the selected date range from the date pickers
            waves = FilterWavesByDate(waves, startDate, endDate);

            // Separate the waves into upward and downward waves using LINQ
            // "IsUpWave" property determines if it's an upward wave
            upWaves = waves.Where(w => w.IsUpWave).ToList();  // Store all upwaves in the upWaves list
            downWaves = waves.Where(w => !w.IsUpWave).ToList(); // Store all downwaves in the downWaves list

            // Clear previous items in both ComboBoxes before populating them with the new wave data
            comboBox_UpWaves.Items.Clear();  // Clear existing items in the UpWaves ComboBox
            comboBox_DownWaves.Items.Clear();  // Clear existing items in the DownWaves ComboBox

            // Loop through each upward wave and add its start and end dates to the UpWaves ComboBox
            foreach (var wave in upWaves)
            {
                // Add formatted start and end date as a string to the UpWaves ComboBox
                comboBox_UpWaves.Items.Add($"{wave.StartDate.ToShortDateString()} - {wave.EndDate.ToShortDateString()}");
            }

            // Loop through each downward wave and add its start and end dates to the DownWaves ComboBox
            foreach (var wave in downWaves)
            {
                // Add formatted start and end date as a string to the DownWaves ComboBox
                comboBox_DownWaves.Items.Add($"{wave.StartDate.ToShortDateString()} - {wave.EndDate.ToShortDateString()}");
            }
        }


        /// <summary>
        /// This method adds annotations to the chart to display a selected wave as both a diagonal line and a highlighted rectangle.
        /// The wave is represented by the start and end date along with the start and end price. 
        /// The line and rectangle are drawn based on the selected wave's data.
        /// </summary>
        /// <param name="selectedWaves">A list of <see cref="Wave"/> objects to be displayed on the chart.</param>
        /// <param name="waveColor">The color to be used for the line and rectangle representing the wave (e.g., green for upwave, red for downwave).</param>
        private void DisplaySelectedWave(List<Wave> selectedWaves, Color waveColor)
        {
            // Loop through each wave in the selectedWaves list
            foreach (var wave in selectedWaves)
            {
                // Get the start date of the current wave
                var startDate = wave.StartDate;

                // Get the end date of the current wave
                var endDate = wave.EndDate;

                // Get the start price of the current wave
                var startPrice = wave.StartPrice;

                // Get the end price of the current wave
                var endPrice = wave.EndPrice;

                // Create a new LineAnnotation to represent the wave as a diagonal line
                var lineAnnotation = new LineAnnotation
                {
                    // Set the X position (start date of the wave)
                    X = startDate.ToOADate(),  // Convert the DateTime startDate to OLE Automation Date format

                    // Set the Y position (start price of the wave)
                    Y = (double)startPrice,     // Convert the start price to double for Y position

                    // Set the width of the line based on the time span of the wave (end date - start date)
                    Width = endDate.ToOADate() - startDate.ToOADate(), // Calculate width based on start and end date difference

                    // Set the height of the line based on the price difference (end price - start price)
                    Height = (double)(endPrice - startPrice), // Calculate height based on price difference

                    // Set the color of the line according to the wave color passed in the parameter
                    LineColor = waveColor,     // Set the line color for this wave (green for upwave, red for downwave)

                    // Set the line width to 2 pixels
                    LineWidth = 2,             // Line width of 2 pixels for visibility

                    // Set the line style to solid
                    LineDashStyle = ChartDashStyle.Solid, // Use a solid line style for the wave

                    // Set the X and Y axes for positioning the annotation within the chart area
                    AxisX = chart_Stock.ChartAreas["OHLC"].AxisX, // Assign the X axis from the "OHLC" chart area
                    AxisY = chart_Stock.ChartAreas["OHLC"].AxisY, // Assign the Y axis from the "OHLC" chart area

                    // Ensure the size of the annotation is not relative, to keep it consistent on all screen sizes
                    IsSizeAlwaysRelative = false
                };

                // Add the created line annotation to the chart to display it
                chart_Stock.Annotations.Add(lineAnnotation);

                // Create a new RectangleAnnotation to highlight the area of the wave
                var rectAnnotation = new RectangleAnnotation
                {
                    // Set the X and Y axes for positioning the rectangle within the chart area
                    AxisX = chart_Stock.ChartAreas["OHLC"].AxisX, // X axis from the "OHLC" chart area
                    AxisY = chart_Stock.ChartAreas["OHLC"].AxisY, // Y axis from the "OHLC" chart area

                    // Set the X position (start date of the wave)
                    X = startDate.ToOADate(),  // Convert the start date to OLE Automation Date format

                    // Set the Y position (start price of the wave)
                    Y = (double)startPrice,     // Convert the start price to double for Y position

                    // Set the width of the rectangle based on the time span (end date - start date)
                    Width = endDate.ToOADate() - startDate.ToOADate(), // Calculate the width of the rectangle

                    // Set the height of the rectangle based on the price difference (end price - start price)
                    Height = (double)(endPrice - startPrice), // Calculate the height of the rectangle based on price difference

                    // Set the color of the rectangle's border to transparent (no visible border)
                    LineColor = Color.Transparent, // Transparent border color

                    // Set the background color of the rectangle using a semi-transparent color (with wave color)
                    BackColor = Color.FromArgb(30, waveColor), // Semi-transparent background color based on wave color

                    // Ensure the size of the rectangle is not relative to any screen size settings
                    IsSizeAlwaysRelative = false
                };

                // Add the created rectangle annotation to the chart
                chart_Stock.Annotations.Add(rectAnnotation);
            }

            // Refresh the chart to display the newly added annotations (line and rectangle)
            chart_Stock.Invalidate(); // Refresh the chart to reflect changes made by adding annotations
        }

        // Handles mouse down events on the stock chart
        private void chart_Stock_MouseDown(object sender, MouseEventArgs e)
        {
            // Only respond to left mouse button clicks
            if (e.Button == MouseButtons.Left)
            {
                // Convert mouse X coordinate to date value
                var date = DateTime.FromOADate(chart_Stock.ChartAreas["OHLC"].AxisX.PixelPositionToValue(e.X));
                // Convert mouse Y coordinate to price value
                var price = (decimal)chart_Stock.ChartAreas["OHLC"].AxisY.PixelPositionToValue(e.Y);

                // Find the nearest peak or valley to the clicked position
                var peakValley = FindNearestPeakOrValley(date, price);
                if (peakValley != null)
                {
                    // Create new wave selection starting at the found peak/valley
                    _waveSelection = new WaveSelection();
                    _waveSelection.StartSelection(peakValley);
                    _isDraggingWave = true; // Begin drag operation
                }
            }
        }

        // Handles mouse movement on the stock chart
        private void chart_Stock_MouseMove(object sender, MouseEventArgs e)
        {
            // Only process if we're actively dragging a wave selection
            if (_isDraggingWave && _waveSelection != null)
            {
                // Update the wave's endpoint with current mouse position
                _waveSelection.UpdateEndPoint(
                    e.Location,             // Current mouse position
                    candlesticks,           // List of all candlesticks
                    chart_Stock.ChartAreas["OHLC"]); // Reference to chart area
                                                     // Force the chart to redraw
                chart_Stock.Invalidate();
            }
        }

        // Handles mouse up events on the stock chart
        private void chart_Stock_MouseUp(object sender, MouseEventArgs e)
        {
            // End drag operation
            _isDraggingWave = false;
            // Note: Selection remains visible after mouse up
        }

        // Handles painting of the stock chart
        private void chart_Stock_Paint(object sender, PaintEventArgs e)
        {
            // Existing paint code...

            // If we have an active wave selection, draw it
            if (_waveSelection != null && _waveSelection.IsActive)
            {
                _waveSelection.Draw(e.Graphics, chart_Stock.ChartAreas["OHLC"]);
            }
        }

        // Finds the nearest peak or valley to the specified date and price
        private CandleStick FindNearestPeakOrValley(DateTime date, decimal price)
        {
            const double maxDaysDiff = 2; // Maximum 2 days apart to be considered "near"
            const decimal priceTolerance = 0.02m; // 2% price difference tolerance

            // Check all known peaks and valleys
            foreach (var pv in peakValleys)
            {
                // Check peaks
                if (pv.Peak != null &&
                    Math.Abs((pv.Peak.Data - date).TotalDays) <= maxDaysDiff && // Within date range
                    (Math.Abs(pv.Peak.High - price) <= priceTolerance * pv.Peak.High || // Price near high
                     Math.Abs(pv.Peak.Close - price) <= priceTolerance * pv.Peak.Close)) // Or price near close
                {
                    return pv.Peak;
                }

                // Check valleys
                if (pv.Valley != null &&
                    Math.Abs((pv.Valley.Data - date).TotalDays) <= maxDaysDiff && // Within date range
                    (Math.Abs(pv.Valley.Low - price) <= priceTolerance * pv.Valley.Low || // Price near low
                     Math.Abs(pv.Valley.Close - price) <= priceTolerance * pv.Valley.Close)) // Or price near close
                {
                    return pv.Valley;
                }
            }
            return null; // No nearby peak or valley found
        }

        // Handles the plus button click (expands the wave)
        private void button_Plus_Click(object sender, EventArgs e)
        {
            if (_waveSelection != null)
            {
                // Adjust wave endpoint (negative step for expansion)
                _waveSelection.AdjustEndPrice(
                    -currentStep,           // Negative value expands the wave
                    candlesticks,           // Current candle data
                    chart_Stock.ChartAreas["OHLC"]); // Chart reference
                chart_Stock.Invalidate();   // Update display
            };
        }

        // Handles the minus button click (contracts the wave)
        private void button_Minus_Click(object sender, EventArgs e)
        {
            if (_waveSelection != null)
            {
                // Adjust wave endpoint (positive step for contraction)
                _waveSelection.AdjustEndPrice(
                    currentStep,            // Positive value contracts the wave
                    candlesticks,           // Current candle data
                    chart_Stock.ChartAreas["OHLC"]); // Chart reference
                chart_Stock.Invalidate();   // Update display
            }
        }

        // Handles the start/stop simulation button click
        private void button3_Click(object sender, EventArgs e)
        {
            // Toggle simulation state
            isSimulating = !isSimulating;
            // Update button text
            button_startstop.Text = isSimulating ? "Stop" : "Start";
            // Enable/disable manual controls
            button_Plus.Enabled = !isSimulating;
            button_Minus.Enabled = !isSimulating;

            // Start/stop the simulation timer
            timer_Tick.Enabled = isSimulating;
        }

        // Calculates the current step size based on scrollbar position
        private decimal GetCurrentStepSize()
        {
            // Convert trackbar value (1-20) to percentage (0.1%-2%)
            return hScrollBar_Steps.Value * 0.001m;
        }

        // Timer tick handler for automatic simulation
        private void timer_Tick_Tick(object sender, EventArgs e)
        {
            if (_waveSelection != null)
            {
                // Get current step size
                decimal adjustment = GetCurrentStepSize();
                // Adjust wave endpoint
                _waveSelection.AdjustEndPrice(adjustment, candlesticks, chart_Stock.ChartAreas["OHLC"]);
                // Update display
                chart_Stock.Invalidate();
            }
        }

        // Handles scrollbar changes for step size adjustment
        private void hScrollBar_Steps_Scroll(object sender, ScrollEventArgs e)
        {
            // Update current step size (convert to percentage)
            currentStep = hScrollBar_Steps.Value * 0.001m; // 1-20 => 0.1%-2%
        }
    }
}
