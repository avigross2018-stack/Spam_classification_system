using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.models
{
    public class NavieBaseModel
    {
        // Stores all possible labels that the model can predict.
        public List<string> Lebels { get; }
        // Stores the prior probability of each label.
        public Dictionary<string, double> Prios { get; }
        // Stores the conditional probability for every combination of: label, feature, and feature value.
        public Dictionary<(string, string, string), double> ConditionalProbabilities { get; }
        // Stores the probability used when a feature value was not seen during the training process.
        public Dictionary<(string, string) ,double> UnseenProbabilities { get; }

        public NavieBaseModel(List<string> lebels,
            Dictionary<string, double> prios,
            Dictionary<(string, string, string), double> conditionalProbabilities,
            Dictionary<(string, string), double> unseenProbabilities)
        {
            Lebels = lebels;
            Prios = prios;
            ConditionalProbabilities = conditionalProbabilities;
            UnseenProbabilities = unseenProbabilities;
        }
        // Returns the conditional probability for a specific label, feature, and value.
        // If the value was not seen during training, the method returns the unseen probability instead.
        public double GetProbability(string lebel, string feature,string value)
        {
            // Creates a key that represents the requested combination.
            var key = (lebel, feature, value);
            // Checks whether this combination exists in the trained model.
            if (ConditionalProbabilities.ContainsKey(key))
            {
                // Returns the probability calculated during training.
                return ConditionalProbabilities[key];
            }
            // Returns the fallback probability for an unseen value.
            return UnseenProbabilities[(lebel, feature)];
        }

    }
}