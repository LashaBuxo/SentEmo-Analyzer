using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentEmo
{
    class MathAnalyzer
    { 
        //from -1 to 1
        public static double calculate_sentiment(EmotionState info, double pos, double neg, double incl, double excl, double syntax_value)
        {

            if (pos == 0 && neg == 0 && incl == 0 && excl == 0 && info.Joy + info.Fear + info.Disgust + info.Sadness + info.Anger == 0)
            {
                //this means absolutily nothing is familiar in text,then 
                //we are calculating sentiment syntax
                info.Sentiment = syntax_value;
                return info.Sentiment;
            }
            double result = 0;
            if (pos >= neg)  //if positive is greater then negative,it includes emotions and inclusivity
            {
                //this means sentiment is positive,
                result = binary_function(0.9, (pos - neg + 1)) + syntax_value;
                return result;
            }
            else
            {
                result = binary_function(0.9, (neg - pos + 1)) + syntax_value;
                return -result;
                //so if it comes here it means sentiment is negative 
            }
            return result;
        }

        public static EmotionState calculate_emotions(EmotionState info, double syntax_value)
        {
            //about joy
            // 30% sentiment value, 60% already collected info.joy , 10% syntax value 
            info.Joy = Math.Max(syntax_value, 0.3 * info.Sentiment + binary_function(0.6, info.Joy) + syntax_value);

            //about sadnass
            // 40% sentiment value, 50% already collected info.joy , 10% syntax value 
            info.Sadness = Math.Max(syntax_value, -0.4 * info.Sentiment + binary_function(0.5, info.Joy) + syntax_value);

            //about disgust
            // 25% sentiment value, 65% already collected info.joy , 10% syntax value 
            info.Disgust = Math.Max(syntax_value, -0.25 * info.Sentiment + binary_function(0.65, info.Joy) + syntax_value);

            //about fear
            // 20% sentiment value, 70% already collected info.joy , 10% syntax value 
            info.Fear = Math.Max(syntax_value, -0.20 * info.Sentiment + binary_function(0.50, info.Joy) + syntax_value);

            //about anger
            // 30% sentiment value, 60% already collected info.joy , 10% syntax value 
            info.Anger = Math.Max(syntax_value, -0.30 * info.Sentiment + binary_function(0.60, info.Joy) + syntax_value);
            return info;
        }

        public static double calculate_inclusion(EmotionState info, double incl, double excl, double syntax_value)
        {
            double result = 0;
            if (incl + excl == 0)
            {
                //if there is no familiar inclusivity words in sentence
                //we should export some value from emotions and sentiment value to not export zero result

                if (info.Sentiment > 0)
                    result = 0.1 * info.Sentiment + syntax_value;
                else
                    result = 0.1 * info.Sentiment - syntax_value;
                return result;
            }
            if (incl >= excl)
            {
                result = binary_function(0.9, (incl - excl + 1)) + syntax_value;
                return result;
            }
            else
            {
                result = binary_function(0.9, (excl - incl + 1)) + syntax_value;
                return -result;
            }
        }

        public static double syntax_value(string[] parts) //we are calculating syntax value by speech parts in the sentence
        {
            int cnt1 = 0, cnt2 = 0, cnt3 = 0;
            for (int i = 0; i < parts.Count(); i++)
            {
                int stop1 = 0, stop2 = 0;
                for (int j = 0; j < parts[i].Length; j++)
                {
                    if (parts[i][j] == 'P') stop1 = 1;
                    if (parts[i][j] == 'V') stop2 = 1;
                }
                if (stop1 == 1) { cnt1++; } else { if (stop2 == 1) { cnt2++; } else { cnt3++; } };
            }
            cnt1 = Math.Max(cnt1, Math.Max(cnt2, cnt3));

            return cnt1 / (parts.Count() * 1.0) / 10.0;
        }
        public static double binary_function(double max_value, double cnt)
        {
            double result = 0, current = max_value / 3;
            for (int i = 0; i < cnt; i++)
            {
                result = result + current;
                current = (max_value - result) * 1 / 3;
            }

            return result;
        }

    }
}
