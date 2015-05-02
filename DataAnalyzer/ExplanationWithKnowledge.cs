using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class ExplanationWithKnowledge
    {
        private string explanationID;
        private string userID;
        private double knowledge;

        public ExplanationWithKnowledge()
        {
            this.knowledge = 0;
        }

        public string ExplanationID { get { return explanationID; } set { explanationID = value; } }
        public string UserID { get { return userID; } set { userID = value; } }
        public double Knowledge { get { return knowledge; } set { knowledge = value; } }
    }
}
