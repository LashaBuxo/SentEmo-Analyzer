using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.PosTagger;
using System.Reflection;
using System.IO;

namespace SentEmo
{
    public class SentEmoAnalyzer
    {
        /// <summary>
        /// Constant parameter for measuring
        /// high capital words factor
        /// </summary>
        const double high_capital_factor = 1.5;

        /// <summary>
        /// Initialization value for the analyzer
        /// </summary>
        public bool IsInitialized;

        /// <summary>
        /// Library which is used for
        /// analyzing sentences
        /// </summary>
        private PreProcessed_Library Library;

        /// <summary>
        /// Constructor
        /// </summary>
        public SentEmoAnalyzer()
        {
            IsInitialized = false;
        }

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        public void Initialize()
        {
            Library = new PreProcessed_Library();
            Library.LoadLibrary();

            IsInitialized = true;
        }

        /// <summary>
        /// Analyses <see cref="text"/> data
        /// </summary>
        /// <param name="text">
        /// actual text which should be analyzed
        /// </param>
        /// <returns>
        /// The <see cref="EmotionState"/> type variable is the result of this analyze
        /// which stores all the values being analyzed
        /// </returns>
        public EmotionState DoAnalysis(string text)
        {
            //count speech parts 
            string[] tokens = Library.tokenizer.Tokenize(text); //divide sentence into words
            for (int j = 1; j < tokens.Count(); j++)
            {
                if (tokens[j - 1] == ":" && tokens[j].Length <= 3) tokens[j] = ":" + tokens[j];
            }
            string[] tags = Library.posTagger.Tag(tokens); //and also get for each word what speech part it is

            EmotionState result = AnalyseSentence(tokens, tags); //merge each result for each sentence to get final result
             
            return result;
        }


        /// <summary>
        /// Analyze each tokens with speech parts
        /// </summary>
        private EmotionState AnalyseSentence(string[] words, string[] parts)
        {
            EmotionState Result = new EmotionState(); //calc is the total emotion and sentiment values of each words

            //here we are collecting info from emotion data
            foreach (string word in words)
            {
                double coeff = word == HelperMethods.ToLowRegistry(word) ? 1 : high_capital_factor;

                int[] values = null;
                if (Library.IsEmotionsAvaliable(word))
                    values = Library.GetEmotions(word);
                if (values != null)
                {
                    //increase values of each thing
                    Result.Anger += values[0] * coeff;
                    Result.Disgust += values[1] * coeff;
                    Result.Fear += values[2] * coeff;
                    Result.Joy += values[3] * coeff;
                    Result.Sadness += values[4] * coeff;
                }
            }

            double incl_val = 0, excl_val = 0;
            foreach (string word in words)
            {
                double coeff = word == HelperMethods.ToLowRegistry(word) ? 1 : high_capital_factor;

                if (Library.GetInclusionValue(word) > 0)
                {
                    incl_val += 1 * coeff;
                }

                if (Library.GetExclusionValue(word) > 0)
                {
                    excl_val += 1 * coeff;
                } 
            }

            double pos_val = Result.Joy + incl_val;
            double neg_val = excl_val + Result.Disgust + Result.Sadness + Result.Fear + Result.Anger;
            //lets translate emotions into positive and negative values, only joy is positive others are negative

            //here we are collecting results from positive and negative data
            foreach (string word in words)
            {
                double coeff = word == HelperMethods.ToLowRegistry(word) ? 1 : high_capital_factor;

                if (Library.IsPositive(word)) 
                    pos_val += 1 * coeff;
                if (Library.IsNegative(word)) 
                   neg_val += 1 * coeff;
            }
             
            //now we have collected anger,fear,sadness,joy,disgust pos_val,neg_val  incl_val,excl_val

            //before summarise we have to calculate syntax value 
            double syntax_value = MathAnalyzer.SyntaxValue(parts); //syntax value is a little point here
            //but improtant its range is from 0 to 0.1 

            Result.Sentiment =MathAnalyzer.CalculateSentiment(Result, pos_val, neg_val, incl_val, excl_val, syntax_value); //calculating sentiment

            Result = MathAnalyzer.CalculateEmotions(Result, syntax_value); //calculating emotions
            //it is improtant that before running you already have calculatid sentiment

            Result.Inclusion = MathAnalyzer.CalculateInclusion(Result, incl_val, excl_val, syntax_value);

            //rounding values 
            Result.Sentiment = Math.Round(Result.Sentiment, 2);
            Result.Fear = Math.Round(Result.Fear, 2);
            Result.Disgust = Math.Round(Result.Disgust, 2);
            Result.Anger = Math.Round(Result.Anger, 2);
            Result.Sadness = Math.Round(Result.Sadness, 2);
            Result.Joy = Math.Round(Result.Joy, 2);
            Result.Inclusion = Math.Round(Result.Inclusion, 2);

            //return result
            return Result;
        } 
    }
}
