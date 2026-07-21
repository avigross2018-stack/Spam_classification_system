using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpamClassificationSystem.src.utils
{
    public class CSVReader: IReader
    {
        private readonly char _delimiter;

        public CsvReader(char delimiter = ',')
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
        }
    }
}