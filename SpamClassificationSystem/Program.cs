using DroneFleetDataProcessing.src;
using SpamClassificationSystem.src.models;
using SpamClassificationSystem.src.utils;
using SpamClassificationSystem.src.workflow;
using System;

namespace SpamClassificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            CSVReader reade = new CSVReader();
            PathManager path = new PathManager();
            //test 
            DataSet test = reade.Read(path.getInputPath("weather_play.csv"));
            for (int i = 0; i < test.GetLabels().Count; i++)
            {
                Console.WriteLine(test.GetLabels()[i]);
            }

            foreach (var d in test.GetRows())
            {
                Console.WriteLine();
                foreach (var (k, v) in d)
                {
                    Console.WriteLine($"key: {k}, val: {v}");
                }
            }
            foreach (var item in test.GetTarget())
            {
                Console.WriteLine();
                Console.WriteLine(item);
            }
        }
    }
}