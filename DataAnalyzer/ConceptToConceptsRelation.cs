using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class ConceptToConceptsRelation
    {
        private string conceptID;
        private List<Concept_TypeAndWeight> relatedConcepts;

        public ConceptToConceptsRelation()
        {
            relatedConcepts = new List<Concept_TypeAndWeight>();
        }

        public string ConceptID { get { return conceptID; } set { conceptID = value; } }
        public List<Concept_TypeAndWeight> RelatedConcepts { get { return relatedConcepts; } set { relatedConcepts = value; } }
    }
}
