using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using SpamClassificationSystem.src.utils;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // Interactive mode:
        // Reads values from the user and predicts one sample.
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

            // Removes the last column because it is the target column.
            List<string> featureHeaders = trainingData
                .GetLabels()
                .Take(trainingData.GetLabels().Count - 1)
                .ToList();

            Dictionary<string, string> sample =
                UserEnterData(featureHeaders);

            string prediction =
                predictor.Predict(sample);

            writer.WritePrediction(
                sample,
                featureHeaders,
                prediction);
        }

        // Batch mode:
        // Reads samples from an input CSV file,
        // predicts each row and sends the result to both writers.
        public void RunBatch(
    string trainPath,
    string inputPath,
    IWriter consoleWriter,
    IWriter csvWriter)
        {
            Console.WriteLine("Reading training data");

            DataSet trainingData = Reader.Read(trainPath);

            NavieBaseModel model = Trainer.Train(trainingData);

            NaiveBasePredictor predictor = new(model);

            // All training columns except the last target column.
            List<string> featureHeaders = trainingData
                .GetLabels()
                .Take(trainingData.GetLabels().Count - 1)
                .ToList();

            Console.WriteLine("Reading input data");

            DataSet inputData = Reader.Read(inputPath);

            foreach (Dictionary<string, string> inputRow in inputData.GetRows())
            {
                // Creates a sample containing only feature columns.
                Dictionary<string, string> sample = featureHeaders
                    .ToDictionary(
                        header => header,
                        header => inputRow[header]);

                string prediction = predictor.Predict(sample);

                consoleWriter.WritePrediction(
                    sample,
                    featureHeaders,
                    prediction);

                csvWriter.WritePrediction(
                    sample,
                    featureHeaders,
                    prediction);
            }
        }

        // Reads one sample from the user.
        public Dictionary<string, string> UserEnterData(
            List<string> featureHeaders)
        {
            Dictionary<string, string> sample = new();

            foreach (string header in featureHeaders)
            {
                Console.Write($"Enter {header}: ");

                string? userInput =
                    Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    throw new ArgumentException(
                        $"{header} cannot be empty.");
                }

                userInput = userInput.Trim();

                string formattedInput =
                    char.ToUpper(userInput[0]) +
                    userInput[1..].ToLower();

                sample.Add(header, formattedInput);
            }

            return sample;
        }
    }
}