using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termographic_image_analysis
{
    class Organizer
    {


        private List<Analysis> analyses;
        private Camera_stream stream;
        private Analysis active_analysis;


        public Organizer(Camera_stream stream)
        {
            analyses = new List<Analysis>();
            this.stream = stream;
        }

        public void Add_analysis(Analysis analysis)
        {
            analyses.Add(analysis);
        }

        public Bitmap Analize(AnalysisTypes.AnalysisType type)
        {
            return null;
        }

        

    }
}
