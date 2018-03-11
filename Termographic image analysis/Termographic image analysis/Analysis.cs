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

        public abstract void Analize();

    }
}
