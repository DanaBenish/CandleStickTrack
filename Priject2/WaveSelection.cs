using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsProject1;
using Priject2;

namespace projict2
{
    public class WaveSelection
    {
        public CandleStick StartCandle { get; private set; }
        public PointF EndPoint { get; private set; }
        public bool IsActive { get; private set; }
        public List<decimal> FibonacciLevels { get; } = new List<decimal> { 100m, 76.4m, 61.8m, 50m, 38.2m, 23.6m, 0m };
        public List<string> Confirmations { get; } = new List<string>();
        public int ConfirmationCount => Confirmations.Count;

        private List<PointF> confirmationPoints = new List<PointF>();
        private ChartArea currentChartArea;



        public void StartSelection(CandleStick peakOrValley)
        {
            StartCandle = peakOrValley;
            IsActive = true;
            Confirmations.Clear();
        }

        public void UpdateEndPoint(PointF endPoint, List<CandleStick> allCandles, ChartArea chartArea)
        {
            if (!IsActive) return;

            EndPoint = endPoint;
            currentChartArea = chartArea; // Store for later use
            CalculateFibonacciLevels(chartArea);
            FindConfirmations(allCandles, chartArea); // Pass chartArea here
        }

        private void CalculateFibonacciLevels(ChartArea chartArea)
        {
            if (StartCandle == null || chartArea == null) return;

            // Convert screen Y to price value using the provided ChartArea
            double endPriceValue = chartArea.AxisY.PixelPositionToValue(EndPoint.Y);
            decimal endPrice = (decimal)endPriceValue;

            // Determine if we're working with a peak (high) or valley (low)
            bool isPeak = StartCandle.High > StartCandle.Low;
            decimal startPrice = isPeak ? StartCandle.High : StartCandle.Low;

            // Calculate price difference
            decimal priceDifference = endPrice - startPrice;

            // Calculate Fibonacci retracement levels
            FibonacciLevels.Clear();
            FibonacciLevels.Add(startPrice);                     // 100% (start)
            FibonacciLevels.Add(startPrice + priceDifference * 0.236m); // 23.6%
            FibonacciLevels.Add(startPrice + priceDifference * 0.382m); // 38.2%
            FibonacciLevels.Add(startPrice + priceDifference * 0.5m);   // 50%
            FibonacciLevels.Add(startPrice + priceDifference * 0.618m); // 61.8%
            FibonacciLevels.Add(startPrice + priceDifference * 0.764m); // 76.4%
            FibonacciLevels.Add(endPrice);                       // 0% (current)
        }

        private void FindConfirmations(List<CandleStick> allCandles, ChartArea chartArea)
        {
            Confirmations.Clear();
            confirmationPoints.Clear();

            if (StartCandle == null || FibonacciLevels.Count < 2 || chartArea == null) return;

            // 1. Convert screen X to date PROPERLY
            DateTime startDate = StartCandle.Data;
            DateTime endDate;
            try
            {
                // First convert screen X to axis value, then to date
                double endDateValue = chartArea.AxisX.PixelPositionToValue(EndPoint.X);
                endDate = DateTime.FromOADate(endDateValue);
            }
            catch
            {
                Confirmations.Add("Invalid date range");
                return;
            }

            // 2. Find candles in this time range (including edge cases)
            var candlesInRange = allCandles
                .Where(c => c.Data >= startDate && c.Data <= endDate)
                .OrderBy(c => c.Data)
                .ToList();

            // DEBUG: Show what we're working with
            Confirmations.Add($"Range: {startDate:HH:mm} to {endDate:HH:mm}");
            Confirmations.Add($"Candles found: {candlesInRange.Count}");

            if (!candlesInRange.Any())
            {
                Confirmations.Add("No candles in this time range");
                return;
            }

            // 3. Simple Fibonacci touch detection
            decimal tolerance = StartCandle.High * 0.005m; // 1% tolerance
            int touchCount = 0;

            foreach (var candle in candlesInRange)
            {
                foreach (decimal fibLevel in FibonacciLevels)
                {
                    if (Math.Abs(candle.High - fibLevel) <= tolerance ||
                        Math.Abs(candle.Low - fibLevel) <= tolerance)
                    {
                        // Add visual dot
                        float x = (float)chartArea.AxisX.ValueToPixelPosition(candle.Data.ToOADate());
                        float y = (float)chartArea.AxisY.ValueToPixelPosition((double)fibLevel);
                        confirmationPoints.Add(new PointF(x, y));
                        touchCount++;
                    }
                }
            }

            Confirmations.Add($"Found {touchCount} Fibonacci touches");
        }

