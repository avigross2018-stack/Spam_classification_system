using SpamClassificationSystem.src.interfaces;
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
        private bool _headerWasWritten;

        // Receives the output file path and the CSV delimiter.
        public CSVWriter(string path, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Output path cannot be empty.",nameof(path));
            }
            _path = path;
            _delimiter = delimiter;
            _headerWasWritten = false;
        }

        // Writes one input row and its prediction to the CSV file.
        public void WritePrediction(Dictionary<string, string> row,List<string> headers, string prediction)
        {
            if (!_headerWasWritten)
            {
                string headerLine = CreateHeaderLine(headers);

                File.WriteAllText(_path,headerLine + Environment.NewLine);

                _headerWasWritten = true;
            }

            string resultLine = CreateResultLine(headers,row,prediction);

            File.AppendAllText(_path,resultLine + Environment.NewLine);
        }

        // Creates the CSV header line and adds the Prediction column.
        private string CreateHeaderLine(List<string> headers)
        {
            List<string> outputHeaders = new(headers);

            outputHeaders.Add("Prediction");

            return string.Join(_delimiter,outputHeaders);
        }

        // Creates one CSV row containing the input values and prediction.
        private string CreateResultLine(List<string> headers,Dictionary<string, string> row,string prediction)
        {
            List<string> values = headers
                .Select(header => row[header])
                .ToList();

            values.Add(prediction);

            return string.Join(_delimiter,values);
        }
    }
}