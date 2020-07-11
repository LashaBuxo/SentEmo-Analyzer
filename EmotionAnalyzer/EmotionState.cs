namespace SentEmo
{ 
        public struct EmotionState
        {
            public double Sentiment;  //from -1 to 1 measures Sentiment
            public double Anger, Disgust, Fear, Joy, Sadness;  //from 0 to 1 measures Emotion
            public double Inclusion; //from -1 to 1  measures Inclusion
            
            public override string ToString()
            {
            return "Sentiment:" + Sentiment + "; Inclusion:" + Inclusion
                + "; Anger:" + Anger + "; Disgust:" + Disgust + "; Fear:" + Fear + 
                "; Joy:" + Joy + "; Sadness:" + Sadness;
            }
        }
}