using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class ConceptWithExplanations
    {
        private string conceptID;
        private List<ExplanationWithWeight> explanationsID;

        public ConceptWithExplanations()
        {
            explanationsID = new List<ExplanationWithWeight>();
        }

        public string ConceptID { get { return conceptID; } set { conceptID = value; } }
        public List<ExplanationWithWeight> ExplanationsID { get { return explanationsID; } set { explanationsID = value; } }
    }
}
