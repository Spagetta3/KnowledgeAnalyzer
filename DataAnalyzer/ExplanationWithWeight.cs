using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class ExplanationWithWeight
    {
        private string explanationID;
        private double weight;

        public string ExplanationID { get { return explanationID; } set { explanationID = value; } }
        public double Weight { get { return weight; } set { weight = value; } }
    }
}
