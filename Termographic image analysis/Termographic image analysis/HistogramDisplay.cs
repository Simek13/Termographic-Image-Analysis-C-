using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Termographic_image_analysis
{
    public partial class HistogramDisplay : UserControl
    {

        private int min = 0;
        private int max = 65535; //maxRange

        private int minLine = 0;
        private int maxLine = 65535;

        private int resolution = 20;

        private const int histogramSize = 65535;

        private int[] histogram=new int[histogramSize];


        public void maxTemp(string temp)
        {
            lblMaxTemp.Text = temp;
        }

        public void minTemp(string temp)
        {
            lblMinTemp.Text = temp;
        }

        public HistogramDisplay()
        {
            InitializeComponent();
        }
        

        public void levelSetup(int range)
        {
            max = range;
        }

        private void HistogramDisplay_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < histogramSize; ++i) {
                histogram[i] = 0;
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle r = ClientRectangle;
            g.FillRectangle(new SolidBrush(Color.White), r);

            int binsCount = max - min + 1;

            int take = binsCount / resolution;
            int remainder = binsCount - take * resolution;

            int sum = 0;
            for (int i = 0; i < histogram.Length; ++i) {
                sum += histogram[i];
            }

            if (sum == 0) {
                return;
            }
            
            int startBin = 0;

            int maxSum = 0;
            for (int i = 0; i < resolution -1; ++i)
            {
                int currentSum = 0;
                int stopBin = startBin + take - 1;

                if (remainder > 0)
                {
                    --remainder;
                    ++stopBin;
                }

                for (int j = startBin; j <= stopBin; ++j)
                {
                    currentSum += histogram[j];
                }
                if (currentSum > maxSum)
                    maxSum = currentSum;
                startBin = stopBin + 1;
            }

            startBin = 0;
            int startX = r.X;
            remainder = binsCount - take * resolution;
            for (int i = 0; i < resolution - 1; ++i)
            {
                int stopBin = startBin + take - 1;
                if (remainder > 0)
                {
                    --remainder;
                    ++stopBin;
                }
                
                int currentSum=0;
                for (int j = startBin; j <= stopBin; ++j) {
                    currentSum += histogram[j];
                }
                
                int stopX = r.X + r.Width * (stopBin + 1) / binsCount;
                int width = stopX - startX + 1;
                int height = r.Height*currentSum/maxSum;
                int y = r.Y + r.Height - height;
                
                

                g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(startX, y, width, height));

                startBin = stopBin + 1;
                startX = stopX + 1;
                
            }

            int lineX = r.X + (int)(((float)r.Width * ((float)minLine / 65535))); // / (float)binsCount);
            g.DrawLine(new Pen(Color.Red, 5), new Point(lineX, r.Y), new Point(lineX, r.Y + r.Height));
            
            
            lblMinTemp.Left = lineX;

            lblMinTemp.Visible = true;

            lineX = r.X + (int)(((float)r.Width * ((float)maxLine / 65535))); // / binsCount;
            g.DrawLine(new Pen(Color.Red, 5), new Point(lineX, r.Y), new Point(lineX, r.Y + r.Height));




            lblMaxTemp.Left = lineX;

            lblMaxTemp.Visible = true;

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        public new int Min
        {
            get
            {
                return min;
            }
        }

        public new int Max
        {
            get
            {
                return max;
            }
        }

        public new int MinLine
        {
            get
            {
                return minLine;
            }
            set
            {
                minLine = value;
                Invalidate();
            }
        }

        public new int MaxLine
        {
            get
            {
                return maxLine;
            }
            set
            {
                maxLine = value;
                Invalidate();
            }
        }

        public new int Resolution {
            get {
                return resolution;
            }
            set {
                resolution = value;
                Invalidate();
            }
        }

        public void UpdateHistogram(int[] newData) {
            int n = histogram.Length;
            if (newData.Length < n) {
                n = newData.Length;
            }

            for (int i = 0; i < n; ++i) {
                histogram[i] = newData[i];
            }
            Invalidate();
        }
    }
}
