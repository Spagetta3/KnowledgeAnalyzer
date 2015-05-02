using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class LOandConcept
    {
        private string lo_ID;
        private string conceptID;

        public LOandConcept()
        { }

        public LOandConcept(string lo_ID, string conceptID)
        {
            this.lo_ID = lo_ID;
            this.conceptID = conceptID;
        }

        public string Lo_ID { get { return lo_ID; } set { lo_ID = value; } }
        public string ConceptID { get { return conceptID; } set { conceptID = value; } }
    }
}
