using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class Concept_TypeAndWeight
    {
        private string conceptID;
        private double weight;
        private string type;

        public Concept_TypeAndWeight() { }
        public Concept_TypeAndWeight(string conceptID, string type, double weight)
        {
            this.conceptID = conceptID;
            this.type = type;
            this.weight = weight;
        }

        public string ConceptID { get { return conceptID; } set { conceptID = value; } }
        public double Weight { get { return weight; } set { weight = value; } }
        public string Type { get { return type; } set { type = value; } }

    }
}
