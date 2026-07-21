using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpamClassificationSystem.src.utils
{
    public class ConsoleWriter : IWriter
    {
        private readonly char _delimiter;

        // Receives the delimiter used between printed values.
        public ConsoleWriter(char delimiter = ',')
        {
            _delimiter = delimiter;
        }

        // Prints all input rows and their predictions to the console.
        public void WritePredictions(DataSet inputData,List<string> predictions)
        {
            List<Dictionary<string, string>> rows = inputData.GetRows();

            List<string> headers = inputData.GetLabels();

            if (rows.Count != predictions.Count)
            {
                throw new ArgumentException("The number of predictions must match the number of rows.");
            }

            for (int i = 0; i < rows.Count; i++)
            {
                string values = string.Join(_delimiter,headers.Select(header => rows[i][header]));

                Console.WriteLine($"Row {i + 1}: {values} -> {predictions[i]}");
            }
        }
    }
}