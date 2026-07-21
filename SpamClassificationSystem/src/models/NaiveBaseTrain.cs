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

        public NavieBaseModel Train(DataSet dataSet)
        {
            int numberOfRows = dataSet.GetRows().Count;   //hold number of row in the table.
            string targetColumn = dataSet.GetLabels().Last();     //hold the last labelName (the target label name)
            
            
            List<string> labels =   // filter distinctTarget from target list. 
                dataSet.GetTarget()
                    .Distinct()
                    .ToList();
            

            Dictionary<string,double> priors = new();
            foreach (string label in labels)
            {
                int count = dataSet.GetTarget().Count(t => t == label);  //count how many every target exist.

                priors[label] = (double)count / numberOfRows;
            }


            Dictionary<(string Label, 
                        string Feature, 
                        string Value), double> cond = new();

            Dictionary<(string Label,
                        string Feature), double> unseen = new();                       

            foreach (var label in labels)
            {
                var targetRows = dataSet.GetRows()    //getting all rows for every target
                    .Where(r => r[targetColumn] == label)
                    .ToList();

                foreach(string feature in dataSet.GetLabels())     // go over every column except the target column
                {
                    if(feature == targetColumn)     //Skipping the target column
                        continue;

                    var distinctValues = dataSet.GetRows()    //all distinct values in every row
                        .Select(r => r[feature])
                        .Distinct()
                        .ToList();

                    int amountDistinctValues = distinctValues.Count;    //the amount of every distinct value


                    // calculate probability for every value
                    foreach(string value in distinctValues)
                    {
                        int match = targetRows.Count(r => r[feature] == value);     //count amount of rows for every distinct target column value
                    
                        double probability = (double)(match +1) / (targetRows.Count + amountDistinctValues);    //calculate the probability

                        cond[(label, feature, value)] = probability;
                    }

                    unseen[(label, feature)] = 1.0 / (targetRows.Count + amountDistinctValues);    //calculate the probability for unseen data
                }

                    
            }
            return new NavieBaseModel(labels, priors, cond, unseen);
        }
    }
}