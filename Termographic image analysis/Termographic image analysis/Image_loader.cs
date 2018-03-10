using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Termographic_image_analysis
{
    class Image_loader : Image_fetching
    {

        public Image_loader()
        {

        }


        public void Load_image()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Tekstualna datoteka (*.txt)|*.txt|Sve datoteke (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader input = new StreamReader(new FileStream(dialog.FileName, FileMode.Open));

                int rows = 240;
                int cols = 320;

                lastImageData = new ushort[rows, cols];

                for (int i = 0; i < rows; ++i)
                {
                    string line = input.ReadLine();
                    string[] parts = line.Split(' ');
                    for (int j = 0; j < cols; ++j)
                    {
                        string[] subparts = parts[j].Split('.');
                        lastImageData[i, j] = (ushort)(int.Parse(subparts[0]) * 100 + int.Parse(subparts[1]));
                    }
                }

            }

        }

        }
}
