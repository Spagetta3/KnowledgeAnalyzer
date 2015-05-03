using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class LearningObject
    {
        private string id;
        private double ari;
        private double lix;
        private int length;

        public string ID { get { return id; } set { id = value; } }
        public double Ari { get { return ari; } set { ari = value; } }
        public double Lix { get { return lix; } set { lix = value; } }
        public int Length { get { return length; } set { length = value; } }
    }
}
