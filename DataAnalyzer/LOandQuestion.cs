using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class LOandQuestion
    {
        private string questionID;
        private string explanationID;
        private string conceptID;
        private string conceptName;
        private double weightQuestion;
        private double weightExplanation;
        private double questionDifficulty;
        private string questionLabel;
        private string explanationLabel;

        public LOandQuestion()
        {}

        public LOandQuestion(string questionID, string conceptID, string conceptName, double weightQuestion, double questionDifficulty, string questionLabel)
        {
            this.questionID = questionID;
            this.conceptID = conceptID;
            this.conceptName = conceptName;
            this.weightQuestion = weightQuestion;
            this.questionDifficulty = questionDifficulty;
            this.questionLabel = questionLabel;
        }


        public string QuestionID { get { return questionID; } set { questionID = value; } }
        public string ExplanationID { get { return explanationID; } set { explanationID = value; } }
        public string ConceptID { get { return conceptID; } set { conceptID = value; } }
        public string ConceptName { get { return conceptName; } set { conceptName = value; } }
        public double WeightQuestion { get { return weightQuestion; } set { weightQuestion = value; } }
        public double WeightExplanation { get { return weightExplanation; } set { weightExplanation = value; } }
        public double QuestionDifficulty { get { return questionDifficulty; } set { questionDifficulty = value; } }
        public string QuestionLabel { get { return questionLabel; } set { questionLabel = value; } }
        public string ExplanationLabel { get { return explanationLabel; } set { explanationLabel = value; } }
    }
}
