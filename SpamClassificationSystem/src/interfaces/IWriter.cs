using SpamClassificationSystem.src.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.interfaces
{
    public interface IWriter
    {
        void WritePredictions(DataSet inputData,List<string> predictions);
    }
}