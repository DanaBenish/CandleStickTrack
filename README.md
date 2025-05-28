# 🌊📈 WaveFinder - Advanced Stock Pattern Analyzer

[![Demo Video](https://img.shields.io/badge/▶-Watch%20Demo-red?style=for-the-badge&logo=youtube)](https://your-video-link-here.com)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

## 🚀 Project Overview
WaveFinder is an **interactive C# stock analysis tool** that combines candlestick pattern visualization with advanced wave theory and Fibonacci retracement tools. Perfect for technical analysts looking to identify market trends.

![App Screenshot](screenshot.png) *(Upload a screenshot and replace this text)*

## ✨ Key Features

### 📊 Data Visualization
| Feature | Description |
|---------|-------------|
| **Dual-Chart System** | Synchronized candlestick (OHLC) + volume charts |
| **Smart Timeframes** | Daily/Weekly/Monthly aggregation with smooth transitions |
| **Dynamic Date Ranges** | Interactive calendar pickers for precise analysis |

**🌊 Wave Analysis**
- Auto peak/valley detection (adjustable sensitivity)
- Manual wave drawing between critical points
- Multi-wave management with color coding (green=bullish/red=bearish)

**📐 Fibonacci Tools**
- Automatic retracement level calculation (23.6%, 38.2%, etc.)
- Golden dots marking accurate predictions
- Live level updates during simulation

**🎮 Interactive Simulator**
- +/- buttons for manual wave adjustment
- Auto-mode with configurable step size
- Real-time chart updates

## 🚀 Quick Start
1. **Prerequisites**: .NET 6.0+, Visual Studio 2022
2. **Run the app**:
```bash
git clone https://github.com/danabenish/CandleStickTrack.git
cd StockWave-Analyzer/src
start WindowsFormsProject1.sln
```

## 📂 Data Format Requirements
```bash
CSV files must contain these exact column headers:
"Date","Open","High","Low","Close","Volume"
"2023-01-03",9.960000000382,10.1599998474,9.9499998092,10.090000152587,32307
```

## 🎛️ Controls Reference

| Control               | Action                                  |
|-----------------------|-----------------------------------------|
| 🖱️ Left-click + drag  | Draw new wave between points            |
| 🎚️ Margin slider      | Adjust peak/valley detection sensitivity|
| ▶/⏹️ Buttons          | Start/Stop auto-simulation              |
| +/- Buttons           | Manual wave height adjustment           |
| 📅 Date dropdowns      | Change chart date range                 |
| Right-click wave      | Delete/Configure wave                   |
| Mouse wheel           | Zoom in/out on chart                    |

## 📝 Example Workflow

1. **Load Data**  
   Import your stock CSV file (`File > Import` or drag-and-drop)

2. **Set Range**  
   Select start/end dates from dropdown calendars

3. **Analyze**  
   - Let the system auto-detect peaks/valleys  
     *or*  
   - Manually draw waves between key points (click+drag)

4. **Simulate**  
   - Run auto-simulator with ▶ button  
   - Observe Fibonacci levels updating in real-time

5. **Validate**  
   Watch for golden dots at accurate prediction points

## 💡 Tips for Best Results
- **📅 Use 6+ months** of daily data for clearer wave patterns  
- **⛰️ Start waves** at significant peaks/valleys (ignore minor fluctuations)  
- **🎚️ Adjust sensitivity** until inflection points are clearly marked  
- **🔍 Zoom in** (mouse wheel) for precise wave drawing  
- **🔄 Compare multiple waves** using different colors  
