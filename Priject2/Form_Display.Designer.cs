namespace Priject2
{
    partial class Form_Display
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart_Stock = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.label_EndDate = new System.Windows.Forms.Label();
            this.label_StartDate = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.label_scrollbar = new System.Windows.Forms.Label();
            this.comboBox_UpWaves = new System.Windows.Forms.ComboBox();
            this.comboBox_DownWaves = new System.Windows.Forms.ComboBox();
            this.label_UpWavesBox = new System.Windows.Forms.Label();
            this.label_DownWavesBox = new System.Windows.Forms.Label();
            this.button_Plus = new System.Windows.Forms.Button();
            this.button_Minus = new System.Windows.Forms.Button();
            this.button_startstop = new System.Windows.Forms.Button();
            this.timer_Tick = new System.Windows.Forms.Timer(this.components);
            this.hScrollBar_Steps = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Stock)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_Stock
            // 
            chartArea8.Name = "ChartArea1";
            this.chart_Stock.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.chart_Stock.Legends.Add(legend8);
            this.chart_Stock.Location = new System.Drawing.Point(22, 12);
            this.chart_Stock.Name = "chart_Stock";
            series8.ChartArea = "ChartArea1";
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            this.chart_Stock.Series.Add(series8);
            this.chart_Stock.Size = new System.Drawing.Size(824, 394);
            this.chart_Stock.TabIndex = 0;
            this.chart_Stock.Text = "chart1";
            this.chart_Stock.Paint += new System.Windows.Forms.PaintEventHandler(this.chart_Stock_Paint);
            this.chart_Stock.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart_Stock_MouseDown);
            this.chart_Stock.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_Stock_MouseMove);
            this.chart_Stock.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_Stock_MouseUp);
            // 
            // button_Refresh
            // 
            this.button_Refresh.Location = new System.Drawing.Point(326, 422);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(75, 23);
            this.button_Refresh.TabIndex = 1;
            this.button_Refresh.Text = "Refresh";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(22, 425);
            this.dateTimePicker_StartDate.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_StartDate.TabIndex = 2;
            this.dateTimePicker_StartDate.Value = new System.DateTime(2021, 2, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(537, 422);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_EndDate.TabIndex = 3;
            // 
            // label_EndDate
            // 
            this.label_EndDate.AutoSize = true;
            this.label_EndDate.Location = new System.Drawing.Point(534, 406);
            this.label_EndDate.Name = "label_EndDate";
            this.label_EndDate.Size = new System.Drawing.Size(50, 13);
            this.label_EndDate.TabIndex = 4;
            this.label_EndDate.Text = "End date";
            // 
            // label_StartDate
            // 
            this.label_StartDate.AutoSize = true;
            this.label_StartDate.Location = new System.Drawing.Point(19, 409);
            this.label_StartDate.Name = "label_StartDate";
            this.label_StartDate.Size = new System.Drawing.Size(55, 13);
            this.label_StartDate.TabIndex = 5;
            this.label_StartDate.Text = "Start Date";
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(196, 468);
            this.hScrollBar1.Maximum = 8;
            this.hScrollBar1.Minimum = 1;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(336, 21);
            this.hScrollBar1.TabIndex = 6;
            this.hScrollBar1.Value = 1;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // label_scrollbar
            // 
            this.label_scrollbar.AutoSize = true;
            this.label_scrollbar.Location = new System.Drawing.Point(323, 500);
            this.label_scrollbar.Name = "label_scrollbar";
            this.label_scrollbar.Size = new System.Drawing.Size(91, 13);
            this.label_scrollbar.TabIndex = 7;
            this.label_scrollbar.Text = "Current Margin:  1";
            // 
            // comboBox_UpWaves
            // 
            this.comboBox_UpWaves.FormattingEnabled = true;
            this.comboBox_UpWaves.Location = new System.Drawing.Point(616, 492);
            this.comboBox_UpWaves.Name = "comboBox_UpWaves";
            this.comboBox_UpWaves.Size = new System.Drawing.Size(121, 21);
            this.comboBox_UpWaves.TabIndex = 9;
            this.comboBox_UpWaves.SelectedIndexChanged += new System.EventHandler(this.comboBox_UpWaves_SelectedIndexChanged);
            // 
            // comboBox_DownWaves
            // 
            this.comboBox_DownWaves.FormattingEnabled = true;
            this.comboBox_DownWaves.Location = new System.Drawing.Point(798, 492);
            this.comboBox_DownWaves.Name = "comboBox_DownWaves";
            this.comboBox_DownWaves.Size = new System.Drawing.Size(121, 21);
            this.comboBox_DownWaves.TabIndex = 10;
            this.comboBox_DownWaves.SelectedIndexChanged += new System.EventHandler(this.comboBox_DownWaves_SelectedIndexChanged);
            // 
            // label_UpWavesBox
            // 
            this.label_UpWavesBox.AutoSize = true;
            this.label_UpWavesBox.Location = new System.Drawing.Point(613, 471);
            this.label_UpWavesBox.Name = "label_UpWavesBox";
            this.label_UpWavesBox.Size = new System.Drawing.Size(58, 13);
            this.label_UpWavesBox.TabIndex = 11;
            this.label_UpWavesBox.Text = "Up Waves";
            // 
            // label_DownWavesBox
            // 
            this.label_DownWavesBox.AutoSize = true;
            this.label_DownWavesBox.Location = new System.Drawing.Point(795, 471);
            this.label_DownWavesBox.Name = "label_DownWavesBox";
            this.label_DownWavesBox.Size = new System.Drawing.Size(72, 13);
            this.label_DownWavesBox.TabIndex = 12;
            this.label_DownWavesBox.Text = "Down Waves";
            // 
            // button_Plus
            // 
            this.button_Plus.Location = new System.Drawing.Point(906, 130);
            this.button_Plus.Name = "button_Plus";
            this.button_Plus.Size = new System.Drawing.Size(75, 23);
            this.button_Plus.TabIndex = 13;
            this.button_Plus.Text = "+";
            this.button_Plus.UseVisualStyleBackColor = true;
            this.button_Plus.Click += new System.EventHandler(this.button_Plus_Click);
            // 
            // button_Minus
            // 
            this.button_Minus.Location = new System.Drawing.Point(906, 176);
            this.button_Minus.Name = "button_Minus";
            this.button_Minus.Size = new System.Drawing.Size(75, 23);
            this.button_Minus.TabIndex = 14;
            this.button_Minus.Text = "-";
            this.button_Minus.UseVisualStyleBackColor = true;
            this.button_Minus.Click += new System.EventHandler(this.button_Minus_Click);
            // 
            // button_startstop
            // 
            this.button_startstop.Location = new System.Drawing.Point(906, 219);
            this.button_startstop.Name = "button_startstop";
            this.button_startstop.Size = new System.Drawing.Size(75, 23);
            this.button_startstop.TabIndex = 15;
            this.button_startstop.Text = "S/S";
            this.button_startstop.UseVisualStyleBackColor = true;
            this.button_startstop.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer_Tick
            // 
            this.timer_Tick.Tick += new System.EventHandler(this.timer_Tick_Tick);
            // 
            // hScrollBar_Steps
            // 
            this.hScrollBar_Steps.Location = new System.Drawing.Point(860, 326);
            this.hScrollBar_Steps.Minimum = 2;
            this.hScrollBar_Steps.Name = "hScrollBar_Steps";
            this.hScrollBar_Steps.Size = new System.Drawing.Size(159, 17);
            this.hScrollBar_Steps.TabIndex = 17;
            this.hScrollBar_Steps.Value = 2;
            this.hScrollBar_Steps.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Steps_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(917, 304);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Step Size";
            // 
            // Form_Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 569);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hScrollBar_Steps);
            this.Controls.Add(this.button_startstop);
            this.Controls.Add(this.button_Minus);
            this.Controls.Add(this.button_Plus);
            this.Controls.Add(this.label_DownWavesBox);
            this.Controls.Add(this.label_UpWavesBox);
            this.Controls.Add(this.comboBox_DownWaves);
            this.Controls.Add(this.comboBox_UpWaves);
            this.Controls.Add(this.label_scrollbar);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.label_StartDate);
            this.Controls.Add(this.label_EndDate);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Controls.Add(this.button_Refresh);
            this.Controls.Add(this.chart_Stock);
            this.Name = "Form_Display";
            this.Text = "Form_Display";
            ((System.ComponentModel.ISupportInitialize)(this.chart_Stock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Stock;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.Label label_EndDate;
        private System.Windows.Forms.Label label_StartDate;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label label_scrollbar;
        private System.Windows.Forms.ComboBox comboBox_UpWaves;
        private System.Windows.Forms.ComboBox comboBox_DownWaves;
        private System.Windows.Forms.Label label_UpWavesBox;
        private System.Windows.Forms.Label label_DownWavesBox;
        private System.Windows.Forms.Button button_Plus;
        private System.Windows.Forms.Button button_Minus;
        private System.Windows.Forms.Button button_startstop;
        private System.Windows.Forms.Timer timer_Tick;
        private System.Windows.Forms.HScrollBar hScrollBar_Steps;
        private System.Windows.Forms.Label label1;
    }
}