using SpamClassificationSystem.src.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.models
{
    public class NaiveBasePredictor: IPredictor
    {
        private readonly NavieBaseModel _model;

        // Receives a trained model for making predictions.
        public NaiveBasePredictor(NavieBaseModel model)
        {
            _model = model;
        }

        // Predicts the most likely label for a new sample.
        public string Predict(Dictionary<string, string> sample)
        {
            string? bestLabel = null;
            double bestScore = double.NegativeInfinity;

            foreach (string label in _model.Lebels)
            {
                double score = _model.Prios[label];

                foreach (var item in sample)
                {
                    string feature = item.Key;
                    string value = item.Value;

                    var conditionKey = (label, feature, value);

                    if (_model.ConditionalProbabilities.TryGetValue(conditionKey, out double probability))
                    {
                        score *= probability;
                    }
                    else
                    {
                        var unseenKey = (label, feature);

                        if (_model.UnseenProbabilities.TryGetValue(unseenKey, out double unseenProbability))
                        {
                            score *= unseenProbability;
                        }
                        else
                        {
                            throw new InvalidOperationException($"No probability was found for feature '{feature}'.");
                        }
                    }
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestLabel = label;
                }
            }

            return bestLabel?? throw new InvalidOperationException( "Prediction failed because the model contains no labels.");
        }
    }
}
