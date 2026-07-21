using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpamClassificationSystem.src.utils
{
    public class CSVReader : IReader
    {
        private readonly char _delimiter;

        public CSVReader(char delimiter = ',')
        {
            _delimiter = delimiter;
        }

        public DataSet Read(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("CSV path cannot be empty.", nameof(path));
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("CSV file was not found.", path);
            }
            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2)
            {
                throw new InvalidDataException("this file must contain a header and at least one data row.");
            }

            List<string> headers = ParseHeaders(lines[0]);
            List < Dictionary<string, string> > rows = new();
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                Dictionary<string, string> row = ParseRow(lines[i], headers, i + 1);

                rows.Add(row);
            }

            string targetColumn = headers[^1];

            List<string> targets = rows
                .Select(row => row[targetColumn])
                .Distinct()
                .ToList();

            return new DataSet(headers, rows, targets);

        }

        private List<string> ParseHeaders(string headerLine)
        {
            List<string> headers = headerLine
            .Split(_delimiter)
            .Select(header => header.Trim())
            .ToList();
            if (headers.Count < 2)
            {
                throw new InvalidDataException("CSV file must contain at least one feature column and one target column.");
            }

            if (headers.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidDataException("CSV contains an empty column name.");
            }

            if (headers.Distinct().Count() != headers.Count)
            {
                throw new InvalidDataException("CSV contains duplicate column names.");
            }

            return headers;
        }
        private Dictionary<string, string> ParseRow(
        string line,
        List<string> headers,
        int lineNumber)
        {
            string[] values = line
                .Split(_delimiter)
                .Select(value => value.Trim())
                .ToArray();

            if (values.Length != headers.Count)
            {
                throw new InvalidDataException($"Line {lineNumber} contains {values.Length} values,but {headers.Count} were expected.");
            }

            Dictionary<string, string> row = new();

            for (int i = 0; i < headers.Count; i++)
            {
                row[headers[i]] = values[i];
            }

            return row;
        }
    }
}
