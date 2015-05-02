using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class QuestionAndExplanations
    {
        private string id;
        private List<string> loIDs;
        private double difficulty;

        public QuestionAndExplanations()
        {
            this.loIDs = new List<string>();
        }

        public string ID { get { return id; } set { id = value; } }
        public List<string> LoIDs { get { return loIDs; } set { loIDs = value; } }
        public double Difficulty { get { return difficulty; } set { difficulty = value; } }
    }
}
