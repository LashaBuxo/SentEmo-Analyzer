using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentEmo;
namespace Tests
{
    [TestClass]
    public class SentimentTests
    {
        [TestMethod]
        public void TestEmotion1()
        {
            SentEmoAnalyzer analyzer = new SentEmoAnalyzer();
            analyzer.Initialize();

            EmotionState result = analyzer.DoAnalysis("I am happy that you came, Thanks for that.");
            Assert.IsTrue(result.Sentiment > 0);
        }

        //Test For Happy Popular Lyrics
        [TestMethod]
        public void TestEmotion2()
        {
            SentEmoAnalyzer analyzer = new SentEmoAnalyzer();
            analyzer.Initialize();

            EmotionState result = analyzer.DoAnalysis(@"(Because I'm happy)
Clap along if you feel like a room without a roof
(Because I'm happy)
Clap along if you feel like happiness is the truth
(Because I'm happy)
Clap along if you know what happiness is to you
(Because I'm happy)
Clap along if you feel like that's what you wanna do

Here come bad news, talking this and that(Yeah!)
Well, give me all you got, don't hold it back (Yeah!)
Well, I should probably warn you I'll be just fine (Yeah!)
No offense to you
Don't waste your time, here's why

(Because I'm happy)
Clap along if you feel like a room without a roof
(Because I'm happy)
Clap along if you feel like happiness is the truth
(Because I'm happy)
Clap along if you know what happiness is to you
(Because I'm happy)
Clap along if you feel like that's what you wanna do");

            Assert.IsTrue(result.Sentiment >= 0); 
        }

        //Test For Sad Lyrics
        [TestMethod]
        public void TestEmotion3()
        {
            SentEmoAnalyzer analyzer = new SentEmoAnalyzer();
            analyzer.Initialize();

            EmotionState result = analyzer.DoAnalysis(@"You and I, we're like fireworks and symphonies exploding in the sky
With you, I'm alive
Like all the missing pieces of my heart, they finally collide
So stop time right here in the moonlight
'Cause I don't ever wanna close my eyes
Without you, I feel broke
Like I'm half of a whole
Without you, I've got no hand to hold
Without you, I feel torn
Like a sail in a storm
Without you, I'm just a sad song
I'm just a sad song
With you, I fall
It's like I'm leaving all my past and silhouettes up on the wall
With you, I'm a beautiful mess
It's like we're standing hand and hand with all our fears up on the edge
So stop time right here in the moonlight
'Cause I don't ever wanna close my eyes
Without you, I feel broke
Like I'm half of a whole
Without you, I've got no hand…");

            Assert.IsTrue(result.Sentiment <= 0);

        }
    }
}
