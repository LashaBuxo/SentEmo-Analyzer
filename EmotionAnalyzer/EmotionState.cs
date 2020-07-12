namespace SentEmo
{
    /// <summary>
    /// Structure for storing sentiment-emotion
    /// analyzed result
    /// </summary>
    public struct EmotionState
    {
        /// <summary>
        /// Sentiment value from -1 to 1
        /// (-1 = Very negative, 1 = Very positive)
        /// </summary>
        public double Sentiment;

        /// <summary>
        /// Emotion value from 0 to 1. For each emotion type
        /// 0 is considered as zero-emotional in current type of emotion
        /// 1 is considered as very-emotional in current type of emotion
        /// </summary>
        public double Anger, Disgust, Fear, Joy, Sadness;

        /// <summary>
        /// Inclusivity value from -1 to 1
        /// (-1 = Very Exclusive, 1 = Very Inclusive)
        /// </summary>
        public double Inclusion;

        /// <summary>
        /// Overriden Method for debug purpose 
        /// </summary>
        public override string ToString()
        {
            return "Sentiment:" + Sentiment + "; Inclusion:" + Inclusion
                + "; Anger:" + Anger + "; Disgust:" + Disgust + "; Fear:" + Fear +
                "; Joy:" + Joy + "; Sadness:" + Sadness;
        }
    }
}