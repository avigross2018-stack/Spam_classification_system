using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.models
{
    public class DataSet
    {
        private List<string> _labels { get; set;}
        private List<Dictionary<string, string>> _rows {get; set;}
        private List<string> _target { get; set;}

        DataSet(List<string> labels, List<Dictionary<string, string>> rows, List<string> target)
        {
            _labels = labels;
            _rows = rows;
            _target = target;
        }

        DataSet(){}
    }
}