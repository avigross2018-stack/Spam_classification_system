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
        public void WritePrediction(Dictionary<string, string> row,List<string> headers,string prediction)
        {
            foreach (string header in headers)
            {
                Console.WriteLine($"{header}: {row[header]}");
            }

            Console.WriteLine($"Prediction: {prediction}");
        }
    }
}