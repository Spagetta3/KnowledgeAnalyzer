using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    class Program
    {
        public static DateTime _jan1st1970 = new DateTime(1970, 1, 1);
        static void Main(string[] args)
        {
            GetKnowledgeForTests();
        }

        public static void GetKnowledgeForTests()
        {
            HashSet<QuestionAndExplanations> qae = new HashSet<QuestionAndExplanations>();
            HashSet<ConceptToConceptsRelation> concepts = new HashSet<ConceptToConceptsRelation>();
            List<string> loids = new List<string>();
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\alef\\otázky_testyLisp_texty.txt";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;
            string id = null;
            bool firstTime = true;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');

                    if (firstTime)
                    {
                        firstTime = false;
                        id = words[0];
                        QuestionAndExplanations q = new QuestionAndExplanations();
                        q.ID = id;
                        q.Difficulty = Double.Parse(words[5].Replace('.', ','));

                        for (int i = 0; i < Int32.Parse(words[6]); i++)
                            q.LoIDs.Add(words[7 + i]);

                        qae.Add(q);
                        continue;
                    }

                    if (words[0] == id)
                        continue;
                    else
                    {
                        id = words[0];
                        QuestionAndExplanations q = new QuestionAndExplanations();
                        q.ID = id;
                        q.Difficulty = Double.Parse(words[5].Replace('.', ','));

                        for (int i = 0; i < Int32.Parse(words[6]); i++)
                            q.LoIDs.Add(words[7 + i]);

                        qae.Add(q);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
                file.Close();
            }

            // nacitanie id s ktorymi pracujeme
            path = "C:\\Users\\Veronika\\Desktop\\LO\\lo_ids.txt";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    loids.Add(words[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            file.Close();
            
            // nacitanie vztahov medzi konceptami
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\alef\\conceptWithConcepts.csv";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    
                    ConceptToConceptsRelation c = new ConceptToConceptsRelation();
                    c.ConceptID = words[1];
                    Concept_TypeAndWeight ctw = new Concept_TypeAndWeight(words[2], words[3], Double.Parse(words[4].Replace('.', ',')));
                    c.RelatedConcepts.Add(ctw);
                    
                    bool added = false;

                    foreach (ConceptToConceptsRelation value in concepts)
                    {
                        if (value.ConceptID == c.ConceptID)
                        {
                            bool addedRelation = false;

                            foreach (Concept_TypeAndWeight v in value.RelatedConcepts)
                            {
                                if (v.ConceptID == words[2])
                                {
                                    added = true;
                                    addedRelation = true;
                                    break;
                                }
                            }

                            if (!addedRelation)
                            {
                                value.RelatedConcepts.Add(ctw);
                                added = true;
                            }
                        }  
                    }

                    if (!added)
                        concepts.Add(c);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            file.Close();

            // nacitanie explanations a konceptov
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\alef\\conceptWithLO.csv";
            file = new System.IO.StreamReader(path);
            HashSet<ConceptWithExplanations> conceptsAndExplanations = new HashSet<ConceptWithExplanations>();

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    ConceptWithExplanations cwe = new ConceptWithExplanations();
                    cwe.ConceptID = words[1];
                    ExplanationWithWeight e = new ExplanationWithWeight();
                    e.ExplanationID = words[2];
                    e.Weight = Double.Parse(words[4].Replace('.', ','));
                    cwe.ExplanationsID.Add(e);

                    bool isInSetup = false;

                    foreach (string value in loids)
                    {
                        if (e.ExplanationID == value)
                        {
                            isInSetup = true;
                            break;
                        }
                    }

                    if (isInSetup)
                    {
                        bool added = false;

                        foreach (ConceptWithExplanations value in conceptsAndExplanations)
                        {
                            if (value.ConceptID == cwe.ConceptID)
                            {
                                bool addedRelation = false;

                                foreach (ExplanationWithWeight v in value.ExplanationsID)
                                {
                                    if (v.ExplanationID == words[2])
                                    {
                                        added = true;
                                        addedRelation = true;
                                        break;
                                    }
                                }

                                if (!addedRelation)
                                {
                                    value.ExplanationsID.Add(e);
                                    added = true;
                                }
                            }
                        }

                        if (!added)
                            conceptsAndExplanations.Add(cwe);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                } 
            }

            file.Close();

            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Testy-Lisp\\Vyhodnotenie\\names.txt";
            file = new System.IO.StreamReader(path);

            List<string> names = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    names.Add(line);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            file.Close();
            HashSet<ExplanationWithKnowledge> firstKnowledge = new HashSet<ExplanationWithKnowledge>();
            HashSet<ExplanationWithKnowledge> secondKnowledge = new HashSet<ExplanationWithKnowledge>();
            HashSet<ExplanationWithKnowledge> thirdKnowledge = new HashSet<ExplanationWithKnowledge>();
            HashSet<ExplanationWithKnowledge> fourthKnowledge = new HashSet<ExplanationWithKnowledge>();
            HashSet<ExplanationWithKnowledge> fifthKnowledge = new HashSet<ExplanationWithKnowledge>();

            string iteration = "first";
            int countIteration = 0;

            foreach (string value in names)
            {
                path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Testy-Lisp\\Vyhodnotenie\\" + value;
                file = new System.IO.StreamReader(path);
                firstTime = true;
                int count = 0;
                string[] questions = new string[12];
                int countLine = 0;
                
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        string[] words = line.Split(';');
                        if (firstTime)
                        {
                            count = Int32.Parse(words[2]);
                            for (int i = 0; i < count; i++)
                                questions[i] = words[3+i];

                            firstTime = false;
                            continue;
                        }

                        for (int i = 0; i < questions.Length; i++)
                        {
                            if (questions[i] == null)
                                break;

                            double knowledge = 0;
                            List<string> explanationsToQuestion = null;
                            double difficulty = 0;

                            foreach (QuestionAndExplanations v in qae)
                            {
                                if (v.ID == questions[i])
                                {
                                    explanationsToQuestion = v.LoIDs;
                                    difficulty = v.Difficulty;
                                    break;
                                }
                            }

                            if (words[2+i] == "T")
                            {
                                knowledge = difficulty + Math.Sqrt(difficulty);
                                foreach (string e in explanationsToQuestion)
                                { 
                                    ExplanationWithKnowledge newKnowledge = new ExplanationWithKnowledge();
                                    newKnowledge.ExplanationID = e;
                                    newKnowledge.Knowledge = knowledge;
                                    newKnowledge.UserID = words[1];

                                    if (iteration == "first")
                                        AddKnowledge(newKnowledge, firstKnowledge);
                                    else if (iteration == "second")
                                        AddKnowledge(newKnowledge, secondKnowledge);
                                    else if (iteration == "third")
                                        AddKnowledge(newKnowledge, thirdKnowledge);
                                    else if (iteration == "fourth")
                                        AddKnowledge(newKnowledge, fourthKnowledge);
                                    else if (iteration == "fifth")
                                        AddKnowledge(newKnowledge, fifthKnowledge);
                                    
                                    double e1 = 1;
                                    double e2 = 1;
                                    double newEnergy = 0;

                                    if (knowledge >= 0)
                                        newEnergy = e1 + 1 * e2 * knowledge;
                                    else
                                        newEnergy = e1 + 1 * e2 * (-1 * knowledge);

                                    string oldConcept = GetBestConcept(e, conceptsAndExplanations);
                                    List<string> oldExplanations = new List<string>();
                                    oldExplanations.Add(e);

                                    if (iteration == "first")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, firstKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "second")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, secondKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "third")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, thirdKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "fourth")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, fourthKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "fifth")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, fifthKnowledge, concepts, conceptsAndExplanations);
                                }
                            }
                            else if (words[2 + i] == "F" || words[2 + i] == "N")
                            {
                                knowledge = difficulty + Math.Sqrt(difficulty);
                                foreach (string e in explanationsToQuestion)
                                {
                                    ExplanationWithKnowledge newKnowledge = new ExplanationWithKnowledge();
                                    newKnowledge.ExplanationID = e;
                                    newKnowledge.Knowledge -= knowledge;
                                    newKnowledge.UserID = words[1];

                                    if (iteration == "first")
                                        AddKnowledge(newKnowledge, firstKnowledge);
                                    else if (iteration == "second")
                                        AddKnowledge(newKnowledge, secondKnowledge);
                                    else if (iteration == "third")
                                        AddKnowledge(newKnowledge, thirdKnowledge);
                                    else if (iteration == "fourth")
                                        AddKnowledge(newKnowledge, fourthKnowledge);
                                    else if (iteration == "fifth")
                                        AddKnowledge(newKnowledge, fifthKnowledge);

                                    double e1 = 1;
                                    double e2 = 1;
                                    double newEnergy = 0;

                                    if (knowledge >= 0)
                                        newEnergy = e1 + 1 * e2 * knowledge;
                                    else
                                        newEnergy = e1 + 1 * e2 * (-1 * knowledge);

                                    string oldConcept = GetBestConcept(e, conceptsAndExplanations);
                                    List<string> oldExplanations = new List<string>();
                                    oldExplanations.Add(e);

                                    if (iteration == "first")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, firstKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "second")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, secondKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "third")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, thirdKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "fourth")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, fourthKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "fifth")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, fifthKnowledge, concepts, conceptsAndExplanations);
                                }
                            }
                            else if (words[2 + i].Contains("/"))
                            {
                                string[] values = words[2 + i].Split(' ');
                                string[] numb = values[1].Trim( new Char[] { '(', ')' } ).Split('/');
                                knowledge = (Int32.Parse(numb[0]) / Int32.Parse(numb[1])) * (difficulty + Math.Sqrt(difficulty)); 

                                foreach (string e in explanationsToQuestion)
                                {
                                    ExplanationWithKnowledge newKnowledge = new ExplanationWithKnowledge();
                                    newKnowledge.ExplanationID = e;
                                    if (values[0] == "T")
                                        newKnowledge.Knowledge += knowledge;
                                    else
                                        newKnowledge.Knowledge -= knowledge;
                                    newKnowledge.UserID = words[1];

                                    if (iteration == "first")
                                        AddKnowledge(newKnowledge, firstKnowledge);
                                    else if (iteration == "second")
                                        AddKnowledge(newKnowledge, secondKnowledge);
                                    else if (iteration == "third")
                                        AddKnowledge(newKnowledge, thirdKnowledge);
                                    else if (iteration == "fourth")
                                        AddKnowledge(newKnowledge, fourthKnowledge);
                                    else if (iteration == "fifth")
                                        AddKnowledge(newKnowledge, fifthKnowledge);

                                    double e1 = 1;
                                    double e2 = 1;
                                    double newEnergy = 0;

                                    if (knowledge >= 0)
                                        newEnergy = e1 + 1 * e2 * knowledge;
                                    else
                                        newEnergy = e1 + 1 * e2 * (-1 * knowledge);

                                    string oldConcept = GetBestConcept(e, conceptsAndExplanations);
                                    List<string> oldExplanations = new List<string>();
                                    oldExplanations.Add(e);

                                    if (iteration == "first")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, firstKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "second")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, secondKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "third")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, thirdKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "fourth")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, fourthKnowledge, concepts, conceptsAndExplanations);
                                    else if (iteration == "fifth")
                                        SpreadKnowledge(oldExplanations, 1, oldConcept, words[1], knowledge, newEnergy, fifthKnowledge, concepts, conceptsAndExplanations);
                                }
                            }
                        }
                    }   
                    catch (Exception e)
                    {
                        Console.WriteLine(e.InnerException.Message);
                        Console.WriteLine(countLine.ToString());
                    }
                    countLine++;
                }

                file.Close();

                if (countIteration == 0)
                {
                    countIteration++;
                    iteration = "second";
                }
                else if (countIteration == 1)
                {
                    countIteration++;
                    iteration = "third";
                }
                else if (countIteration == 2)
                {
                    countIteration++;
                    iteration = "fourth";
                }
                else if (countIteration == 3)
                {
                    countIteration++;
                    iteration = "fifth";
                }
            }

            WriteList(firstKnowledge, "firstKnowledgeSpread2.csv");
            WriteList(secondKnowledge, "secondKnowledgeSpread2.csv");
            WriteList(thirdKnowledge, "thirdKnowledgeSpread2.csv");
            WriteList(fourthKnowledge, "fourthKnowledgeSpread2.csv");
            WriteList(fifthKnowledge, "fifthKnowledgeSpread2.csv");

            //todo otvor ohodnotenie otazok a vyrataj pribytok vedomosti pre studenta
            //todo urob zaznam pre studenta, nadobudnutie vedomosti, cas na zaklade mysi, cas na zaklade webky, scrolls, lo-id, ARI, LIX,....
            //urob si korelacie, alebo natrenuj metodu...
        }

        private static string GetBestConcept(string explanation, HashSet<ConceptWithExplanations> conceptsAndExplanations)
        {
            string concept = null;
            double maxWeight = 0;

            foreach (ConceptWithExplanations value in conceptsAndExplanations)
            {
                foreach (ExplanationWithWeight v in value.ExplanationsID)
                {
                    if (v.ExplanationID == explanation && v.Weight > maxWeight)
                    {
                        concept = value.ConceptID;
                        maxWeight = v.Weight;
                    }
                }
            }
            return concept;
        }
        private static void WriteList(HashSet<ExplanationWithKnowledge> hash, string name)
        {
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Testy-Lisp\\Vyhodnotenie\\" + name;
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);

            foreach (ExplanationWithKnowledge value in hash)
            {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(value.UserID);
                    sb.Append(";");
                    sb.Append(value.ExplanationID);
                    sb.Append(";");
                    sb.Append(value.Knowledge.ToString());

                    file.WriteLine(sb.ToString());
            }
            file.Close();
        }

        private static void SpreadKnowledge(List<string> oldExplanations, double e1_old, string oldConcept, string userID, double knowledge, double energy, HashSet<ExplanationWithKnowledge> hash, HashSet<ConceptToConceptsRelation> concepts, HashSet<ConceptWithExplanations> conceptsAndExplanations)
        {
            if (energy >= 0.5)
            {
                foreach (ConceptWithExplanations value in conceptsAndExplanations)
                {
                    if (value.ConceptID == oldConcept)
                    {
                        foreach (ExplanationWithWeight e in value.ExplanationsID)
                        {
                            bool isInOldExplanation = false;
                            foreach (string old in oldExplanations)
                            {
                                if (old == e.ExplanationID)
                                {
                                    isInOldExplanation = true;
                                    break;
                                }
                            }

                            if (!isInOldExplanation)
                            {
                                ExplanationWithKnowledge newKnowledge = new ExplanationWithKnowledge();
                                newKnowledge.ExplanationID = e.ExplanationID;
                                newKnowledge.Knowledge = knowledge * e.Weight;
                                newKnowledge.UserID = userID;
                                AddKnowledge(newKnowledge, hash);
                                oldExplanations.Add(e.ExplanationID);
                            }
                        }
                        break;
                    }
                }

                foreach (ConceptToConceptsRelation concept in concepts)
                {
                    if (concept.ConceptID == oldConcept)
                    {
                        foreach (Concept_TypeAndWeight ctw in concept.RelatedConcepts)
                        {
                            if (ctw.ConceptID != oldConcept)
                            {
                                double e1 = e1_old - 0.1;  // znizovat na zaklade vahy
                                double e2 = e1_old - 0.1;
                                double newEnergy = 0;

                                if (knowledge >= 0)
                                    newEnergy = e1 + Decay(ctw.Type) * e2 * knowledge;
                                else
                                    newEnergy = e1 + Decay(ctw.Type) * e2 * (-1 * knowledge);
                                SpreadKnowledge(oldExplanations, e1, ctw.ConceptID, userID, knowledge, newEnergy, hash, concepts, conceptsAndExplanations);
                            }
                        }
                        break;
                    }
                }    
            }
        }

        private static double ActivationFunction(double probability)
        {
             if (probability <= 0.3)
                return 0;
            else if (probability > 0.3 && probability < 0.4)
                return 10 * (probability - 0.3);
            else
                return 1;
        }

        private static double Decay(string type)
        {
            if (type == "ConceptInheritsFromConceptRelation")
                return 0.5;
            else if (type == "ConceptRelatedToConceptRelation")
                return 0.25;
            else
                return 0.3;
        }

        private static void AddKnowledge(ExplanationWithKnowledge newKnowledge, HashSet<ExplanationWithKnowledge> hash)
        {
            bool added = false;

            foreach (ExplanationWithKnowledge v in hash)
            {
                if (v.ExplanationID == newKnowledge.ExplanationID && v.UserID == newKnowledge.UserID)
                {
                    v.Knowledge += newKnowledge.Knowledge;
                    added = true;
                    break;
                }
            }

            if (!added)
                hash.Add(newKnowledge);
        }

        public static void GetEyeTrackerData()
        {
            HashSet<LOeyeTracker> lo_visits = new HashSet<LOeyeTracker>();
            string path = "C:\\Users\\Veronika\\Desktop\\KnowledgeEstimation_Data_Export.csv";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;
            string loID = null;
            bool saccade = false;
            bool fixation = false;
            int fixationCount = 0;
            int saccadeCount = 0;
            bool firstTimeForLO = true;
            TimeSpan firstTime = new TimeSpan();
            TimeSpan actualTime = new TimeSpan();

            try 
            {
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');

                if (firstTimeForLO)
                {
                    firstTimeForLO = false;
                    loID = words[15];
                    
                    if (words[24] == "Fixation")
                    {
                        fixation = true;
                        fixationCount++;
                    }
                    else if (words[24] == "Saccade")
                    {
                        saccade = true;
                        saccadeCount++;
                    }

                    firstTime = TimeSpan.Parse(words[20]);
                    actualTime = TimeSpan.Parse(words[20]);
                    continue;
                }

                TimeSpan tmp = TimeSpan.Parse(words[20]);

                if (tmp == actualTime && words[15] == loID && (words[24] == "Saccade" && saccade) || (words[24] == "Fixation" && fixation))
                    continue;
                else if (tmp == actualTime && words[15] == loID && (words[24] == "Saccade" && fixation) || (words[24] == "Fixation" && saccade))
                {
                    if (saccade)
                    {
                        saccade = false;
                        fixation = true;
                        fixationCount++;
                    }
                    else
                    {
                        fixation = false;
                        saccade = true;
                        saccadeCount++;
                    }
                    continue;
                }
                else 
                {
                    if (tmp.TotalSeconds - actualTime.TotalSeconds <= 2 && words[15] == loID)
                    {
                        actualTime = tmp;
                        continue;
                    }
                    else
                    {
                        if (actualTime.TotalSeconds - firstTime.TotalSeconds > 4 && actualTime.TotalSeconds - firstTime.TotalSeconds < 480)
                        {
                            LOeyeTracker lo = new LOeyeTracker();
                            lo.ID = loID;
                            lo.SaccadeCount = saccadeCount;
                            lo.FixationCount = fixationCount;
                            lo.ActiveTime = (Double)actualTime.TotalSeconds - (Double)firstTime.TotalSeconds;

                            bool added = false;

                            foreach (LOeyeTracker value in lo_visits)
                            {
                                if (value.ID == lo.ID)
                                {
                                    value.ActiveTime += lo.ActiveTime;
                                    added = true;
                                    break;
                                }
                            }

                            if (!added)
                            {
                                lo_visits.Add(lo);
                            }

                            loID = words[15];
                            firstTime = tmp;
                            actualTime = tmp;

                            if (words[24] == "Saccade")
                            {
                                saccade = true;
                                fixation = false;
                                saccadeCount = 1;
                            }
                            else if (words[24] == "Fixation")
                            {
                                fixation = true;
                                saccade = false;
                                fixationCount = 1;
                            }                                
                        }
                    }
                }
            }

            if (actualTime.TotalSeconds - firstTime.TotalSeconds > 4 && actualTime.TotalSeconds - firstTime.TotalSeconds < 480)
            {
                LOeyeTracker lo = new LOeyeTracker();
                lo.ID = loID;
                lo.SaccadeCount = saccadeCount;
                lo.FixationCount = fixationCount;
                lo.ActiveTime = (Double)actualTime.TotalSeconds - (Double)firstTime.TotalSeconds;

                bool added = false;

                foreach (LOeyeTracker value in lo_visits)
                {
                    if (value.ID == lo.ID)
                    {
                        value.ActiveTime += lo.ActiveTime;
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    lo_visits.Add(lo);
                }
            }

            file.Close();

            path = "C:\\Users\\Veronika\\Desktop\\EyeTrackerData.csv";
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(path);

            foreach (LOeyeTracker value in lo_visits)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.ID);
                sb.Append(";");
                sb.Append(value.ActiveTime.ToString());
                sb.Append(";");
                sb.Append(value.FixationCount.ToString());
                sb.Append(";");
                sb.Append(value.SaccadeCount.ToString());

                file2.WriteLine(sb.ToString());
            }

            file2.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        public static void ConnectData()
        {
            HashSet<LOvisit> lo_visits = new HashSet<LOvisit>();
            string path = "C:\\Users\\Veronika\\Desktop\\LO\\results-ari-lix.csv";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                LOvisit lo = new LOvisit();
                lo.Id_lo = words[0];
                lo.Ari = Double.Parse(words[1].Replace('.',','));
                lo.Lix = Double.Parse(words[2].Replace('.', ','));
                lo_visits.Add(lo);
            }

            file.Close();
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\statistics_loTimes_avg.csv";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');
                
                foreach (LOvisit value in lo_visits)
                {
                    if (value.Id_lo == words[0])
                    {
                        value.WholeTime = Double.Parse(words[1]);
                        value.ActiveTime = Double.Parse(words[2]);
                        value.EyeTime = Double.Parse(words[3]);
                        break;
                    }
                }
            }

            file.Close();
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\mouse_activeTime_avg.csv";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');

                foreach (LOvisit value in lo_visits)
                {
                    if (value.Id_lo == words[0])
                    {
                        value.MouseActiveTime = Double.Parse(words[1].Replace('.', ','));
                        break;
                    }
                }
            }

            file.Close();
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\scrolls_count_avg.csv";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');

                foreach (LOvisit value in lo_visits)
                {
                    if (value.Id_lo == words[0])
                    {
                        value.NumbOfScrolls = Int32.Parse(words[1]);
                        break;
                    }
                }
            }

            file.Close();
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\clicks_count_avg.csv";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');

                foreach (LOvisit value in lo_visits)
                {
                    if (value.Id_lo == words[0])
                    {
                        value.NumbOfClicks = Int32.Parse(words[1]);
                        break;
                    }
                }
            }

            file.Close();
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\final_data.csv";
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(path);

            foreach (LOvisit value in lo_visits)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.Id_lo);
                sb.Append(";");
                sb.Append(value.Ari.ToString());
                sb.Append(";");
                sb.Append(value.Lix.ToString());
                sb.Append(";");
                sb.Append(value.WholeTime.ToString());
                sb.Append(";");
                sb.Append(value.ActiveTime.ToString());
                sb.Append(";");
                sb.Append(value.EyeTime.ToString());
                sb.Append(";");
                sb.Append(value.NumbOfScrolls.ToString());
                sb.Append(";");
                sb.Append(value.NumbOfClicks.ToString());
                sb.Append(";");
                sb.Append(value.MouseActiveTime.ToString());

                file2.WriteLine(sb.ToString());
            }

            file2.Close();
        }

        public static void GetScrollsCount()
        {
            HashSet<LOvisit> lo_visits = new HashSet<LOvisit>();
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\scrolls_alefdata.csv";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\scrolls_count_per_user.csv";
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(path);
            string line;
            int counter = 0;
            bool scrollDown = false;
            bool scrollUp = false;
            string loID = null;
            string userID = null;
            bool firstTime = true;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(';');

                    if (firstTime)
                    {
                        loID = GetLoID(words[7]);
                        userID = words[0];
                        if (words[1] == "s-down")
                            scrollDown = true;
                        else
                            scrollUp = true;

                        firstTime = false;
                        counter++;
                        continue;
                    }

                    string tmp_loID = GetLoID(words[7]);
                    string tmp_userID = words[0];
                    string tmp_event = words[1];

                    if ((tmp_event == "s-up" && scrollUp || tmp_event == "s-down" && scrollDown) && tmp_userID == userID && tmp_loID == loID)
                    {
                        counter++;
                        continue;
                    }
                    else
                    {
                        LOvisit lv = new LOvisit();
                        lv.Id_user = userID;
                        lv.Id_lo = loID;
                        lv.NumbOfScrolls = 1;

                        bool added = false;

                        foreach (LOvisit value in lo_visits)
                        {
                            if (value.Id_lo == lv.Id_lo && value.Id_user == lv.Id_user)
                            {
                                value.NumbOfScrolls += lv.NumbOfScrolls;
                                added = true;
                                break;
                            }
                        }

                        if (!added)
                        {
                            lo_visits.Add(lv);
                        }

                        if (tmp_event == "s-up")
                        {
                            scrollUp = true;
                            scrollDown = false;
                        }
                        else
                        {
                            scrollDown = true;
                            scrollUp = false;
                        }

                        loID = tmp_loID;
                        userID = tmp_userID;
                        counter++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(counter.ToString());
                }
            }

            HashSet<LOvisit> data = new HashSet<LOvisit>();

            foreach (LOvisit value in lo_visits)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.Id_user);
                sb.Append(",");
                sb.Append(value.Id_lo);
                sb.Append(",");
                sb.Append(value.NumbOfScrolls.ToString());

                file2.WriteLine(sb.ToString());

                bool added = false;

                foreach (LOvisit v in data)
                {
                    if (v.Id_lo == value.Id_lo)
                    {
                        v.NumbOfScrolls += value.NumbOfScrolls;
                        v.Counter += 1;
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    value.Counter = 1;
                    data.Add(value);
                }
            }

            System.IO.StreamWriter file3 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\scrolls_count_avg.csv");

            foreach (LOvisit value in data)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(value.Id_lo);
                sb2.Append(";");
                sb2.Append((value.NumbOfScrolls / value.Counter).ToString());
                file3.WriteLine(sb2.ToString());
            }

            file.Close();
            file2.Close();
            file3.Close();
        }

        public static void GetClicksCount()
        {
            HashSet<LOvisit> lo_visits = new HashSet<LOvisit>();
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\clicks_alefdata.csv";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\clicks_count_per_user.csv";
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(path);
            string line;
            int counter = 0;
            bool mouseDown = false;
            bool mouseUp = false;
            string loID = null;
            string userID = null;
            bool firstTime = true;
            int numbOfClicks = 0;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(';');

                    if (firstTime)
                    {
                        loID = GetLoID(words[7]);
                        userID = words[0];
                        if (words[1] == "m-down")
                            mouseDown = true;
                        else
                            mouseUp = true;
                        
                        firstTime = false;
                        counter++;
                        continue;
                    }

                    string tmp_loID = GetLoID(words[7]);
                    string tmp_userID = words[0];
                    string tmp_event = words[1];
                    
                    if (tmp_event == "m-up" && mouseDown && tmp_userID == userID && tmp_loID == loID)
                    {
                        mouseDown = false;
                        mouseUp = true;
                        numbOfClicks++;
                        counter++;
                        continue;
                    }
                    else if (tmp_event == "m-down" && mouseUp && tmp_userID == userID && tmp_loID == loID)
                    {
                        mouseDown = true;
                        mouseUp = false;
                        numbOfClicks++;
                        counter++;
                        continue;
                    }
                    else if (tmp_userID != userID || tmp_loID != loID)
                    {
                        LOvisit lv = new LOvisit();
                        lv.Id_user = userID;
                        lv.Id_lo = loID;
                        lv.NumbOfClicks = numbOfClicks;

                        bool added = false;

                        foreach (LOvisit value in lo_visits)
                        {
                            if (value.Id_lo == lv.Id_lo && value.Id_user == lv.Id_user)
                            {
                                value.NumbOfClicks += lv.NumbOfClicks;
                                added = true;
                                break;
                            }
                        }

                        if (!added)
                        {
                            lo_visits.Add(lv);
                        }

                        numbOfClicks = 0;
                        loID = tmp_loID;
                        userID = tmp_userID;
                        counter++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(counter.ToString());
                }
            }

            HashSet<LOvisit> data = new HashSet<LOvisit>();

            foreach (LOvisit value in lo_visits)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.Id_user);
                sb.Append(",");
                sb.Append(value.Id_lo);
                sb.Append(",");
                sb.Append(value.NumbOfClicks.ToString());

                file2.WriteLine(sb.ToString());

                bool added = false;

                foreach (LOvisit v in data)
                {
                    if (v.Id_lo == value.Id_lo)
                    {
                        v.NumbOfClicks += value.NumbOfClicks;
                        v.Counter += 1;
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    value.Counter = 1;
                    data.Add(value);
                }
            }

            System.IO.StreamWriter file3 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\clicks_count_avg.csv");

            foreach (LOvisit value in data)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(value.Id_lo);
                sb2.Append(";");
                sb2.Append((value.NumbOfClicks / value.Counter).ToString());
                file3.WriteLine(sb2.ToString());
            }

            file.Close();
            file2.Close();
            file3.Close();
        }

        public static void GetEventData()
        {
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\data.csv";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\scrolls_alefdata.csv";
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(path);
            string line;
            int counter = 0;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(';');

                    if ((words[1] == "s-up" || words[1] == "s-down") && words[7].Contains("learning_objects"))
                        file2.WriteLine(line);

                    counter++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(counter.ToString());
                }
            }

            file.Close();
            file2.Close();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
        
        public static void GetActiveTimeForMouse()
        {
            int counter = 0;
            string line;
            HashSet<LOvisit> lo_visits = new HashSet<LOvisit>();

            System.IO.StreamReader file = new System.IO.StreamReader(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\mouse_alefdata.csv");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\mouse_activeTime.csv");
            string loID = null;
            string userID = null;
            TimeSpan firstTime = new TimeSpan();
            TimeSpan actualTime = new TimeSpan();
            bool firstTimeForLO = true;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(';');
                    if (firstTimeForLO)
                    {
                        firstTime = TimeSpan.Parse(words[3]);
                        firstTimeForLO = false;
                        actualTime = TimeSpan.Parse(words[3]);
                        loID = GetLoID(words[7]);
                        userID = words[0];
                        continue;
                    }

                    TimeSpan tmp = TimeSpan.Parse(words[3]);
                    string tmp_loID = GetLoID(words[7]);
                    string tmp_userID = words[0];

                    if (actualTime == tmp && tmp_loID == loID && tmp_userID == userID)
                    {
                        counter++;
                        continue;
                    }
                    else
                    {
                        if (tmp.TotalSeconds - actualTime.TotalSeconds <= 2 && tmp_loID == loID && tmp_userID == userID)
                        {
                            actualTime = tmp;
                            counter++;
                            continue;
                        }
                        else
                        {
                            if (actualTime.TotalSeconds - firstTime.TotalSeconds > 4 && actualTime.TotalSeconds - firstTime.TotalSeconds < 480)
                            {
                                LOvisit lv = new LOvisit();
                                lv.Id_user = userID;
                                lv.Id_lo = loID;
                                lv.ActiveTime = (Double)actualTime.TotalSeconds - (Double)firstTime.TotalSeconds;

                                bool added = false;

                                foreach (LOvisit value in lo_visits)
                                {
                                    if (value.Id_lo == lv.Id_lo && value.Id_user == lv.Id_user)
                                    {
                                        value.ActiveTime += lv.ActiveTime;
                                        added = true;
                                        break;
                                    }
                                }

                                if (!added)
                                {
                                    lo_visits.Add(lv);
                                }
                            }

                            firstTime = tmp;
                            actualTime = tmp;
                            loID = tmp_loID;
                            userID = tmp_userID;
                        }
                    }

                    counter++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(counter.ToString());
                }
            }

            HashSet<LOvisit> data = new HashSet<LOvisit>();

            foreach (LOvisit value in lo_visits)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.Id_user);
                sb.Append(",");
                sb.Append(value.Id_lo);
                sb.Append(",");
                sb.Append(value.ActiveTime.ToString());

                file2.WriteLine(sb.ToString());

                bool added = false;

                foreach (LOvisit v in data)
                {
                    if (v.Id_lo == value.Id_lo)
                    {
                        v.ActiveTime += value.ActiveTime;
                        v.Counter += 1;
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    value.Counter = 1;
                    data.Add(value);
                }
            }

            System.IO.StreamWriter file3 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Data-server\\mouse_activeTime_avg.csv");

            foreach (LOvisit value in data)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(value.Id_lo);
                sb2.Append(";");
                sb2.Append((value.ActiveTime / value.Counter).ToString());
                file3.WriteLine(sb2.ToString());
            }

            file.Close();
            file2.Close();
            file3.Close();
        }

        public static string GetLoID(string url)
        {
            string[] words = url.Split('/');
            return words[2];
        }

        public static void CompareFilesWithLOid()
        {
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\alef\\uniqueLOids.txt";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;

            Hashtable ids = new Hashtable();
            int notUnique = 0;
            int counter = 0;

            while ((line = file.ReadLine()) != null)
            {
                if (!ids.Contains(line))
                {
                    counter++;
                    ids.Add(line, line);
                }
                else
                {
                    Console.WriteLine(line);
                    notUnique++;
                }
            }

            Console.WriteLine("lo_unique duplikatne: " + notUnique.ToString());
            Console.WriteLine("lo_unique pocet: " + counter.ToString());
            file.Close();

            path = "C:\\Users\\Veronika\\Desktop\\LO\\lo_ids.txt";
            file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (!ids.Contains(line))
                    Console.WriteLine("Chyba: " + line);
            }
            file.Close();
            Console.ReadKey();

        }
        public static void SelectUniqueLO()
        {
            string path = "D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\alef\\otázky_testyLisp_texty.txt";
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path);

            Hashtable ids = new Hashtable();
            int counterLines = 0;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    int count = int.Parse(words[6]);            

                    for (int i = 1; i <= count; i++)
                    {
                        if (!ids.Contains(words[6+i]))
                            ids.Add(words[6 + i],words[6 + i]);
                    }

                    counterLines++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(counterLines.ToString());
                }
            }

            file.Close();
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\alef\\uniqueLOids.txt");

            foreach (DictionaryEntry value in ids)
            {
                file2.WriteLine(value.Value);
            }

            file2.Close();
        }

        public static void CountARIandLIX()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\\Users\\Veronika\\Desktop\\LO\\lo_ids.txt");
            List<string> lo_ids = new List<string>();
            int counter = 0;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    lo_ids.Add(words[0]);
                    counter++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(counter.ToString());
                }
            }

            file.Close();

            List<LearningObject> objects = new List<LearningObject>();

            foreach (string id in lo_ids)
            {
                LearningObject lo = new LearningObject();
                lo.ID = id;
                int words = 0;
                int chars = 0;
                int longWords = 0;
                int sentences = 0;

                try
                {
                    // pootvarat subory a vyratat ARI, LIX, ulozit :)
                    string path = "C:\\Users\\Veronika\\Desktop\\LO\\" + id + ".txt";
                    file = new System.IO.StreamReader(path);

                    while ((line = file.ReadLine()) != null && line != "")
                    {
                        string[] wordsInLine = line.Split(' ');

                        foreach (string value in wordsInLine)
                        {
                            chars += value.Length;
                            if (value.Length >= 6)
                                longWords++;
                            if (value.Contains(".") || value.Contains("?") || value.Contains("!"))
                                sentences++;
                            words++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(id.ToString());
                }

                lo.Ari = 4.7 * ((double)chars / (double)words) + 0.5 * ((double)words / (double)sentences) - 21.43;
                lo.Lix = ((double)words / (double)sentences) + (100 * ((double)longWords / (double)words)) - 21.43;
                objects.Add(lo);
                file.Close();
            }

            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"C:\\Users\\Veronika\\Desktop\\LO\\results-new.csv");
            foreach (LearningObject value in objects)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.ID);
                sb.Append(",");
                sb.Append(value.Ari.ToString().Replace(',', '.'));
                sb.Append(",");
                sb.Append(value.Lix.ToString().Replace(',','.'));

                file2.WriteLine(sb.ToString());
            }
            file2.Close();
        }

        public static void QuestionsAndLOconnector()
        {
            string line;
            int counter = 0;

            HashSet<LOandQuestion> questionsAndExplanations = new HashSet<LOandQuestion>();
            HashSet<LOandConcept> explanationsAndConcepts = new HashSet<LOandConcept>();
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\\Users\\Veronika\\Desktop\\explanation_concepts.txt");
            
            while ((line = file.ReadLine()) != null && line != "")
            {
                try
                {
                    string[] words = line.Split(',');
                    
                    LOandConcept lc = new LOandConcept(words[3], words[0]);
                    bool added = false;

                    foreach (LOandConcept value in explanationsAndConcepts)
                    {
                        if (value.Lo_ID == lc.Lo_ID && value.ConceptID == lc.ConceptID)
                        {
                            added = true;
                            counter++;
                            break;
                        }
                    }

                    if (!added)
                    {
                        counter++;
                        explanationsAndConcepts.Add(lc);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
    

            file.Close();

            file = new System.IO.StreamReader(@"C:\\Users\\Veronika\\Desktop\\questionWithConcepts.csv");
           
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] words = line.Split(',');
                    LOandQuestion laq = new LOandQuestion(words[2], words[0], words[1], Double.Parse(words[3], CultureInfo.InvariantCulture), Double.Parse(words[5], CultureInfo.InvariantCulture), words[4]);
                    bool added = false;
                    counter = 0;

                    foreach (LOandQuestion value in questionsAndExplanations)
                    {
                        if (value.ConceptID == laq.ConceptID && value.QuestionID == laq.QuestionID)
                        {
                            added = true;
                            counter++;
                            break;
                        }
                    }

                    if (!added)
                    {
                        questionsAndExplanations.Add(laq);
                        counter++;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }

            file.Close();

            // Spájanie otázok a explanation

            HashSet<LOandQuestion> result = new HashSet<LOandQuestion>();

            foreach (LOandQuestion value in questionsAndExplanations)
            {
                try
                {
                    List<string> explID = FindExplanations(value, explanationsAndConcepts);
                   
                    foreach (string v in explID)
                    {
                        LOandQuestion newLOaQ = value;
                        newLOaQ.ExplanationID = v;
                        // TODO pridat vahu explanation a label explanation
                        result.Add(newLOaQ);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }

            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"C:\\Users\\Veronika\\Desktop\\QuestionsAndExplanations.csv");

            foreach (LOandQuestion value in result)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.ConceptID);
                sb.Append(",");
                sb.Append(value.ConceptName);
                sb.Append(",");
                sb.Append(value.QuestionID);
                sb.Append(",");
                sb.Append(value.QuestionLabel);
                sb.Append(",");
                sb.Append(value.QuestionDifficulty.ToString());
                sb.Append(",");
                sb.Append(value.ExplanationID);

                file2.WriteLine(sb.ToString());
            }
        }

        public static List<string> FindExplanations(LOandQuestion value, HashSet<LOandConcept> explanationsAndConcepts)
        {
            List<string> result = new List<string>();

            foreach (LOandConcept v in explanationsAndConcepts)
            {
                // Možnosť pridať váhy
                if (value.ConceptID.Equals(v.ConceptID))
                    result.Add(v.Lo_ID);
            }

            return result;
        }

        public void ConvertSeparator()
        {
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\\Users\\Veronika\\Desktop\\Data.csv");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"C:\\Users\\Veronika\\Desktop\\Data-repaired2.csv");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');
                string tmp = words[7];
                if (tmp.Contains("learning_objects"))
                {
                    line = line.Replace(';', ',');
                    file2.WriteLine(line);
                }
            }

            file.Close();
            file2.Close();
        }

        public static void CountTime()
        {
            int counter = 0;
            string line;
            HashSet<LOvisit> lo_visits = new HashSet<LOvisit>();

            System.IO.StreamReader file = new System.IO.StreamReader(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\statistics.csv");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\statistics_new.csv");
            
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');
                double tmp = Double.Parse(words[3]);

                if (tmp > 4 && tmp < 480)
                {
                    try
                    {
                        LOvisit lv = new LOvisit();
                        lv.Id_user = words[0];
                        lv.Id_lo = words[1];
                        lv.WholeTime = Double.Parse(words[2]);
                        lv.ActiveTime = Double.Parse(words[3]);
                        lv.EyeTime = Double.Parse(words[4]);

                        bool added = false;

                        foreach (LOvisit value in lo_visits)
                        {
                            if (value.Id_lo == lv.Id_lo && value.Id_user == lv.Id_user)
                            {
                                value.WholeTime += lv.WholeTime;
                                value.ActiveTime += lv.ActiveTime;
                                value.EyeTime += lv.EyeTime;
                                added = true;
                                counter++;
                                break;
                            }
                        }

                        if (!added)
                        {
                            lo_visits.Add(lv);
                            counter++;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(counter.ToString() + "*****" + e.InnerException);
                    }
                }
            }

            HashSet<LOvisit> data = new HashSet<LOvisit>();

            foreach (LOvisit value in lo_visits)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(value.Id_user);
                sb.Append(",");
                sb.Append(value.Id_lo);
                sb.Append(",");
                sb.Append(value.WholeTime.ToString());
                sb.Append(",");
                sb.Append(value.ActiveTime.ToString());
                sb.Append(",");
                sb.Append(value.EyeTime.ToString());

                file2.WriteLine(sb.ToString());

                bool added = false;

                foreach (LOvisit v in data)
                {
                    if (v.Id_lo == value.Id_lo)
                    {
                        v.WholeTime += value.WholeTime;
                        v.ActiveTime += value.ActiveTime;
                        v.EyeTime += value.EyeTime;
                        v.Counter += 1;
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    value.Counter = 1;
                    data.Add(value);
                }

                
            }

            System.IO.StreamWriter file3 = new System.IO.StreamWriter(@"D:\\Dropbox\\Documents\\skola\\Diplomovka\\Experiment Lisp\\Statistiky\\statistics_loTimes_avg.csv");

            foreach (LOvisit value in data)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(value.Id_lo);
                sb2.Append(";");
                sb2.Append((value.WholeTime/value.Counter).ToString());
                sb2.Append(";");
                sb2.Append((value.ActiveTime / value.Counter).ToString());
                sb2.Append(";");
                sb2.Append((value.EyeTime / value.Counter).ToString());
                file3.WriteLine(sb2.ToString());
            }

            file.Close();
            file2.Close();
            file3.Close();
        }

        public void ConvertDate()
        {
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\\Users\\Veronika\\Desktop\\Data.csv");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"C:\\Users\\Veronika\\Desktop\\Data-repaired2.csv");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');
                string tmp = words[7];
                
                DateTime d = _jan1st1970;
                double t = double.Parse(tmp);
                d = d.AddMilliseconds(t);

                string date = d.ToString("dd.MM.yyyy");
                string time = d.ToString("h:mm:ss");

                StringBuilder sb = new StringBuilder();
                sb.Append(words[0]);
                sb.Append(";");
                sb.Append(words[1]);
                sb.Append(";");
                sb.Append(date);
                sb.Append(";");
                sb.Append(time);
                sb.Append(";");

                for (int i = 3; i < words.Length - 1; i++)
                {
                    sb.Append(words[i]);
                    sb.Append(";");
                }
                file2.WriteLine(sb.ToString());
            }

            file.Close();
            file2.Close();
        }

        public long Convert(DateTime from)
        {
            return System.Convert.ToInt64((from - _jan1st1970).TotalMilliseconds);
        }

        public DateTime Convert(long from)
        {
            return _jan1st1970.AddMilliseconds(from);
        }
    }
}
