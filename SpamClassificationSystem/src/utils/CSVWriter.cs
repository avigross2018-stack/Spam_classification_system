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
            //Creates a new CSVWriter object and receives a path and separator.
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Output path cannot be empty.",nameof(path));
            }
            _path = path; //Saves the path of the output file.
            _delimiter = delimiter; // Stores the character that separates the values ​​in the file, usually a comma
            _headerWasWritten = false; //Checks whether the header line has already been written to the file.
        }

        // Writes one input row and its prediction to the CSV file.
        public void WritePrediction(Dictionary<string, string> row,List<string> headers, string prediction)
        {
            //Checks if the header has not yet been written to the file.
            if (!_headerWasWritten)
            {
                //Creates the header line using the CreateHeaderLine function.
                string headerLine = CreateHeaderLine(headers);

                //Creates or overwrites the file and writes the header line to it.
                File.WriteAllText(_path,headerLine + Environment.NewLine);

                //Indicates that the title has already been written so that it will not be written again.
                _headerWasWritten = true;
            }
            //Creates a CSV row containing the input values ​​and the forecast
            string resultLine = CreateResultLine(headers,row,prediction);

            //Appends the result line to the end of the file.
            File.AppendAllText(_path,resultLine + Environment.NewLine);
        }

        // Creates the CSV header line and adds the Prediction column.
        private string CreateHeaderLine(List<string> headers)
        {
            //Creates a copy of the title list so as not to change the original list.
            List<string> outputHeaders = new(headers);

            //Adds a new column called Prediction.
            outputHeaders.Add("Prediction");

            //Combines all titles into one string with the separator between them.
            return string.Join(_delimiter,outputHeaders);
        }

        // Creates one CSV row containing the input values and prediction.
        private string CreateResultLine(List<string> headers,Dictionary<string, string> row,string prediction)
        {
            //Goes through the titles in order and takes the corresponding value for each title from the row.
            List<string> values = headers
                .Select(header => row[header])
                .ToList();

            //Adds the forecast to the end of the list.
            values.Add(prediction);

            //Combines all values ​​into a single string in CSV format
            return string.Join(_delimiter,values);
        }
    }
}