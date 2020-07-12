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
    /// <summary>
    /// Library which contains all the
    /// used preprocessed resources for analyze
    /// </summary>
    class PreProcessed_Library
    {
        public bool isLibraryLoaded = false;

        /// <summary>
        /// NLP Objects/Tool which analyze
        /// senteces/words by semantic rules
        /// </summary>
        public EnglishMaximumEntropySentenceDetector sentenceDetector;

        /// <summary>
        /// NLP Objects/Tool which analyze
        /// senteces/words by semantic rules
        /// </summary>
        public EnglishMaximumEntropyTokenizer tokenizer;

        /// <summary>
        /// NLP Objects/Tool which analyze
        /// senteces/words by semantic rules
        /// </summary>
        public EnglishMaximumEntropyPosTagger posTagger;


        private Dictionary<string, bool> positive_words;
        private Dictionary<string, bool> negative_words;
        private Dictionary<string, int[]> emotion_words;
        private Dictionary<string, int> inclusion_values;
        private Dictionary<string, int> exclusion_values;


        /// <summary>
        /// Constructor of the library
        /// </summary>
        public PreProcessed_Library()
        {
            positive_words = new Dictionary<string, bool>();
            negative_words = new Dictionary<string, bool>();
            emotion_words = new Dictionary<string, int[]>();
            inclusion_values = new Dictionary<string, int>();
            exclusion_values = new Dictionary<string, int>();

            isLibraryLoaded = false;
        }


        /// <summary>
        /// Loads all the nescesary resources
        /// for the library
        /// </summary>
        public void LoadLibrary()
        {
            try
            {
                HelperMethods.CreateResourceInFileSystem("EnglishSD.nbin");
                HelperMethods.CreateResourceInFileSystem("EnglishTok.nbin");
                HelperMethods.CreateResourceInFileSystem("EnglishPOS.nbin");

                sentenceDetector = new EnglishMaximumEntropySentenceDetector("EnglishSD.nbin");
                tokenizer = new EnglishMaximumEntropyTokenizer("EnglishTok.nbin");
                posTagger = new EnglishMaximumEntropyPosTagger("EnglishPOS.nbin");

                positive_words = HelperMethods.Import_PositiveWords(); //data of positive words
                negative_words = HelperMethods.Import_NegativeWords(); //data of negative words
                emotion_words = HelperMethods.Import_EmotionWords();  //data of emotion words with 5 expression values
                inclusion_values = HelperMethods.Import_InclusiveWords();  //data of inclusion with rate
                exclusion_values = HelperMethods.Import_ExclusionWords();  //data of inclusion with rate

                isLibraryLoaded = true;
            } catch (Exception e)
            {
                isLibraryLoaded = false;
                throw new Exception("Error in library load state", e);
            }
        }

        /// <summary>
        /// Adding single data in Library
        /// </summary>
        /// <param name="word">
        /// word which are being added in library
        /// </param>
        /// <param name="dataType">
        /// Type of the data 
        /// </param>
        /// <param name="emotions">
        /// If Included, corresponding Emotion array for word
        /// </param>
        /// <param name="value">
        /// If Included, corresponding Inlusivity value for word
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
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


 #region LibraryMethods
        public bool IsPositive(string word)
        {
            word = HelperMethods.ToLowRegistry(word);
            return positive_words.ContainsKey(word);
        }
        public bool IsNegative(string word)
        {
            word = HelperMethods.ToLowRegistry(word);
            return negative_words.ContainsKey(word);
        }

        public bool IsEmotionsAvaliable(string word)
        {
            word = HelperMethods.ToLowRegistry(word);
            return emotion_words.ContainsKey(word);
        }
        public int[] GetEmotions(string word)
        {
            word = HelperMethods.ToLowRegistry(word);
            if (!IsEmotionsAvaliable(word))
            {
                throw new Exception("Word emotion data not available.");
            }
            return emotion_words[word];
        }

        public int GetInclusionValue(string word)
        {
            word = HelperMethods.ToLowRegistry(word);
            if (inclusion_values.ContainsKey(word))
                return inclusion_values[word];
            return 0;
        }
        public int GetExclusionValue(string word)
        {
            word = HelperMethods.ToLowRegistry(word);
            if (exclusion_values.ContainsKey(word))
                return exclusion_values[word];
            return 0;
        }
    }
#endregion
}

