using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class LOeyeTracker
    {
        private string id;
        private int fixationCount;
        private int saccadeCount;
        private double activeTime;

        public string ID { get { return id; } set { id = value; } }
        public int FixationCount { get { return fixationCount; } set { fixationCount = value; } }
        public int SaccadeCount { get { return saccadeCount; } set { saccadeCount = value; } }
        public double ActiveTime { get { return activeTime; } set { activeTime = value; } }
    }
}
