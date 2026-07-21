using DroneFleetDataProcessing.src;
using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using SpamClassificationSystem.src.utils;
using SpamClassificationSystem.src.workflow;
using System;
using System.Security.Cryptography.X509Certificates;

namespace SpamClassificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            IReader reader = new CSVReader();
            ITrainer trainer = new NaiveBaseTrain();

            PathManager path = new PathManager();
            string filePath = path.getInputPath("Play_Tennis_Train.csv");


            IWriter butchWriter = new ConsoleWriter();
            Pipeline pipeline = new Pipeline(reader, trainer);

            pipeline.RunBatch(filePath, butchWriter);
        }
    }
}