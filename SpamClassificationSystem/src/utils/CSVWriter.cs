using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpamClassificationSystem.src.utils
{
    public class CSVWriter : IWriter
    {
        private readonly string _path;
        private readonly char _delimiter;

        // Receives the output file path and the CSV delimiter.
        public CSVWriter(
            string path,
            char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    "Output path cannot be empty.",
                    nameof(path));
            }

            _path = path;
            _delimiter = delimiter;
        }

        // Writes all input rows and their predictions to a CSV file.
        public void WritePredictions(
            DataSet inputData,
            List<string> predictions)
        {
            List<Dictionary<string, string>> rows =
                inputData.GetRows();

            List<string> headers =
                inputData.GetLabels();

            if (rows.Count != predictions.Count)
            {
                throw new ArgumentException(
                    "The number of predictions must match the number of rows.");
            }

            List<string> outputLines = new();

            outputLines.Add(CreateHeaderLine(headers));

            for (int i = 0; i < rows.Count; i++)
            {
                string line = CreateResultLine(
                    headers,
                    rows[i],
                    predictions[i]);

                outputLines.Add(line);
            }

            File.WriteAllLines(_path, outputLines);
        }

        // Creates the CSV header line.
        private string CreateHeaderLine(
            List<string> headers)
        {
            List<string> outputHeaders = new(headers);

            outputHeaders.Add("Prediction");

            return string.Join(
                _delimiter,
                outputHeaders);
        }

        // Creates one CSV row with the prediction.
        private string CreateResultLine(
            List<string> headers,
            Dictionary<string, string> row,
            string prediction)
        {
            List<string> values = headers
                .Select(header => row[header])
                .ToList();

            values.Add(prediction);

            return string.Join(
                _delimiter,
                values);
        }
    }
}