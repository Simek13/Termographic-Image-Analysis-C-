using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jai_FactoryDotNET;
using System.Net.Sockets;
using System.Threading;

namespace Termographic_image_analysis
{
    class Camera_stream : Image_fetching
    {
        
        private CFactory factory;
        private CCamera camera;

        public Camera_stream(CFactory factory, CCamera camera)
        {
            this.factory = factory;
            this.camera = camera;
        }



        private bool StartImageAcquisition()
        {


            factory = new Jai_FactoryDotNET.CFactory();

            Jai_FactoryDotNET.Jai_FactoryWrapper.EFactoryError error = Jai_FactoryWrapper.EFactoryError.Success;
            try
            {
                error = factory.Open("");
            }
            catch (Exception e)
            {
                //MessageBox.Show("Priprema za detekciju kamere nije uspjela. " + e.Message, "Detekcija kamere", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
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
                //MessageBox.Show("Kamera nije spojena.", "Kamera", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                factory.Close();
                factory.Dispose();
                factory = null;
                return false;
            }
            return true;

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
                //MessageBox.Show("Detekcija kamere nije uspjela. " + e.Message, "Detekcija kamere", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

    }
}
