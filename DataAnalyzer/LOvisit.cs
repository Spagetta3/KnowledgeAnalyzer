using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class LOvisit
    {
        private string id_lo;
        private string id_user;
        private double activeTime;
        private double wholeTime;
        private double eyeTime;
        private int counter;
        private int numbOfClicks;
        private int numbOfScrolls;
        private double ari;
        private double lix;
        private double mouseActiveTime;

        public string Id_lo { get { return id_lo; } set { id_lo = value; } }
        public string Id_user { get { return id_user; } set { id_user = value; } }
        public double ActiveTime { get { return activeTime; } set { activeTime = value; } }
        public double WholeTime { get { return wholeTime; } set { wholeTime = value; } }
        public double EyeTime { get { return eyeTime; } set { eyeTime = value; } }
        public int Counter { get { return counter; } set { counter = value; } }
        public int NumbOfClicks { get { return numbOfClicks; } set { numbOfClicks = value; } }
        public int NumbOfScrolls { get { return numbOfScrolls; } set { numbOfScrolls = value; } }
        public double Ari { get { return ari; } set { ari = value; } }
        public double Lix { get { return lix; } set { lix = value; } }
        public double MouseActiveTime { get { return mouseActiveTime; } set { mouseActiveTime = value; } }
    }
}
