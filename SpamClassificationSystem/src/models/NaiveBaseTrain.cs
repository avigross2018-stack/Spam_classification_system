using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpamClassificationSystem.src.models;
using SpamClassificationSystem.src.interfaces;

namespace SpamClassificationSystem.src.models
{
    public class NaiveBaseTrain : ITrainer
    {
        private DataSet _data;

        public NaiveBaseTrain(DataSet data)
        {
            _data = data;
        }

        public NavieBaseModel Train()
        {
            int numberOfRows = _data.GetRows().Count;   //hold number of row in the table.
            string targetColumn = _data.GetLabels().Last();     //hold the last labelName (the target label name)
            
            
            List<string> distinctTarget =   // filter distinctTarget from target list. 
                _data.GetTarget()
                    .Distinct()
                    .ToList();
            

            Dictionary<string,double> priors = new();
            foreach (string target in distinctTarget)
            {
                int count = _data.GetTarget().Count(t => t == target);  //count how many every target exist.

                priors[target] = (double)count / numberOfRows;
            }


            Dictionary<(string Label, 
                        string Feature, 
                        string Value), double> cond = new();

            Dictionary<(string Label,
                        string Feature), double> unseen = new();                       

            foreach (var target in distinctTarget)
            {
                var targetRows = _data.GetRows()    //getting all rows for every target
                    .Where(r => r[targetColumn] == target)
                    .ToList();

                foreach(string feature in _data.GetLabels())     // go over every column except the target column
                {
                    if(feature == targetColumn)     //Skipping the target column
                        continue;

                    var distinctValues = _data.GetRows()    //all distinct values in every row
                        .Select(r => r[feature])
                        .Distinct()
                        .ToList();

                    int amountDistinctValues = distinctValues.Count;    //the amount of every distinct value


                    // calculate probability for every value
                    foreach(string value in distinctValues)
                    {
                        int match = targetRows.Count(r => r[feature] == value);     //count amount of rows for every distinct target column value
                    
                        double probability = (double)(match +1) / (targetRows.Count + amountDistinctValues);    //calculate the probability

                        cond[(target, feature, value)] = probability;
                    }

                    unseen[(target, feature)] = 1.0 / (targetRows.Count + amountDistinctValues);    //calculate the probability for unseen data
                }

                
            }
            return new NavieBaseModel(distinctTarget, priors, cond, unseen);
        }
    }
}