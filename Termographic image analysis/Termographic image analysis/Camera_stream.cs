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


    }
}
