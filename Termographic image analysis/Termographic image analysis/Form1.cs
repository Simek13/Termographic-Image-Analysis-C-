using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jai_FactoryDotNET;
using System.Net.Sockets;
using System.Threading;

namespace Termographic_image_analysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int minValue = 0;
        private int maxValue = 65535;
        private int minData = 0;
        private int maxData = 65535;
        private bool performColoring = true;

        private Bitmap lastImage = null; //last image on stream or opened image
        private Bitmap lastImageBackgroundRemoved = null;
        private Bitmap lastImageInflamation = null; // and Symmetry for saveing last image data
        private Bitmap lastImageSymmetry = null;
        private int minBodyTemp; //doesnt work on very low temperatures
        private double averageBodyTemp; //also has problems

        private string lastDirectory = "";
        private bool automaticAdjustment = true;

        private ushort[,] lastImageData = null; //temperature file

        private int[] histogramData;

        private int inflamationROI = 0;                         //selected ROI data information (size, temp)
        private double inflamationROIaverageTemperature = 0;

        private uint[,] lastIntegralImageData = null;
        private bool selecting = false;
        int minRectangleX = 0;
        int maxRectangleX = 0;
        int minRectangleY = 0;
        int maxRectangleY = 0;
        private int initialX = 0;
        private int initialY = 0;

        //Luka analysis

        private bool inflamationDetection = false;

        //Šime analysis

        private bool symmetryAnalysis = false;


        
        private void Form1_Load(object sender, EventArgs e)
        {

            //Test();
            //return;

            coloringCheckBox.Enabled = false;
            automaticAdjustmentCheckBox.Enabled = false;

            FlipAcquisitionState();

            if (camera != null)
            {
                saveButton.Enabled = true;
                saveFastButton.Enabled = true;
            }

        }

        private CCamera InitializeCamera()
        {
            // GenICam nodes
            CNode myWidthNode;
            CNode myHeightNode;

            //Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.EFactoryError.Success;

            try
            {
                bool AnyChanges = factory.UpdateCameraList(CFactory.EDriverType.SocketDriver);
            }
            catch (Exception e)
            {
                MessageBox.Show("Detekcija kamere nije uspjela. " + e.Message, "Detekcija kamere", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return null;
            }

            if (factory.NumOfCameras > 0)
            {
                for (int i = 0; i < factory.CameraList.Count; i++)
                {
                    camera = factory.CameraList[i];
                    if (Jai_FactoryWrapper.EFactoryError.Success == camera.Open())
                    {
                        break;
                    }
                }

                camera.StretchLiveVideo = true;

                // Get the Width GenICam Node
                myWidthNode = camera.GetNode("Width");
                if (myWidthNode != null)
                {
                    SetFramegrabberValue("Width", (Int64)myWidthNode.Value);
                }

                myHeightNode = camera.GetNode("Height");
                if (myHeightNode != null)
                {
                    SetFramegrabberValue("Height", (Int64)myHeightNode.Value);
                }

                camera.GetNode("IRFormat").Value = 2;

                SetFramegrabberPixelFormat();

                return camera;

            }

            return null;
        }

        private void HandleImage(ref Jai_FactoryWrapper.ImageInfo ImageInfo)
        {

            try
            {
                /*
                CNode node = camera.GetNode("ScaleLimitLow");
                scaleLimitLow = (double)node.Value;
                node = camera.GetNode("ScaleLimitUpper");
                scaleLimitUpper = (double)node.Value;
                */
            }
            catch (Jai_FactoryDotNET.Jai_FactoryWrapper.FactoryErrorException e)
            {
            }

            int rows = 240;
            int cols = 320;

            if (camera.LastRawFrameCopy.ImageSize != rows * cols * 2)
            {
                return;
            }

            lastImageData = GetImageData(camera.LastRawFrameCopy);

            DrawImage(lastImageData);

        }

        private void DrawImage(ushort[,] imageData)
        {
            if (!symmetryAnalysis) {
                coloringCheckBox.Enabled = true;
                automaticAdjustmentCheckBox.Enabled = true;
            }

            lastIntegralImageData = CalculateIntegralImageData(imageData);

            if (imageData == null)
            {
                return;
            }
            int[] histogram = new int[maxData * 100];
            for (int i = 0; i < histogram.Length; ++i)
            {
                histogram[i] = 0;
            }
            minData = 65535;
            maxData = 0;

            for (int i = 0; i < imageData.GetLength(0); ++i)
            {
                for (int j = 0; j < imageData.GetLength(1); ++j)
                {
                    ushort value = imageData[i, j];
                    ++histogram[value];
                    if (value < minData && value != 0)
                    {
                        minData = value;
                    }
                    if (maxData < value && value != 0)
                    {
                        maxData = value;
                    }
                }
            }


            int[] histogramFinal = new int[maxData - minData];
            int level = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (i > minData && i < maxData)
                {
                    histogramFinal[level] = histogram[i];
                    level++;
                }
            }
            //

            histogramData = (int[])histogramFinal.Clone();

            //maxValue = maxData;

            histogramDisplay.levelSetup(level);
            histogramDisplay.UpdateHistogram(histogramFinal);

            int bitmapMin = minValue;
            int bitmapMax = maxValue;
            if (automaticAdjustment)
            {
                histogramDisplay.MinLine = 0;
                histogramDisplay.MaxLine = 65535;
                minValue = minData;
                maxValue = maxData;
            }



            histogramDisplay.minTemp(((float)minValue / 100).ToString());
            histogramDisplay.maxTemp(((float)maxValue / 100).ToString());

            Bitmap image = GetBitmap(imageData, minValue, maxValue, performColoring);

            //Graphics g = imageDisplay.CreateGraphics();
            imageDisplay.Image = image;
            int width = imageDisplay.Width;
            int height = imageDisplay.Height;
            //g.DrawImage(image, 0, 0, width, height);
            if (lastImage != null)
            {
                lastImage.Dispose();
            }

            lastImage = (Bitmap)image.Clone();

            minBodyTemp = removeBackground((Bitmap)lastImage.Clone());
            drawInflamation((int)(((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp) + minBodyTemp), (Bitmap)image.Clone());
            


            averageTemp.Text = "Prosječna temperatura tijela: " + averageBodyTemp.ToString() + "°C";
            averageTemp.ForeColor = Color.Blue;
            averageTemp.Visible = true;

            lastImage = image;
        }

        private uint[,] CalculateIntegralImageData(ushort[,] data)
        {

            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            uint[,] result = new uint[rows + 1, cols + 1];

            for (int i = 0; i < rows + 1; ++i)
            {
                result[i, 0] = 0;
            }
            for (int i = 0; i < cols + 1; ++i)
            {
                result[0, i] = 0;
            }
            result[1, 1] = data[0, 0];
            for (int i = 1; i < rows; ++i)
            {
                result[i + 1, 1] = result[i, 1] + data[i, 0];
            }
            for (int i = 1; i < cols; ++i)
            {
                result[1, i + 1] = result[1, i] + data[0, i];
            }
            for (int i = 1; i < rows; ++i)
            {
                for (int j = 1; j < cols; ++j)
                {
                    result[i + 1, j + 1] = data[i, j] + result[i + 1, j] + result[i, j + 1] - result[i, j];
                }
            }


            return result;
        }

        private ushort[,] GetImageData(Jai_FactoryDotNET.Jai_FactoryWrapper.ImageInfo imageInfo)
        {

            int rows = (int)imageInfo.SizeY;
            int cols = (int)imageInfo.SizeX;

            ushort[,] imageData = new ushort[rows, cols];
            unsafe
            {
                byte* bytes = (byte*)imageInfo.ImageBuffer;

                for (int i = 0; i < rows; ++i)
                {
                    for (int j = 0; j < cols; ++j)
                    {
                        int p1 = bytes[2 * (i * cols + j)];
                        int p2 = bytes[2 * (i * cols + j) + 1];
                        imageData[i, j] = (ushort)((p2 << 8) + p1);

                    }
                }

            }

            return imageData;
        }

        private Bitmap GetBitmap(ushort[,] imageData, int min, int max, bool performColoring = false)
        {

            int[][] colors = { new int[] { 0, 0, 131 }, new int[] { 0, 0, 135 }, new int[] { 0, 0, 139 }, new int[] { 0, 0, 143 }, new int[] { 0, 0, 147 }, new int[] { 0, 0, 151 }, new int[] { 0, 0, 155 }, new int[] { 0, 0, 159 }, new int[] { 0, 0, 163 }, new int[] { 0, 0, 167 }, new int[] { 0, 0, 171 }, new int[] { 0, 0, 175 }, new int[] { 0, 0, 179 }, new int[] { 0, 0, 183 }, new int[] { 0, 0, 187 }, new int[] { 0, 0, 191 }, new int[] { 0, 0, 195 }, new int[] { 0, 0, 199 }, new int[] { 0, 0, 203 }, new int[] { 0, 0, 207 }, new int[] { 0, 0, 211 }, new int[] { 0, 0, 215 }, new int[] { 0, 0, 219 }, new int[] { 0, 0, 223 }, new int[] { 0, 0, 227 }, new int[] { 0, 0, 231 }, new int[] { 0, 0, 235 }, new int[] { 0, 0, 239 }, new int[] { 0, 0, 243 }, new int[] { 0, 0, 247 }, new int[] { 0, 0, 251 }, new int[] { 0, 0, 255 }, new int[] { 0, 4, 255 }, new int[] { 0, 8, 255 }, new int[] { 0, 12, 255 }, new int[] { 0, 16, 255 }, new int[] { 0, 20, 255 }, new int[] { 0, 24, 255 }, new int[] { 0, 28, 255 }, new int[] { 0, 32, 255 }, new int[] { 0, 36, 255 }, new int[] { 0, 40, 255 }, new int[] { 0, 44, 255 }, new int[] { 0, 48, 255 }, new int[] { 0, 52, 255 }, new int[] { 0, 56, 255 }, new int[] { 0, 60, 255 }, new int[] { 0, 64, 255 }, new int[] { 0, 68, 255 }, new int[] { 0, 72, 255 }, new int[] { 0, 76, 255 }, new int[] { 0, 80, 255 }, new int[] { 0, 84, 255 }, new int[] { 0, 88, 255 }, new int[] { 0, 92, 255 }, new int[] { 0, 96, 255 }, new int[] { 0, 100, 255 }, new int[] { 0, 104, 255 }, new int[] { 0, 108, 255 }, new int[] { 0, 112, 255 }, new int[] { 0, 116, 255 }, new int[] { 0, 120, 255 }, new int[] { 0, 124, 255 }, new int[] { 0, 128, 255 }, new int[] { 0, 131, 255 }, new int[] { 0, 135, 255 }, new int[] { 0, 139, 255 }, new int[] { 0, 143, 255 }, new int[] { 0, 147, 255 }, new int[] { 0, 151, 255 }, new int[] { 0, 155, 255 }, new int[] { 0, 159, 255 }, new int[] { 0, 163, 255 }, new int[] { 0, 167, 255 }, new int[] { 0, 171, 255 }, new int[] { 0, 175, 255 }, new int[] { 0, 179, 255 }, new int[] { 0, 183, 255 }, new int[] { 0, 187, 255 }, new int[] { 0, 191, 255 }, new int[] { 0, 195, 255 }, new int[] { 0, 199, 255 }, new int[] { 0, 203, 255 }, new int[] { 0, 207, 255 }, new int[] { 0, 211, 255 }, new int[] { 0, 215, 255 }, new int[] { 0, 219, 255 }, new int[] { 0, 223, 255 }, new int[] { 0, 227, 255 }, new int[] { 0, 231, 255 }, new int[] { 0, 235, 255 }, new int[] { 0, 239, 255 }, new int[] { 0, 243, 255 }, new int[] { 0, 247, 255 }, new int[] { 0, 251, 255 }, new int[] { 0, 255, 255 }, new int[] { 4, 255, 251 }, new int[] { 8, 255, 247 }, new int[] { 12, 255, 243 }, new int[] { 16, 255, 239 }, new int[] { 20, 255, 235 }, new int[] { 24, 255, 231 }, new int[] { 28, 255, 227 }, new int[] { 32, 255, 223 }, new int[] { 36, 255, 219 }, new int[] { 40, 255, 215 }, new int[] { 44, 255, 211 }, new int[] { 48, 255, 207 }, new int[] { 52, 255, 203 }, new int[] { 56, 255, 199 }, new int[] { 60, 255, 195 }, new int[] { 64, 255, 191 }, new int[] { 68, 255, 187 }, new int[] { 72, 255, 183 }, new int[] { 76, 255, 179 }, new int[] { 80, 255, 175 }, new int[] { 84, 255, 171 }, new int[] { 88, 255, 167 }, new int[] { 92, 255, 163 }, new int[] { 96, 255, 159 }, new int[] { 100, 255, 155 }, new int[] { 104, 255, 151 }, new int[] { 108, 255, 147 }, new int[] { 112, 255, 143 }, new int[] { 116, 255, 139 }, new int[] { 120, 255, 135 }, new int[] { 124, 255, 131 }, new int[] { 128, 255, 128 }, new int[] { 131, 255, 124 }, new int[] { 135, 255, 120 }, new int[] { 139, 255, 116 }, new int[] { 143, 255, 112 }, new int[] { 147, 255, 108 }, new int[] { 151, 255, 104 }, new int[] { 155, 255, 100 }, new int[] { 159, 255, 96 }, new int[] { 163, 255, 92 }, new int[] { 167, 255, 88 }, new int[] { 171, 255, 84 }, new int[] { 175, 255, 80 }, new int[] { 179, 255, 76 }, new int[] { 183, 255, 72 }, new int[] { 187, 255, 68 }, new int[] { 191, 255, 64 }, new int[] { 195, 255, 60 }, new int[] { 199, 255, 56 }, new int[] { 203, 255, 52 }, new int[] { 207, 255, 48 }, new int[] { 211, 255, 44 }, new int[] { 215, 255, 40 }, new int[] { 219, 255, 36 }, new int[] { 223, 255, 32 }, new int[] { 227, 255, 28 }, new int[] { 231, 255, 24 }, new int[] { 235, 255, 20 }, new int[] { 239, 255, 16 }, new int[] { 243, 255, 12 }, new int[] { 247, 255, 8 }, new int[] { 251, 255, 4 }, new int[] { 255, 255, 0 }, new int[] { 255, 251, 0 }, new int[] { 255, 247, 0 }, new int[] { 255, 243, 0 }, new int[] { 255, 239, 0 }, new int[] { 255, 235, 0 }, new int[] { 255, 231, 0 }, new int[] { 255, 227, 0 }, new int[] { 255, 223, 0 }, new int[] { 255, 219, 0 }, new int[] { 255, 215, 0 }, new int[] { 255, 211, 0 }, new int[] { 255, 207, 0 }, new int[] { 255, 203, 0 }, new int[] { 255, 199, 0 }, new int[] { 255, 195, 0 }, new int[] { 255, 191, 0 }, new int[] { 255, 187, 0 }, new int[] { 255, 183, 0 }, new int[] { 255, 179, 0 }, new int[] { 255, 175, 0 }, new int[] { 255, 171, 0 }, new int[] { 255, 167, 0 }, new int[] { 255, 163, 0 }, new int[] { 255, 159, 0 }, new int[] { 255, 155, 0 }, new int[] { 255, 151, 0 }, new int[] { 255, 147, 0 }, new int[] { 255, 143, 0 }, new int[] { 255, 139, 0 }, new int[] { 255, 135, 0 }, new int[] { 255, 131, 0 }, new int[] { 255, 128, 0 }, new int[] { 255, 124, 0 }, new int[] { 255, 120, 0 }, new int[] { 255, 116, 0 }, new int[] { 255, 112, 0 }, new int[] { 255, 108, 0 }, new int[] { 255, 104, 0 }, new int[] { 255, 100, 0 }, new int[] { 255, 96, 0 }, new int[] { 255, 92, 0 }, new int[] { 255, 88, 0 }, new int[] { 255, 84, 0 }, new int[] { 255, 80, 0 }, new int[] { 255, 76, 0 }, new int[] { 255, 72, 0 }, new int[] { 255, 68, 0 }, new int[] { 255, 64, 0 }, new int[] { 255, 60, 0 }, new int[] { 255, 56, 0 }, new int[] { 255, 52, 0 }, new int[] { 255, 48, 0 }, new int[] { 255, 44, 0 }, new int[] { 255, 40, 0 }, new int[] { 255, 36, 0 }, new int[] { 255, 32, 0 }, new int[] { 255, 28, 0 }, new int[] { 255, 24, 0 }, new int[] { 255, 20, 0 }, new int[] { 255, 16, 0 }, new int[] { 255, 12, 0 }, new int[] { 255, 8, 0 }, new int[] { 255, 4, 0 }, new int[] { 255, 0, 0 }, new int[] { 251, 0, 0 }, new int[] { 247, 0, 0 }, new int[] { 243, 0, 0 }, new int[] { 239, 0, 0 }, new int[] { 235, 0, 0 }, new int[] { 231, 0, 0 }, new int[] { 227, 0, 0 }, new int[] { 223, 0, 0 }, new int[] { 219, 0, 0 }, new int[] { 215, 0, 0 }, new int[] { 211, 0, 0 }, new int[] { 207, 0, 0 }, new int[] { 203, 0, 0 }, new int[] { 199, 0, 0 }, new int[] { 195, 0, 0 }, new int[] { 191, 0, 0 }, new int[] { 187, 0, 0 }, new int[] { 183, 0, 0 }, new int[] { 179, 0, 0 }, new int[] { 175, 0, 0 }, new int[] { 171, 0, 0 }, new int[] { 167, 0, 0 }, new int[] { 163, 0, 0 }, new int[] { 159, 0, 0 }, new int[] { 155, 0, 0 }, new int[] { 151, 0, 0 }, new int[] { 147, 0, 0 }, new int[] { 143, 0, 0 }, new int[] { 139, 0, 0 }, new int[] { 135, 0, 0 }, new int[] { 131, 0, 0 }, new int[] { 131, 0, 0 } };

            int rows = imageData.GetLength(0);
            int cols = imageData.GetLength(1);

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
                    int d = imageData[i, j];

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopImageAcquisition();
            Environment.Exit(0);
        }

        private void StartImageAcquisition()
        {


            factory = new Jai_FactoryDotNET.CFactory();

            Jai_FactoryDotNET.Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.EFactoryError.Success;
            try
            {
                error = factory.Open("");
            }
            catch (Exception e)
            {
                MessageBox.Show("Priprema za detekciju kamere nije uspjela. " + e.Message, "Detekcija kamere", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            camera = InitializeCamera();
            if (camera != null)
            {

                camera.NewImageDelegate += new Jai_FactoryWrapper.ImageCallBack(HandleImage);
                camera.SkipImageDisplayWhenBusy = false;
                camera.StartImageAcquisition(false, 5);

            }
            else
            {
                MessageBox.Show("Kamera nije spojena.", "Kamera", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                factory.Close();
                factory.Dispose();
                factory = null;
            }

        }

        private void StopImageAcquisition()
        {
            if (camera != null)
            {
                camera.NewImageDelegate -= new Jai_FactoryWrapper.ImageCallBack(HandleImage);
                camera.StopImageAcquisition();
                camera = null;
                factory.Close();
                factory.Dispose();
                factory = null;
            }
        }

        private void SetFramegrabberValue(String nodeName, Int64 int64Val)
        {
            if (null == camera)
            {
                return;
            }

            IntPtr hDevice = IntPtr.Zero;
            Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.J_Camera_GetLocalDeviceHandle(camera.CameraHandle, ref hDevice);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hDevice)
            {
                return;
            }

            IntPtr hNode;
            error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, nodeName, out hNode);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hNode)
            {
                return;
            }

            error = Jai_FactoryWrapper.J_Node_SetValueInt64(hNode, false, int64Val);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            //Special handling for Active Silicon CXP boards, which also has nodes prefixed
            //with "Incoming":
            if ("Width" == nodeName || "Height" == nodeName)
            {
                string strIncoming = "Incoming" + nodeName;
                IntPtr hNodeIncoming;
                error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, strIncoming, out hNodeIncoming);
                if (Jai_FactoryWrapper.EFactoryError.Success != error)
                {
                    return;
                }

                if (IntPtr.Zero == hNodeIncoming)
                {
                    return;
                }

                error = Jai_FactoryWrapper.J_Node_SetValueInt64(hNodeIncoming, false, int64Val);
            }
        }

        private void SetFramegrabberPixelFormat()
        {
            String nodeName = "PixelFormat";

            if (null == camera)
            {
                return;
            }

            IntPtr hDevice = IntPtr.Zero;
            Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.J_Camera_GetLocalDeviceHandle(camera.CameraHandle, ref hDevice);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hDevice)
            {
                return;
            }

            long pf = 0;
            error = Jai_FactoryWrapper.J_Camera_GetValueInt64(camera.CameraHandle, nodeName, ref pf);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }
            UInt64 pixelFormat = (UInt64)pf;

            UInt64 jaiPixelFormat = 0;
            error = Jai_FactoryWrapper.J_Image_Get_PixelFormat(camera.CameraHandle, pixelFormat, ref jaiPixelFormat);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            StringBuilder sbJaiPixelFormatName = new StringBuilder(512);
            uint iSize = (uint)sbJaiPixelFormatName.Capacity;
            error = Jai_FactoryWrapper.J_Image_Get_PixelFormatName(camera.CameraHandle, jaiPixelFormat, sbJaiPixelFormatName, iSize);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            IntPtr hNode;
            error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, nodeName, out hNode);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hNode)
            {
                return;
            }

            error = Jai_FactoryWrapper.J_Node_SetValueString(hNode, false, sbJaiPixelFormatName.ToString());
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            //Special handling for Active Silicon CXP boards, which also has nodes prefixed
            //with "Incoming":
            string strIncoming = "Incoming" + nodeName;
            IntPtr hNodeIncoming;
            error = Jai_FactoryWrapper.J_Camera_GetNodeByName(hDevice, strIncoming, out hNodeIncoming);
            if (Jai_FactoryWrapper.EFactoryError.Success != error)
            {
                return;
            }

            if (IntPtr.Zero == hNodeIncoming)
            {
                return;
            }

            error = Jai_FactoryWrapper.J_Node_SetValueString(hNodeIncoming, false, sbJaiPixelFormatName.ToString());
        }

        private void minTrackBar_ValueChanged(object sender, EventArgs e)
        {
            //minValue = minTrackBar.Value;

            minValue = minData + (int)(((float)minTrackBar.Value / 65535) * ((float)(maxData - minData)));
            histogramDisplay.MinLine = minTrackBar.Value;
        }

        private void maxTrackBar_ValueChanged(object sender, EventArgs e)
        {
            maxValue = minData + (int)(((float)maxTrackBar.Value / 65535) * ((float)(maxData - minData)));
            histogramDisplay.MaxLine = maxTrackBar.Value;
        }

        private void coloringCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (lastImageData != null)
            {
                performColoring = coloringCheckBox.Checked;
                DrawImage(lastImageData);
            }
        }


        private void resolutionTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (!inflamationDetection && !symmetryAnalysis)
                histogramDisplay.Resolution = resolutionTrackBar.Value;
            else if (inflamationDetection)
            {
                Bitmap image = (Bitmap)lastImage.Clone();
                histogramLabel.Text = "> " + (RoundUp(((((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp) + minBodyTemp)) / 100 - 273.15, 2)).ToString() + "°C";
                drawInflamation((int)(((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp) + minBodyTemp), image);
            }
            else
            {
                Bitmap image = (Bitmap)lastImage.Clone();
                histogramLabel.Text = (RoundUp(System.Convert.ToDouble((float)resolutionTrackBar.Value / 256) * tempDiff, 2)).ToString() + "°C";
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Tekstualna datoteka (*.txt)|*.txt|Sve datoteke (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string name = dialog.FileName;

                if (SaveImage(name) == true)
                {
                    lastDirectory = Path.GetDirectoryName(dialog.FileName);
                }
            }
        }

        private void saveFastButton_Click(object sender, EventArgs e)
        {

            if (lastImage != null)
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                if (Directory.Exists(lastDirectory) == false)
                {
                    lastDirectory = Directory.GetCurrentDirectory();
                }
                string path = Path.Combine(lastDirectory, timeStamp + ".t");

                SaveImage(path);
            }

        }

        struct PreviousImageData
        {
            public ushort[,] data;
            public bool performColoring;
            public bool automaticAdjustment;
            public bool inflamationDetection;
            public bool symmetryAnalysis;
            public int min;
            public int max;
            public int resolutionValue;
            public int minTrackBar;
            public int maxTrackBar;
        };

        private bool SaveImage(string name)
        {

            string baseName = name;

            if (name.IndexOf(".") != -1)
            {
                baseName = name.Substring(0, name.LastIndexOf("."));
            }

            if (lastImage != null)
            {
                if (inflamationDetection)
                {
                    return save(lastImageInflamation, baseName);
                }
                else if (symmetryAnalysis)
                {
                    return save(lastImageSymmetry, baseName);
                }
                else
                {
                    return save(lastImage, baseName);
                }
            }
            else
            {
                MessageBox.Show("Slika nije spremljena!", "Spremanje slike", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

        }

        private bool save(Bitmap image, string baseName)
        {
            PictureBox last = new PictureBox();
            PreviousImageData pid = new PreviousImageData();
            pid.data = lastImageData;
            pid.performColoring = performColoring;
            pid.automaticAdjustment = automaticAdjustment;
            pid.min = minValue;
            pid.max = maxValue;
            pid.inflamationDetection = inflamationDetection;
            pid.symmetryAnalysis = symmetryAnalysis;
            pid.resolutionValue = resolutionTrackBar.Value;
            pid.minTrackBar = minTrackBar.Value;
            pid.maxTrackBar = maxTrackBar.Value;

            last.Height = previousImagesPanel.Height - 25;
            last.Width = last.Height * 360 / 240;
            last.Image = (Bitmap)image.Clone();
            last.SizeMode = PictureBoxSizeMode.StretchImage;
            last.Tag = pid;
            last.Cursor = Cursors.Hand;
            last.Click += delegate (System.Object o, System.EventArgs e)
            {
                PreviousImageData previous = (PreviousImageData)(((PictureBox)o).Tag);
                performColoring = previous.performColoring;
                coloringCheckBox.Checked = performColoring;
                automaticAdjustment = previous.automaticAdjustment;
                automaticAdjustmentCheckBox.Checked = automaticAdjustment;
                minValue = previous.min;
                maxValue = previous.max;

                inflamationDetection = previous.inflamationDetection;
                if (inflamationDetection)
                {
                    inflamationDetectionToolStripMenuItem.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    inflamationDetectionToolStripMenuItem.BackColor = Color.White;
                    histogramLabel.Text = "Histogram";
                }
                symmetryAnalysis = previous.symmetryAnalysis;
                if (symmetryAnalysis)
                {
                    legSymetToolStripMenuItem.BackColor = Color.DeepSkyBlue;
                    coloringCheckBox.Checked = false;
                    performColoring = false;
                    coloringCheckBox.Enabled = false;

                    automaticAdjustmentCheckBox.Enabled = false;


                    histogramLabel.Text = (RoundUp(System.Convert.ToDouble((((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp)) / 100), 2)).ToString() + "°C";

                }
                else
                {
                    legSymetToolStripMenuItem.BackColor = Color.White;
                    histogramLabel.Text = "Histogram";
                }
                minTrackBar.Value = previous.minTrackBar;
                maxTrackBar.Value = previous.maxTrackBar;

                resolutionTrackBar.Value = previous.resolutionValue;
                lastImageData = previous.data;
                DrawImage(lastImageData);
            };

            previousImagesPanel.Controls.Add(last);
            Button dummyButton = new Button();
            previousImagesPanel.Controls.Add(dummyButton);
            previousImagesPanel.ScrollControlIntoView(dummyButton);
            previousImagesPanel.Controls.Remove(dummyButton);

            Stream outputStream = new FileStream(baseName + ".png", FileMode.Create);
            image.Save(outputStream, ImageFormat.Png);
            outputStream.Close();

            StreamWriter textOutput = new StreamWriter(new FileStream(baseName + ".txt", FileMode.Create));

            int rows = lastImageData.GetLength(0);
            int cols = lastImageData.GetLength(1);

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    ushort d = lastImageData[i, j];
                    if (j != 0)
                    {
                        textOutput.Write(" ");
                    }
                    textOutput.Write((d / 100).ToString() + "." + (d % 100).ToString());
                }
                textOutput.Write("\n");
            }

            textOutput.Close();

            return true;
        }

        private void automaticAdjustmentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            automaticAdjustment = automaticAdjustmentCheckBox.Checked;
            if (automaticAdjustment)
            {
                minTrackBar.Value = 0;
                maxTrackBar.Value = 65535;
                minTrackBar.Enabled = false;
                maxTrackBar.Enabled = false;
            }
            else
            {
                //minTrackBar.Value = minValue;
                //maxTrackBar.Value = maxValue;
                minTrackBar.Enabled = true;
                maxTrackBar.Enabled = true;
            }

            DrawImage(lastImageData);

        }

        private void openButton_Click(object sender, EventArgs e)
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

                lastImageData = removeHeader(lastImageData);
                DrawImage(lastImageData);
                saveButton.Enabled = true;
                saveFastButton.Enabled = true;

            }

        }

        private void fetchButton_Click(object sender, EventArgs e)
        {
            FlipAcquisitionState();
            fetchButton.Focus();
        }

        private void FlipAcquisitionState()
        {
            fetchButton.Enabled = false;
            if (camera == null)
            {
                 StartImageAcquisition();
                if (camera != null)
                {
                    fetchButton.Text = "Zaustavi";
                    fetchButton.BackColor = Color.FromArgb(255, 232, 232);
                    openButton.Enabled = false;
                    saveButton.Enabled = true;
                    saveFastButton.Enabled = true;
                    autofocusButton.Enabled = true;
                    focusNearButton.Enabled = true;
                    focusFarButton.Enabled = true;
                }
            }
            else
            {
                StopImageAcquisition();
            }

            if (camera == null)
            {
                fetchButton.Text = "Pokreni";
                fetchButton.BackColor = Color.FromArgb(232, 255, 232);
                openButton.Enabled = true;
                autofocusButton.Enabled = false;
                focusNearButton.Enabled = false;
                focusFarButton.Enabled = false;
            }

            fetchButton.Enabled = true;
        }

        private void minTrackBar_Scroll(object sender, EventArgs e)
        {
            if (camera == null)
            {
                DrawImage(lastImageData);
            }
        }

        private void maxTrackBar_Scroll(object sender, EventArgs e)
        {
            if (camera == null)
            {
                DrawImage(lastImageData);
            }
        }

        private void imageDisplay_MouseLeave(object sender, EventArgs e)
        {
            if (!inflamationDetection)
            {
                temperatureLabel.Text = "-";
            }
            else
            {
                drawInflamation((int)(((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp) + minBodyTemp), (Bitmap)lastImage.Clone());
            }
        }

        private void imageDisplay_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button != MouseButtons.Left)
            {
                selecting = false;
            }

            if (lastImageData == null)
            {
                return;
            }

            int x = e.X;
            int y = e.Y;

            int rows = lastImageData.GetLength(0);
            int cols = lastImageData.GetLength(1);

            int row = (int)(rows * e.Y / (double)imageDisplay.Height);
            int col = (int)(cols * e.X / (double)imageDisplay.Width);

            if (row < 0)
            {
                row = 0;
            }
            if (row >= rows)
            {
                row = rows - 1;
            }
            if (col < 0)
            {
                col = 0;
            }
            if (col >= cols)
            {
                col = cols - 1;
            }

            int data = lastImageData[row, col];

            if (data == 0)
            {
                temperatureLabel.Text = "-";
            }
            else
            {
                //double temperatureK = scaleLimitLow + (data - minData) / (double)(maxData - minData) * (scaleLimitUpper - scaleLimitLow);

                /*
                double PR1=21106.77;
                double PB=1501;
                double PF=1;
                double PO=-7340;
                double PR2=0.012545258;
                
                double temperatureK=PB/Math.Log(PR1 / (PR2 * (data + PO)) + PF);
                */

                /*
                double R=16066.8095703125;
                double B = 1419;
                double F = 1;
                double O = 18800;
                double temperatureK = B / Math.Log(R / (data- O) + F);
                */

                double temperatureK = data / 100.0;
                double temperatureC = temperatureK - 273.15;

                if (selecting)
                {
                    minRectangleX = initialX;
                    maxRectangleX = initialX;
                    minRectangleY = initialY;
                    maxRectangleY = initialY;

                    if (x < minRectangleX)
                    {
                        minRectangleX = x;
                    }
                    if (maxRectangleX < x)
                    {
                        maxRectangleX = x;
                    }

                    if (y < minRectangleY)
                    {
                        minRectangleY = y;
                    }
                    if (maxRectangleY < y)
                    {
                        maxRectangleY = y;
                    }

                }

                string meanTemperature = "";
                if (selecting)
                {
                    int minRow = (int)(rows * minRectangleY / (double)imageDisplay.Height);
                    int minCol = (int)(cols * minRectangleX / (double)imageDisplay.Width);
                    int maxRow = (int)(rows * maxRectangleY / (double)imageDisplay.Height);
                    int maxCol = (int)(cols * maxRectangleX / (double)imageDisplay.Width);

                    if (minRow < 0)
                    {
                        minRow = 0;
                    }
                    if (minRow >= rows)
                    {
                        minRow = rows - 1;
                    }
                    if (maxRow < 0)
                    {
                        maxRow = 0;
                    }
                    if (maxRow >= rows)
                    {
                        maxRow = rows - 1;
                    }

                    if (minCol < 0)
                    {
                        minCol = 0;
                    }
                    if (minCol >= cols)
                    {
                        minCol = cols - 1;
                    }
                    if (maxCol < 0)
                    {
                        maxCol = 0;
                    }
                    if (maxCol >= cols)
                    {
                        maxCol = cols - 1;
                    }


                    if (inflamationDetection)
                    {
                        inflamationROI = 0;
                        inflamationROIaverageTemperature = 0;
                        for (int i = minRow; i < maxRow; i++)
                        {
                            for (int j = minCol; j < maxCol; j++)
                            {
                                if (lastImageInflamation.GetPixel(j, i).GetBrightness() == 0)
                                {
                                    inflamationROIaverageTemperature += lastImageData[i, j];
                                    inflamationROI++;
                                }
                            }
                        }
                        if (inflamationROIaverageTemperature != 0)
                        {
                            inflamationROIaverageTemperature = RoundUp(inflamationROIaverageTemperature / ((double)inflamationROI * 100) - 273.15, 2);
                        }
                    }

                    double meanTemperatureK = lastIntegralImageData[maxRow + 1, maxCol + 1] - lastIntegralImageData[maxRow + 1, minCol] - lastIntegralImageData[minRow, maxCol + 1] + lastIntegralImageData[minRow, minCol];
                    //double meanTemperatureK = 0;
                    meanTemperatureK /= (maxRow - minRow + 1) * (maxCol - minCol + 1) * 100;
                    double meanTemperatureC = meanTemperatureK - 273.15;

                    meanTemperature = "        srednje: " + string.Format("{0:N2} °C", meanTemperatureC) + "    " + string.Format("{0:N2} K", meanTemperatureK);

                }
                if (inflamationDetection && selecting)
                {
                    temperatureLabel.Text = " ROI: " + inflamationROI.ToString() + "px" + "   Prosječna temperatura ROI: " + inflamationROIaverageTemperature.ToString() + "°C";
                }
                else
                {
                    temperatureLabel.Text = string.Format("{0:N2} °C", temperatureC) + "    " + string.Format("{0:N2} K", temperatureK) + meanTemperature;
                }

                imageDisplay.Invalidate();
            }


        }

        private void imageDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (selecting == false)
            {
                selecting = true;
                initialX = e.X;
                initialY = e.Y;
            }
        }

        private void imageDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            selecting = false;
        }

        private void imageDisplay_Paint(object sender, PaintEventArgs e)
        {
            if (selecting)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red, 2), minRectangleX, minRectangleY, maxRectangleX - minRectangleX, maxRectangleY - minRectangleY);
            }

        }

        private void focusNearButton_Click(object sender, EventArgs e)
        {
            if (camera != null)
            {
                byte[] followingCommand = { 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1a, 0x11, 0x02, 0x2e, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x66, 0x6f, 0x63, 0x75, 0x73, 0x2e, 0x73, 0x70, 0x65, 0x65, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00 };
                ExecuteCommand(followingCommand);
            }
        }

        private void focusFarButton_Click(object sender, EventArgs e)
        {
            if (camera != null)
            {
                byte[] followingCommand = { 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1a, 0x11, 0x02, 0x2e, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x66, 0x6f, 0x63, 0x75, 0x73, 0x2e, 0x73, 0x70, 0x65, 0x65, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00 };
                ExecuteCommand(followingCommand);
            }

        }

        private void autofocusButton_Click(object sender, EventArgs e)
        {
            if (camera != null)
            {
                byte[] initialCommand = { 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1a, 0x11, 0x01, 0x2e, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x66, 0x6f, 0x63, 0x75, 0x73, 0x2e, 0x61, 0x75, 0x74, 0x6f, 0x66, 0x75, 0x6c, 0x6c, 0x00, 0x01 };
                ExecuteCommand(initialCommand);
            }
        }

        private void ExecuteCommand(byte[] initialCommand)
        {
            new Thread(delegate ()
            {
                try
                {
                    int port = 22136;
                    string address = camera.IPAddress.ToString();
                    TcpClient tcpClient;
                    while (true)
                    {
                        tcpClient = new TcpClient();
                        if (tcpClient.ConnectAsync(address, port).Wait(300))
                        {
                            break;
                        }
                        Console.WriteLine("Connection failed!");
                    }
                    Stream stream = tcpClient.GetStream();

                    stream.Write(initialCommand, 0, initialCommand.Length);
                    stream.Flush();

                    byte[] answer = new byte[1025];
                    int n = stream.Read(answer, 0, answer.Length);

                    stream.Close();
                    tcpClient.Close();

                    if (answer[8] != 0x12)
                    {
                        tcpClient = new TcpClient(address, port);
                        stream = tcpClient.GetStream();

                        byte[] nextCommand = { 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0xd0, 0x04, 0x72, 0x6f, 0x6f, 0x74, 0x05, 0x33, 0x76, 0x6c, 0x69, 0x67 };
                        stream.Write(nextCommand, 0, nextCommand.Length);
                        stream.Flush();

                        n = stream.Read(answer, 0, answer.Length);

                        stream.Close();
                        tcpClient.Close();




                        tcpClient = new TcpClient(address, port);
                        stream = tcpClient.GetStream();

                        byte[] finalCommand = initialCommand;
                        stream.Write(finalCommand, 0, finalCommand.Length);
                        stream.Flush();

                        n = stream.Read(answer, 0, answer.Length);

                        stream.Close();
                        tcpClient.Close();
                    }
                }
                catch (SocketException socketException)
                {
                    Console.WriteLine("Autofocus communication failure!");
                }
            }).Start();

        }

        private void focusNearButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (camera != null && e.Button == MouseButtons.Left)
            {
                byte[] initialCommand = { 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1a, 0x11, 0x02, 0x2e, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x66, 0x6f, 0x63, 0x75, 0x73, 0x2e, 0x73, 0x70, 0x65, 0x65, 0x64, 0x00, 0xff, 0xff, 0xff, 0xce };
                ExecuteCommand(initialCommand);
            }
        }

        private void focusFarButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (camera != null && e.Button == MouseButtons.Left)
            {
                byte[] initialCommand = { 0xfe, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1a, 0x11, 0x02, 0x2e, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x66, 0x6f, 0x63, 0x75, 0x73, 0x2e, 0x73, 0x70, 0x65, 0x65, 0x64, 0x00, 0x00, 0x00, 0x00, 0x32 };
                ExecuteCommand(initialCommand);
            }
        }



        private ushort[,] removeHeader(ushort[,] ImageData)
        {
            int rows = ImageData.GetLength(0);
            int cols = ImageData.GetLength(1);

            minData = 65525;
            maxData = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    ushort value = ImageData[i, j];

                    if (value == 0)
                        ImageData[i, j] = 1000;
                    else
                    {
                        if (value < minData)
                            minData = value;
                        if (value > maxData)
                            maxData = value;
                    }
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (ImageData[i, j] == 1000)
                        ImageData[i, j] = (ushort)minData;
                }
            }

            return ImageData;

        }

        void drawInflamation(int inflamationRange, Bitmap image)
        {


            if (inflamationDetection)
            {

                int totalTemp = 0;
                double meanTemp = 0;
                int pixelNum = 0;

                for (int i = 0; i < lastImageData.GetLength(0); i++)
                {
                    for (int j = 0; j < lastImageData.GetLength(1); j++)
                    {
                        if (lastImageData[i, j] > inflamationRange)
                        {
                            ++pixelNum;
                            totalTemp += lastImageData[i, j];
                            image.SetPixel(j, i, Color.Black);
                        }
                    }
                    if (pixelNum != 0)
                    {
                        meanTemp = (double)totalTemp / (pixelNum * 100);
                        meanTemp -= 273.15;
                        meanTemp = RoundUp(meanTemp, 2);
                        temperatureLabel.Text = "Srednja T označeno = " + meanTemp.ToString() + "°C  Površina: " + pixelNum + "px ";

                    }
                    else
                        temperatureLabel.Text = "Srednja T = 0°C  Površina: 0px";
                }

                imageDisplay.Image = image;
                lastImageInflamation = (Bitmap)image.Clone();
            }
        }

        private void inflamationDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastImageData != null)
            {

                Bitmap image = (Bitmap)lastImage.Clone();

                if (inflamationDetection)
                {
                    inflamationDetection = false;
                    inflamationDetectionToolStripMenuItem.BackColor = Color.White;
                    histogramLabel.Text = "Histogram:";
                    temperatureLabel.Text = "-";
                    DrawImage(lastImageData);
                }
                else
                {
                    inflamationDetectionToolStripMenuItem.BackColor = Color.DeepSkyBlue;
                    legSymetToolStripMenuItem.BackColor = Color.White;
                    symmetryAnalysis = false;
                    inflamationDetection = true;
                    coloringCheckBox.Enabled = true;
                    automaticAdjustmentCheckBox.Enabled = true;

                    histogramLabel.Text = (RoundUp(System.Convert.ToDouble((((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp) + minBodyTemp) / 100 - 273.15), 2)).ToString() + "°C";

                    drawInflamation((int)(((float)resolutionTrackBar.Value / 256) * (maxData - minBodyTemp) + minBodyTemp), image);
                }

            }
        }

        public static double RoundUp(double input, int places)
        {
            Double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        int[] findPeaks(int[] histogram)
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

        int removeBackground(Bitmap image)
        {
            int[] histogram = new int[histogramData.Length];

            histogram = (int[])histogramData.Clone();

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

            histogram = (int[])histogramData.Clone();

            int minimum = 65535;
            int a = 0;
            for (int i = peak1; i < peak2; i++)
            {
                if (histogram[i] < minimum)
                {
                    minimum = histogram[i];
                    a = i;
                }
            }

            int average = 0;
            int pixels = 0;
            for (int i = 0; i < lastImageData.GetLength(0); i++)
            {
                for (int j = 0; j < lastImageData.GetLength(1); j++)
                {
                    if (lastImageData[i, j] < a + minData)
                    {
                        image.SetPixel(j, i, Color.White);
                    }
                    else
                    {
                        average += lastImageData[i, j];
                        pixels++;
                    }
                }
            }

            averageBodyTemp = RoundUp(((double)average / pixels) / 100 - 273.15, 2);

            lastImageBackgroundRemoved = (Bitmap)image.Clone();

            return a + minData;
        }

        private void averageTemp_MouseHover(object sender, EventArgs e)
        {
            if (!symmetryAnalysis)
            {
                if (inflamationDetection)
                {
                    removeBackground(lastImageInflamation);
                }

                imageDisplay.Image = lastImageBackgroundRemoved;
            }
        }

        private void averageTemp_MouseLeave(object sender, EventArgs e)
        {
            if (!symmetryAnalysis)
            {
                if (inflamationDetection)
                {
                    imageDisplay.Image = lastImageInflamation;
                }
                else
                {
                    imageDisplay.Image = lastImage;
                }
            }
        }

        private void legSymetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastImageData != null)
            {


                if (symmetryAnalysis)
                {
                    symmetryAnalysis = false;
                    legSymetToolStripMenuItem.BackColor = Color.White;
                    histogramLabel.Text = "Histogram:";
                    temperatureLabel.Text = "-";
                    coloringCheckBox.Checked = true;
                    performColoring = true;
                    coloringCheckBox.Enabled = true;
                    automaticAdjustmentCheckBox.Enabled = true;

                }
                else
                {

                    // Disable coloring
                    coloringCheckBox.Checked = false;
                    performColoring = false;
                    coloringCheckBox.Enabled = false;

                    automaticAdjustmentCheckBox.Enabled = false;
                    symmetryAnalysis = true;

                    legSymetToolStripMenuItem.BackColor = Color.DeepSkyBlue;
                    inflamationDetectionToolStripMenuItem.BackColor = Color.White;
                    inflamationDetection = false;
                    

                }
            }

        }







  
       

    }

}