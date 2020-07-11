using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.PosTagger;
namespace SentEmo
{
    class PreProcessed_Library
    {
        public bool isLibraryLoaded = false;

        public EnglishMaximumEntropySentenceDetector sentenceDetector;
        public EnglishMaximumEntropyTokenizer tokenizer;
        public EnglishMaximumEntropyPosTagger posTagger;

        private Dictionary<string, bool> positive_words;
        private Dictionary<string, bool> negative_words;
        private Dictionary<string, int[]> emotion_words;
        private Dictionary<string, int> inclusion_values;
        private Dictionary<string, int> exclusion_values;

        public PreProcessed_Library()
        {
            positive_words = new Dictionary<string, bool>();
            negative_words = new Dictionary<string, bool>();
            emotion_words = new Dictionary<string, int[]>();
            inclusion_values = new Dictionary<string, int>();
            exclusion_values = new Dictionary<string, int>();

            isLibraryLoaded = false;
        }

        public void LoadLibrary()
        {
            try
            {
                //import models for divide text data into sentences,and sentences into single words(with syntax and etc.)
                 
                HelperMethods.CreateResourceInFileSystem("EnglishSD.nbin");
                HelperMethods.CreateResourceInFileSystem("EnglishTok.nbin");
                HelperMethods.CreateResourceInFileSystem("EnglishPOS.nbin");

                sentenceDetector = new EnglishMaximumEntropySentenceDetector("EnglishSD.nbin");
                tokenizer = new EnglishMaximumEntropyTokenizer("EnglishTok.nbin");
                posTagger = new EnglishMaximumEntropyPosTagger("EnglishPOS.nbin");

                positive_words = HelperMethods.import_positive_words(); //so data of positive words
                negative_words = HelperMethods.import_negative_words(); //data of negative words
                emotion_words = HelperMethods.import_emotion_words();  //data of emotion words with 5 expression values
                inclusion_values = HelperMethods.import_inclusion_words();  //data of inclusion with rate
                exclusion_values = HelperMethods.import_exclusion_words();  //data of inclusion with rate

                isLibraryLoaded = true;
            } catch (Exception e)
            {
                isLibraryLoaded = false;
                throw new Exception("Error in library load state", e);
            }
        }

        public void AddData(string word, PreProcessed_DataType dataType, int[] emotions = null, int value = 0)
        {
            switch (dataType)
            {
                case PreProcessed_DataType.Positive:
                    positive_words.Add(word, true);
                    break;
                case PreProcessed_DataType.Negative:
                    negative_words.Add(word, true);
                    break;
                case PreProcessed_DataType.Inclusive:
                    inclusion_values.Add(word, value);
                    break;
                case PreProcessed_DataType.Exclusive:
                    exclusion_values.Add(word, value);
                    break;
                case PreProcessed_DataType.Emotion:
                    emotion_words.Add(word, emotions);
                    break;
                default:
                    throw new Exception("Innacurate dataType");
            }
        }

        public bool IsPositive(string word)
        {
            return positive_words.ContainsKey(word);
        }
        public bool IsNegative(string word)
        {
            return negative_words.ContainsKey(word);
        }

        public bool IsEmotionsAvaliable(string word)
        {
            return emotion_words.ContainsKey(word);
        }
        public int[] GetEmotions(string word)
        {
            if (!IsEmotionsAvaliable(word))
            {
                throw new Exception("Word emotion data not available.");
            }
            return emotion_words[word];
        }

        public int GetInclusionValue(string word)
        {
            if (inclusion_values.ContainsKey(word))
                return inclusion_values[word];
            return 0;
        }
        public int GetExclusionValue(string word)
        {
            if (exclusion_values.ContainsKey(word))
                return exclusion_values[word];
            return 0;
        }
    }
}
 
