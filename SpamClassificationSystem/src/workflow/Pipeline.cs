using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using Dumpify;

namespace SpamClassificationSystem.src.workflow
{
    public class Pipeline
    {
        public IReader Reader{ get; }
        public ITrainer Trainer{ get; }
        public IWriter Writer{ get; }


        public Pipeline(IReader reader, ITrainer trainer)
        {
            Reader = reader;
            Trainer = trainer;
            
        }

        public void RunBatch(string trainPath)
        {
            System.Console.WriteLine("Reading");
            DataSet data = Reader.Read(trainPath);

            NavieBaseModel model = Trainer.Train(data);

            NaiveBasePredictor predictor = new(model);
            
            Dictionary<string, string> sample = UserEnterData(data);

            string result = predictor.Predict(sample);

            System.Console.WriteLine(result);
            
        }

        public Dictionary<string, string> UserEnterData(DataSet dataSet)
        {
            Dictionary<string, string> sample = new();
            List<string> tags = dataSet.GetLabels();    //hold all column names
            string lastRow = tags[^1];      //hold last column name

            foreach(string tag in tags)
            {
                if(tag == lastRow)
                    continue;
                
                System.Console.WriteLine($"Enter {tag}:");
                string userInput = Console.ReadLine();
                sample.Add(tag, char.ToUpper(userInput[0]) + userInput[1..]);
            }

            return sample;
        }
    }
}