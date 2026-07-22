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
        public IReader Reader { get; }
        public ITrainer Trainer { get; }

        public Pipeline(IReader reader, ITrainer trainer)
        {
            Reader = reader;
            Trainer = trainer;
        }

        public void RunInteractive(
            string trainPath,
            IWriter writer)
        {
            Console.WriteLine("Reading training data");

            DataSet trainingData = Reader.Read(trainPath);

            NavieBaseModel model =
                Trainer.Train(trainingData);

            NaiveBasePredictor predictor =
                new(model);

            List<string> featureHeaders =
                GetFeatureHeaders(trainingData);

            Dictionary<string, string> sample =
                UserEnterData(featureHeaders);

            string prediction =
                predictor.Predict(sample);

            writer.WritePrediction(
                sample,
                featureHeaders,
                prediction);
        }

        public void RunBatch(
            string trainPath,
            string inputPath,
            IWriter consoleWriter,
            IWriter csvWriter)
        {
            Console.WriteLine("Reading training data");

            DataSet trainingData =
                Reader.Read(trainPath);

            NavieBaseModel model =
                Trainer.Train(trainingData);

            NaiveBasePredictor predictor =
                new(model);

            Console.WriteLine("Reading input data");

            DataSet inputData =
                Reader.Read(inputPath);

            List<string> inputHeaders =
                inputData.GetLabels();

            foreach (Dictionary<string, string> sample
                     in inputData.GetRows())
            {
                string prediction =
                    predictor.Predict(sample);

                consoleWriter.WritePrediction(
                    sample,
                    inputHeaders,
                    prediction);

                csvWriter.WritePrediction(
                    sample,
                    inputHeaders,
                    prediction);
            }
        }

        private List<string> GetFeatureHeaders(
            DataSet trainingData)
        {
            return trainingData
                .GetLabels()
                .Take(trainingData.GetLabels().Count - 1)
                .ToList();
        }

        public Dictionary<string, string> UserEnterData(
            List<string> featureHeaders)
        {
            Dictionary<string, string> sample = new();

            foreach (string header in featureHeaders)
            {
                Console.Write($"Enter {header}: ");

                string? userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    throw new ArgumentException(
                        $"{header} cannot be empty.");
                }

                sample[header] = FormatInput(userInput);
            }

            return sample;
        }

        private string FormatInput(string input)
        {
            input = input.Trim();

            return char.ToUpper(input[0]) +
                   input[1..].ToLower();
        }
    }
}