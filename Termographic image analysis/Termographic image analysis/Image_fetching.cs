using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termographic_image_analysis
{
    abstract class Image_fetching
    {

        private ushort[,] lastImageData;

        public Image_fetching()
        {

        }

        public ushort[,] getLastImageData()
        {
            return lastImageData;
        }

        public Bitmap GetBitmap(int min, int max, bool performColoring = false)
        {

            int[][] colors = { new int[] { 0, 0, 131 }, new int[] { 0, 0, 135 }, new int[] { 0, 0, 139 }, new int[] { 0, 0, 143 }, new int[] { 0, 0, 147 }, new int[] { 0, 0, 151 }, new int[] { 0, 0, 155 }, new int[] { 0, 0, 159 }, new int[] { 0, 0, 163 }, new int[] { 0, 0, 167 }, new int[] { 0, 0, 171 }, new int[] { 0, 0, 175 }, new int[] { 0, 0, 179 }, new int[] { 0, 0, 183 }, new int[] { 0, 0, 187 }, new int[] { 0, 0, 191 }, new int[] { 0, 0, 195 }, new int[] { 0, 0, 199 }, new int[] { 0, 0, 203 }, new int[] { 0, 0, 207 }, new int[] { 0, 0, 211 }, new int[] { 0, 0, 215 }, new int[] { 0, 0, 219 }, new int[] { 0, 0, 223 }, new int[] { 0, 0, 227 }, new int[] { 0, 0, 231 }, new int[] { 0, 0, 235 }, new int[] { 0, 0, 239 }, new int[] { 0, 0, 243 }, new int[] { 0, 0, 247 }, new int[] { 0, 0, 251 }, new int[] { 0, 0, 255 }, new int[] { 0, 4, 255 }, new int[] { 0, 8, 255 }, new int[] { 0, 12, 255 }, new int[] { 0, 16, 255 }, new int[] { 0, 20, 255 }, new int[] { 0, 24, 255 }, new int[] { 0, 28, 255 }, new int[] { 0, 32, 255 }, new int[] { 0, 36, 255 }, new int[] { 0, 40, 255 }, new int[] { 0, 44, 255 }, new int[] { 0, 48, 255 }, new int[] { 0, 52, 255 }, new int[] { 0, 56, 255 }, new int[] { 0, 60, 255 }, new int[] { 0, 64, 255 }, new int[] { 0, 68, 255 }, new int[] { 0, 72, 255 }, new int[] { 0, 76, 255 }, new int[] { 0, 80, 255 }, new int[] { 0, 84, 255 }, new int[] { 0, 88, 255 }, new int[] { 0, 92, 255 }, new int[] { 0, 96, 255 }, new int[] { 0, 100, 255 }, new int[] { 0, 104, 255 }, new int[] { 0, 108, 255 }, new int[] { 0, 112, 255 }, new int[] { 0, 116, 255 }, new int[] { 0, 120, 255 }, new int[] { 0, 124, 255 }, new int[] { 0, 128, 255 }, new int[] { 0, 131, 255 }, new int[] { 0, 135, 255 }, new int[] { 0, 139, 255 }, new int[] { 0, 143, 255 }, new int[] { 0, 147, 255 }, new int[] { 0, 151, 255 }, new int[] { 0, 155, 255 }, new int[] { 0, 159, 255 }, new int[] { 0, 163, 255 }, new int[] { 0, 167, 255 }, new int[] { 0, 171, 255 }, new int[] { 0, 175, 255 }, new int[] { 0, 179, 255 }, new int[] { 0, 183, 255 }, new int[] { 0, 187, 255 }, new int[] { 0, 191, 255 }, new int[] { 0, 195, 255 }, new int[] { 0, 199, 255 }, new int[] { 0, 203, 255 }, new int[] { 0, 207, 255 }, new int[] { 0, 211, 255 }, new int[] { 0, 215, 255 }, new int[] { 0, 219, 255 }, new int[] { 0, 223, 255 }, new int[] { 0, 227, 255 }, new int[] { 0, 231, 255 }, new int[] { 0, 235, 255 }, new int[] { 0, 239, 255 }, new int[] { 0, 243, 255 }, new int[] { 0, 247, 255 }, new int[] { 0, 251, 255 }, new int[] { 0, 255, 255 }, new int[] { 4, 255, 251 }, new int[] { 8, 255, 247 }, new int[] { 12, 255, 243 }, new int[] { 16, 255, 239 }, new int[] { 20, 255, 235 }, new int[] { 24, 255, 231 }, new int[] { 28, 255, 227 }, new int[] { 32, 255, 223 }, new int[] { 36, 255, 219 }, new int[] { 40, 255, 215 }, new int[] { 44, 255, 211 }, new int[] { 48, 255, 207 }, new int[] { 52, 255, 203 }, new int[] { 56, 255, 199 }, new int[] { 60, 255, 195 }, new int[] { 64, 255, 191 }, new int[] { 68, 255, 187 }, new int[] { 72, 255, 183 }, new int[] { 76, 255, 179 }, new int[] { 80, 255, 175 }, new int[] { 84, 255, 171 }, new int[] { 88, 255, 167 }, new int[] { 92, 255, 163 }, new int[] { 96, 255, 159 }, new int[] { 100, 255, 155 }, new int[] { 104, 255, 151 }, new int[] { 108, 255, 147 }, new int[] { 112, 255, 143 }, new int[] { 116, 255, 139 }, new int[] { 120, 255, 135 }, new int[] { 124, 255, 131 }, new int[] { 128, 255, 128 }, new int[] { 131, 255, 124 }, new int[] { 135, 255, 120 }, new int[] { 139, 255, 116 }, new int[] { 143, 255, 112 }, new int[] { 147, 255, 108 }, new int[] { 151, 255, 104 }, new int[] { 155, 255, 100 }, new int[] { 159, 255, 96 }, new int[] { 163, 255, 92 }, new int[] { 167, 255, 88 }, new int[] { 171, 255, 84 }, new int[] { 175, 255, 80 }, new int[] { 179, 255, 76 }, new int[] { 183, 255, 72 }, new int[] { 187, 255, 68 }, new int[] { 191, 255, 64 }, new int[] { 195, 255, 60 }, new int[] { 199, 255, 56 }, new int[] { 203, 255, 52 }, new int[] { 207, 255, 48 }, new int[] { 211, 255, 44 }, new int[] { 215, 255, 40 }, new int[] { 219, 255, 36 }, new int[] { 223, 255, 32 }, new int[] { 227, 255, 28 }, new int[] { 231, 255, 24 }, new int[] { 235, 255, 20 }, new int[] { 239, 255, 16 }, new int[] { 243, 255, 12 }, new int[] { 247, 255, 8 }, new int[] { 251, 255, 4 }, new int[] { 255, 255, 0 }, new int[] { 255, 251, 0 }, new int[] { 255, 247, 0 }, new int[] { 255, 243, 0 }, new int[] { 255, 239, 0 }, new int[] { 255, 235, 0 }, new int[] { 255, 231, 0 }, new int[] { 255, 227, 0 }, new int[] { 255, 223, 0 }, new int[] { 255, 219, 0 }, new int[] { 255, 215, 0 }, new int[] { 255, 211, 0 }, new int[] { 255, 207, 0 }, new int[] { 255, 203, 0 }, new int[] { 255, 199, 0 }, new int[] { 255, 195, 0 }, new int[] { 255, 191, 0 }, new int[] { 255, 187, 0 }, new int[] { 255, 183, 0 }, new int[] { 255, 179, 0 }, new int[] { 255, 175, 0 }, new int[] { 255, 171, 0 }, new int[] { 255, 167, 0 }, new int[] { 255, 163, 0 }, new int[] { 255, 159, 0 }, new int[] { 255, 155, 0 }, new int[] { 255, 151, 0 }, new int[] { 255, 147, 0 }, new int[] { 255, 143, 0 }, new int[] { 255, 139, 0 }, new int[] { 255, 135, 0 }, new int[] { 255, 131, 0 }, new int[] { 255, 128, 0 }, new int[] { 255, 124, 0 }, new int[] { 255, 120, 0 }, new int[] { 255, 116, 0 }, new int[] { 255, 112, 0 }, new int[] { 255, 108, 0 }, new int[] { 255, 104, 0 }, new int[] { 255, 100, 0 }, new int[] { 255, 96, 0 }, new int[] { 255, 92, 0 }, new int[] { 255, 88, 0 }, new int[] { 255, 84, 0 }, new int[] { 255, 80, 0 }, new int[] { 255, 76, 0 }, new int[] { 255, 72, 0 }, new int[] { 255, 68, 0 }, new int[] { 255, 64, 0 }, new int[] { 255, 60, 0 }, new int[] { 255, 56, 0 }, new int[] { 255, 52, 0 }, new int[] { 255, 48, 0 }, new int[] { 255, 44, 0 }, new int[] { 255, 40, 0 }, new int[] { 255, 36, 0 }, new int[] { 255, 32, 0 }, new int[] { 255, 28, 0 }, new int[] { 255, 24, 0 }, new int[] { 255, 20, 0 }, new int[] { 255, 16, 0 }, new int[] { 255, 12, 0 }, new int[] { 255, 8, 0 }, new int[] { 255, 4, 0 }, new int[] { 255, 0, 0 }, new int[] { 251, 0, 0 }, new int[] { 247, 0, 0 }, new int[] { 243, 0, 0 }, new int[] { 239, 0, 0 }, new int[] { 235, 0, 0 }, new int[] { 231, 0, 0 }, new int[] { 227, 0, 0 }, new int[] { 223, 0, 0 }, new int[] { 219, 0, 0 }, new int[] { 215, 0, 0 }, new int[] { 211, 0, 0 }, new int[] { 207, 0, 0 }, new int[] { 203, 0, 0 }, new int[] { 199, 0, 0 }, new int[] { 195, 0, 0 }, new int[] { 191, 0, 0 }, new int[] { 187, 0, 0 }, new int[] { 183, 0, 0 }, new int[] { 179, 0, 0 }, new int[] { 175, 0, 0 }, new int[] { 171, 0, 0 }, new int[] { 167, 0, 0 }, new int[] { 163, 0, 0 }, new int[] { 159, 0, 0 }, new int[] { 155, 0, 0 }, new int[] { 151, 0, 0 }, new int[] { 147, 0, 0 }, new int[] { 143, 0, 0 }, new int[] { 139, 0, 0 }, new int[] { 135, 0, 0 }, new int[] { 131, 0, 0 }, new int[] { 131, 0, 0 } };

            int rows = lastImageData.GetLength(0);
            int cols = lastImageData.GetLength(1);

            Bitmap image = new Bitmap(cols, rows, PixelFormat.Format24bppRgb);

            int range = (max - min);

            if (range <= 0)
            {
                range = 1;
            }

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    int d = lastImageData[i, j];

                    if (d > max)
                    {
                        d = max;
                    }

                    d -= min;
                    if (d < 0)
                    {
                        d = 0;
                    }

                    d = (int)(255.0 * d / (double)range);

                    if (performColoring)
                    {
                        image.SetPixel(j, i, Color.FromArgb(colors[d][0], colors[d][1], colors[d][2]));
                    }
                    else
                    {
                        image.SetPixel(j, i, Color.FromArgb(d, d, d));
                    }
                }
            }

            return image;
        }



    }
}
