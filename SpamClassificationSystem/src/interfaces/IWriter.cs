using SpamClassificationSystem.src.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.interfaces
{
    public interface IWriter
    {
        public void WritePrediction(Dictionary<string, string> row, List<string> headers, string prediction);
    }
}