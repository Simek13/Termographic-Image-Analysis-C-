using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termographic_image_analysis
{
    abstract class Analysis
    {

        protected ushort[,] lastImageData;
        protected ushort[] histogram;
        protected ushort minTemp;
        protected ushort maxTemp;
        protected Bitmap image;

        public Analysis(ushort[,] lastImageData, Bitmap image)
        {
            this.lastImageData = lastImageData;
            this.image = image;

            Min_max();
            removeHeader();

        }

        public float Average_temp_area(int minRow, int maxRow, int minCol, int maxCol)
        {
            float meanTemperatureK = 0;

            for(int i = minRow; i<=maxRow; i++)
            {
                for(int j = minCol; j <= maxCol; j++)
                {
                    meanTemperatureK += lastImageData[i, j];
                }
            }

            meanTemperatureK /= (maxRow - minRow + 1) * (maxCol - minCol + 1) * 100;

            return meanTemperatureK;
        }

        public float Pixel_temp(int row, int col)
        {

            return lastImageData[row, col]/100;

        }



        public void Calculate_histogram()
        {
            histogram = new ushort[maxTemp - minTemp + 1];

            for (int i = 0; i <= lastImageData.GetLength(0); i++)
            {
                for (int j = 0; j <= lastImageData.GetLength(1); j++)
                {
                    histogram[lastImageData[i, j] - minTemp]++;
                }
            }

        }

        private void removeHeader()
        {
            int rows = lastImageData.GetLength(0);
            int cols = lastImageData.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if(lastImageData[i,j] == 0)
                    {
                        lastImageData[i,j] = minTemp;
                    }
                }
            }
        }

        public void Min_max()
        {
            minTemp = ushort.MaxValue;
            maxTemp = ushort.MinValue;

            for(int i=0; i <= lastImageData.GetLength(0); i++)
            {
                for(int j = 0; j <= lastImageData.GetLength(1); j++)
                {
                    if(lastImageData[i,j] < minTemp)
                    {
                        minTemp = lastImageData[i, j];
                    }
                    if (lastImageData[i, j] > maxTemp)
                    {
                        maxTemp = lastImageData[i, j];
                    }
                }
            }
        }

        public static float RoundUp(float input, int places)
        {
            float multiplier = (float)Math.Pow(10, Convert.ToDouble(places));
            return (float)Math.Ceiling(input * multiplier) / multiplier;
        }

        public Bitmap removeBackground(Bitmap image)
        {
            int minBodyTemp = findMinBodyTemperature();

            
            for (int i = 0; i < lastImageData.GetLength(0); i++)
            {
                for (int j = 0; j < lastImageData.GetLength(1); j++)
                {
                    if (lastImageData[i, j] < minBodyTemp)
                    {
                        image.SetPixel(j, i, Color.White);
                    }
                    
                }
            }

            
            return image;
        }

        public int findMinBodyTemperature()
        {
            ushort[] histogram = new ushort[this.histogram.Length];

            histogram = (ushort[])this.histogram.Clone();

            histogram[0] = 0;
            histogram[histogram.Length - 1] = 0;

            bool control = true;
            int numberOfHistogramElements = 0;

            while (control)
            {
                numberOfHistogramElements = 0;
                histogram = findPeaks(histogram);
                for (int i = 0; i < histogram.Length; i++)
                {
                    if (histogram[i] != 0)
                    {
                        numberOfHistogramElements++;
                    }
                }

                if (numberOfHistogramElements == 2)
                {
                    control = false;
                }
            }


            int peak1 = 0, peak2 = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (histogram[i] != 0)
                {
                    if (!control)
                    {
                        peak1 = i;
                        control = true;
                    }
                    else
                    {
                        peak2 = i;
                    }
                }
            }

            histogram = (ushort[])this.histogram.Clone();

            int minimum = 65535;
            int tempDiff = 0;
            for (int i = peak1; i < peak2; i++)
            {
                if (histogram[i] < minimum)
                {
                    minimum = histogram[i];
                    tempDiff = i;
                }
            }
            return tempDiff + minTemp;
        }

        private ushort[] findPeaks(ushort[] histogram)
        {
            int[] peaks = new int[histogram.Length];
            int j = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (histogram[i] != 0)
                {
                    peaks[j] = i;
                    j++;
                }
            }

            for (; j < histogram.Length; j++)
            {
                peaks[j] = 0;
            }


            for (int i = 1; peaks[i] != 0; i++)
            {
                if (histogram[peaks[i]] < histogram[peaks[i - 1]] || histogram[peaks[i]] < histogram[peaks[i + 1]])
                {
                    histogram[peaks[i]] = 0;
                }

                if (histogram[peaks[i - 1]] < histogram[peaks[i]])
                {
                    histogram[peaks[i - 1]] = 0;
                }
            }

            return histogram;
        }


    }
}
