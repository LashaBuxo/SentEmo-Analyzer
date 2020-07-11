using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentEmo;
namespace Tests
{
    [TestClass]
    public class BasicTests
    {
        
        [TestMethod]
        public void TestInitialization()
        {
            SentEmoAnalyzer analyzer = new SentEmoAnalyzer(); 

            Assert.AreEqual(analyzer.IsInitialized, false);

            analyzer.Initialize();

            Assert.AreEqual(analyzer.IsInitialized, true); 
        }

        [TestMethod]
        public void TestBasicWord()
        {
            SentEmoAnalyzer analyzer = new SentEmoAnalyzer();  
            analyzer.Initialize();

            EmotionState result = analyzer.DoAnalysis("Hello World!");
        }

       
    }
}
