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
    // Reads and parses CSV files into a DataSet.
    public class CSVReader : IReader
    {
        // Stores the character used to separate CSV values.
        private readonly char _delimiter;

        // Creates a CSV reader with a comma as the default delimiter.
        public CSVReader(char delimiter = ',')
        {
            _delimiter = delimiter;
        }

        // Reads a CSV file and returns its contents as a DataSet.
        public DataSet Read(string path)
        {
            // Validates that the file path is not empty.
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("CSV path cannot be empty.", nameof(path));
            }

            // Validates that the CSV file exists.
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("CSV file was not found.", path);
            }

            // Reads all lines from the CSV file.
            string[] lines = File.ReadAllLines(path);

            // Validates that the file contains a header and at least one data row.
            if (lines.Length < 2)
            {
                throw new InvalidDataException("this file must contain a header and at least one data row.");
            }

            // Parses the first line as column headers.
            List<string> headers = ParseHeaders(lines[0]);

            // Stores all parsed data rows.
            List<Dictionary<string, string>> rows = new();

            // Iterates through all data lines except the header.
            for (int i = 1; i < lines.Length; i++)
            {
                // Skips empty lines.
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                // Parses the current line into a dictionary.
                Dictionary<string, string> row = ParseRow(lines[i], headers, i + 1);

                // Adds the parsed row to the row collection.
                rows.Add(row);
            }

            // Uses the last column as the target column.
            string targetColumn = headers[^1];

            // Extracts all target values from the rows.
            List<string> targets = rows
                .Select(row => row[targetColumn])
                .ToList();

            // Returns the parsed CSV data as a DataSet.
            return new DataSet(headers, rows, targets);
        }

        // Parses and validates the CSV header line.
        private List<string> ParseHeaders(string headerLine)
        {
            // Splits the header line and removes extra spaces.
            List<string> headers = headerLine
                .Split(_delimiter)
                .Select(header => header.Trim())
                .ToList();

            // Validates that the file contains at least one feature and one target column.
            if (headers.Count < 2)
            {
                throw new InvalidDataException("CSV file must contain at least one feature column and one target column.");
            }

            // Validates that no column name is empty.
            if (headers.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidDataException("CSV contains an empty column name.");
            }

            // Validates that all column names are unique.
            if (headers.Distinct().Count() != headers.Count)
            {
                throw new InvalidDataException("CSV contains duplicate column names.");
            }

            // Returns the validated header list.
            return headers;
        }

        // Parses one CSV data line into a dictionary.
        private Dictionary<string, string> ParseRow(
            string line,
            List<string> headers,
            int lineNumber)
        {
            // Splits the line into trimmed values.
            string[] values = line
                .Split(_delimiter)
                .Select(value => value.Trim())
                .ToArray();

            // Validates that the number of values matches the number of headers.
            if (values.Length != headers.Count)
            {
                throw new InvalidDataException($"Line {lineNumber} contains {values.Length} values,but {headers.Count} were expected.");
            }

            // Stores the values using the column names as keys.
            Dictionary<string, string> row = new();

            // Maps each header to its matching value.
            for (int i = 0; i < headers.Count; i++)
            {
                row[headers[i]] = values[i];
            }

            // Returns the parsed row.
            return row;
        }
    }
}