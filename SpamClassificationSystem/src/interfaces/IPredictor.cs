using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.interfaces
{
    public interface IPredictor
    {
        public string Predict(Dictionary<string, string> sample);
    }
}
