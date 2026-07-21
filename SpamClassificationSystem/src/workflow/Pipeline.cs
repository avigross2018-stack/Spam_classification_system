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
            DataSet dataSet = Reader.Read(trainPath);
            NavieBaseModel navieBaseModel = Trainer.Train();
            
        }
    }
}