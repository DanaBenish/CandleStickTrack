using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsProject1
{
    public class PeakVally
    {
        public CandleStick Peak { get; set; } // The peak candlestick
        public CandleStick Valley { get; set; } // The valley candlestick
        public int LeftRange { get; set; } // The left margin (range)
        public int RightRange { get; set; } // The right margin (range)

        /// <summary>
        /// Constructor to create a PeakVally object, which represents a peak and a valley 
        /// with their respective left and right range values.
        /// </summary>
        /// <param name="peak">The peak candle stick object.</param>
        /// <param name="valley">The valley candle stick object.</param>
        /// <param name="leftRange">The left range margin for peak/valley identification.</param>
        /// <param name="rightRange">The right range margin for peak/valley identification.</param>
        public PeakVally(CandleStick peak, CandleStick valley, int leftRange, int rightRange)
        {
            Peak = peak;
            Valley = valley;
            LeftRange = leftRange;
            RightRange = rightRange;
        }

        /// <summary>
        /// Detects peaks and valleys in the provided list of candlesticks and immediately adds annotations to the chart
        /// when peaks and valleys are found. The annotations highlight the points of interest.
        /// </summary>
        /// <param name="candlesticks">The list of candlestick data.</param>
        /// <param name="margin">The margin or range used to identify peaks and valleys.</param>
        /// <param name="chart">The chart where annotations are added.</param>
        /// 

        public static void DetectAndAnnotatePeaksAndValleys(List<CandleStick> candlesticks, int margin, Chart chart)
        {
            // Traverse through the candlesticks to find peaks and valleys
            for (int i = margin; i < candlesticks.Count - margin; i++)
            {
                bool isPeak = true;
                bool isValley = true;

                // Check if it's a peak (higher than surrounding candles within the margin range)
                for (int j = 1; j <= margin; j++)
                {
                    if (candlesticks[i].High <= candlesticks[i - j].High || candlesticks[i].High <= candlesticks[i + j].High)
                    {
                        isPeak = false;
                        break;
                    }
                }

                // Check if it's a valley (lower than surrounding candles within the margin range)
                for (int j = 1; j <= margin; j++)
                {
                    if (candlesticks[i].Low >= candlesticks[i - j].Low || candlesticks[i].Low >= candlesticks[i + j].Low)
                    {
                        isValley = false;
                        break;
                    }
                }

                // If it's a peak or a valley, create a PeakValley object and add annotations
                if (isPeak)
                {
                    // Annotate Peak
                    AddAnnotationToChart(candlesticks[i], "P", Color.Red, chart, (double)candlesticks[i].High);
                }

                if (isValley)
                {
                    // Annotate Valley
                    AddAnnotationToChart(candlesticks[i], "V", Color.Green, chart, (double)candlesticks[i].Low);
                }
            }
        }

        /// <summary>
        /// Method to add annotations (both text and callout annotations) to the chart to indicate the detected peaks and valleys.
        /// </summary>
        /// <param name="candle">The candlestick object representing the peak or valley.</param>
        /// <param name="text">The annotation text ("P" for peak, "V" for valley).</param>
        /// <param name="color">The color of the annotation text and line.</param>
        /// <param name="chart">The chart object to which the annotations will be added.</param>
        /// <param name="price">The price (High for peaks, Low for valleys) where the annotation is to be placed.</param>
        public static void AddAnnotationToChart(CandleStick candle, string text, Color color, Chart chart, double price)
        {
            // Check if the candle is not null before proceeding with annotation
            if (candle != null)
            {
                // Create a DataPoint that will be used for annotation
                var point = new DataPoint
                {
                    // Set the X value using the OLE Automation Date (a format that can be used in charts)
                    XValue = candle.Data.ToOADate(),  // Use OLE Automation Date for the X value

                    // Set the Y value to either the high or low of the candle depending on the annotation type (Peak or Valley)
                    YValues = new double[] { text == "P" ? (double)candle.High : (double)candle.Low }
                };

                // Create a TextAnnotation to display text (P or V) at the peak or valley
                var annotation = new TextAnnotation
                {
                    Text = text,  // Set the text to either "P" for peak or "V" for valley
                    ForeColor = color,  // Set the color of the text for visibility
                    AnchorX = point.XValue,  // X position for the annotation (from the DataPoint)
                    AnchorY = point.YValues[0],  // Y position for the annotation (from the DataPoint's Y value)
                    Font = new Font("Arial", 6),  // Set the font style and size for the annotation text
                    Alignment = ContentAlignment.MiddleCenter,  // Align the text to be in the center of the annotation point
                    AxisX = chart.ChartAreas["OHLC"].AxisX,  // Bind the annotation to the X axis of the chart
                    AxisY = chart.ChartAreas["OHLC"].AxisY  // Bind the annotation to the Y axis of the chart
                };

                // Create a CalloutAnnotation to visually connect the annotation text with the chart using a line
                var calloutAnnotation = new CalloutAnnotation
                {
                    Text = text,  // Set the text (P or V) for the annotation
                    ForeColor = color,  // Set the color of the callout text
                    Font = new Font("Arial", 5),  // Set the font style and size for the callout text
                    AnchorX = point.XValue,  // Set the X position for the callout text (from the DataPoint)
                    AnchorY = point.YValues[0],  // Set the Y position for the callout text (from the DataPoint)
                    Alignment = ContentAlignment.MiddleCenter,  // Center align the text within the callout
                    LineColor = color,  // Set the color of the callout line
                    LineWidth = 1,  // Set the width of the callout line
                    AxisX = chart.ChartAreas["OHLC"].AxisX,  // Associate the callout with the X axis of the chart
                    AxisY = chart.ChartAreas["OHLC"].AxisY  // Associate the callout with the Y axis of the chart
                };

                // Add the created callout annotation to the chart
                chart.Annotations.Add(calloutAnnotation);
            }

        }

        /// <summary>
        /// Detects the peaks and valleys in a list of candlesticks, and returns a list of PeakValley objects 
        /// representing the detected peaks and valleys.
        /// </summary>
        /// <param name="candlesticks">The list of candlestick data to search for peaks and valleys.</param>
        /// <param name="margin">The margin used to identify peaks and valleys based on surrounding data points.</param>
        /// <returns>A list of PeakVally objects representing the detected peaks and valleys.</returns>
        public static List<PeakVally> DetectPeaksAndValleysList(List<CandleStick> candlesticks, int margin)
        {
            // Create an empty list to store the detected peaks and valleys
            List<PeakVally> peakValleys = new List<PeakVally>();

            // Loop through the candlesticks starting from the margin until the end of the list, excluding the last margin elements
            for (int i = margin; i < candlesticks.Count - margin; i++)
            {
                // Initialize flags to check whether the current candlestick is a peak or valley
                bool isPeak = true;
                bool isValley = true;

                // Check if it's a peak by comparing the current candlestick's high value to its neighboring candlesticks
                for (int j = 1; j <= margin; j++)
                {
                    // If the current candlestick's high is less than or equal to the neighboring candlesticks' highs, it's not a peak
                    if (candlesticks[i].High <= candlesticks[i - j].High || candlesticks[i].High <= candlesticks[i + j].High)
                    {
                        // Set isPeak to false and exit the loop
                        isPeak = false;
                        break;
                    }
                }

                // Check if it's a valley by comparing the current candlestick's low value to its neighboring candlesticks
                for (int j = 1; j <= margin; j++)
                {
                    // If the current candlestick's low is greater than or equal to the neighboring candlesticks' lows, it's not a valley
                    if (candlesticks[i].Low >= candlesticks[i - j].Low || candlesticks[i].Low >= candlesticks[i + j].Low)
                    {
                        // Set isValley to false and exit the loop
                        isValley = false;
                        break;
                    }
                }

                // If the current candlestick is identified as a peak, add it to the list of detected peaks and valleys
                if (isPeak)
                {
                    // Store the current peak candlestick
                    var peak = candlesticks[i];

                    // Add a new PeakVally object to the list, where Peak is the current peak and Valley is null
                    peakValleys.Add(new PeakVally(peak, null, 0, 0));
                }

                // If the current candlestick is identified as a valley, add it to the list of detected peaks and valleys
                if (isValley)
                {
                    // Store the current valley candlestick
                    var valley = candlesticks[i];

                    // Add a new PeakVally object to the list, where Valley is the current valley and Peak is null
                    peakValleys.Add(new PeakVally(null, valley, 0, 0));
                }
            }

            // Return the list of detected peaks and valleys
            return peakValleys;

        }
    }
}

