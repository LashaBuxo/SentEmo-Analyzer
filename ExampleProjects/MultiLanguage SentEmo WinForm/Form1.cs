using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SentEmo;

using Google.Apis;
using Google.Cloud.Translation.V2;

namespace MultiLanguage_SentEmo_WinForm
{
    public partial class Form1 : Form
    {
        private SentEmoAnalyzer analyzer;
        public Form1()
        {
            InitializeComponent();
            analyzer = new SentEmoAnalyzer();
            analyzer.Initialize();
            textBox3.Text = "AIzaSyDZucxgqPKlV7eFj1MYJh-iqn8QxQBfFbU";
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            TranslationClient client = TranslationClient.CreateFromApiKey(textBox3.Text);

            Detection languageSpecific = client.DetectLanguage(textBox1.Text);
            TranslationResult translationResult = client.TranslateText(textBox1.Text, LanguageCodes.English);
            EmotionState result= analyzer.DoAnalysis(translationResult.TranslatedText);
            string EmotionJson = result.ToJson();
             
            //if language is detected, then json is translated to source language
            if (languageSpecific.Confidence>0.5)
            {
                translationResult = client.TranslateText(EmotionJson, languageSpecific.Language);
                textBox2.Text = translationResult.TranslatedText;
            } else
            {
                textBox2.Text = EmotionJson;
            }
        }
    }
}
  