        public void Draw(Graphics g, ChartArea priceArea)
        {
            if (!IsActive || StartCandle == null) return;

            float startX = (float)priceArea.AxisX.ValueToPixelPosition(StartCandle.Data.ToOADate());
            float startY = (float)priceArea.AxisY.ValueToPixelPosition((double)StartCandle.High);
            float endX = EndPoint.X; // Keep as screen coordinates
            float endY = EndPoint.Y; // Keep as screen coordinates

            // 1. Draw enclosing rectangle (original functionality)
            using (var pen = new Pen(Color.FromArgb(100, Color.Blue), 1) { DashStyle = DashStyle.Dash })
            {
                g.DrawRectangle(pen,
                    Math.Min(startX, endX),
                    Math.Min(startY, endY),
                    Math.Abs(endX - startX),
                    Math.Abs(endY - startY));
            }

            // 2. Draw diagonal line (original functionality)
            using (var pen = new Pen(Color.Red, 1.5f))
            {
                g.DrawLine(pen, startX, startY, endX, endY);
            }

            // 3. Draw Fibonacci levels (new functionality)
            var font = new Font("Arial", 8);
            foreach (var level in FibonacciLevels)
            {
                float y = (float)priceArea.AxisY.ValueToPixelPosition((double)level);
                g.DrawLine(new Pen(Color.FromArgb(150, Color.Green), 1) { DashStyle = DashStyle.Dash },
                          startX, y, endX, y);
                g.DrawString(level.ToString("0.0"), font, Brushes.Black, endX + 5, y - 8);
            }

            foreach (var point in confirmationPoints)
            {
                g.FillEllipse(Brushes.Gold, point.X - 3, point.Y - 3, 6, 6);
                g.DrawEllipse(Pens.DarkOrange, point.X - 3, point.Y - 3, 6, 6);
            }

            // Draw debug info (last 3 messages)
            float debugY = Math.Max(startY, endY) + 20;
            int startIndex = Math.Max(0, Confirmations.Count - 3);
            for (int i = startIndex; i < Confirmations.Count; i++)
            {
                g.DrawString(Confirmations[i], new Font("Arial", 8), Brushes.Red, endX + 20, debugY);
                debugY += 15;
            }
        }
        public void AdjustEndPrice(decimal priceAdjustment, List<CandleStick> allCandles, ChartArea chartArea)
        {
            if (!IsActive) return;

            // Convert current EndPoint.Y to price
            decimal currentPrice = (decimal)chartArea.AxisY.PixelPositionToValue(EndPoint.Y);

            // Calculate new price
            decimal newPrice = currentPrice + priceAdjustment;

            // Convert back to screen coordinates
            float newY = (float)chartArea.AxisY.ValueToPixelPosition((double)newPrice);

            // Update endpoint
            EndPoint = new PointF(EndPoint.X, newY);

            // Recalculate
            CalculateFibonacciLevels(chartArea);
            FindConfirmations(allCandles, chartArea);
        }

        public void InitializeFromWave(Wave wave, List<CandleStick> allCandles, ChartArea chartArea)
        {
            // Find the start candle based on wave direction
            CandleStick startCandle = allCandles.FirstOrDefault(c =>
                c.Data == wave.StartDate &&
                (wave.IsUpWave ? c.Low == (decimal)wave.StartPrice : c.High == (decimal)wave.StartPrice));

            if (startCandle == null) return;

            StartSelection(startCandle);

            // Set initial end point
            float endX = (float)chartArea.AxisX.ValueToPixelPosition(wave.EndDate.ToOADate());
            float endY = (float)chartArea.AxisY.ValueToPixelPosition(wave.EndPrice);
            EndPoint = new PointF(endX, endY);

            // Initialize Fibonacci levels and confirmations
            CalculateFibonacciLevels(chartArea);
            FindConfirmations(allCandles, chartArea);
        }

    }
}
