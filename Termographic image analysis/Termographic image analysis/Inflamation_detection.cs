﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termographic_image_analysis
{
    class Inflamation_detection : Analysis
    {


        public Inflamation_detection(ushort[,] lastImageData, Bitmap image) : base(lastImageData, image)
        {
           
            
        }


        public Tuple<ushort, float> Analize(ushort range)
        {

            ushort integralTemp = 0;
            ushort pixelNum = 0;
            float meanTemp = 0;

            for (int i = 0; i < lastImageData.GetLength(0); i++)
            {
                for (int j = 0; j < lastImageData.GetLength(1); j++)
                {
                    if (lastImageData[i, j] > range)
                    {
                        ++pixelNum;
                        integralTemp += lastImageData[i, j];
                        image.SetPixel(j, i, Color.Black);
                    }
                }
                    meanTemp = integralTemp / (pixelNum * 100);
                    meanTemp -= (float)273.15;
                    meanTemp = RoundUp(meanTemp, 2);
                
            }
            return new Tuple<ushort, float>(pixelNum, meanTemp);
            
        }

    }
   
}
