# Welcome to Sent-Emo Open Source  Project!

Sent-Emo Analyzer is the open source library, which analyses given **English** text by emotion and its sentiment values. Below are the example projects and the materials used in project.


# How it Works?

```c#
using SentEmo;

SentEmoAnalyzer analyzer = new SentEmoAnalyzer();
analyzer.Initialize();
EmotionState result = analyzer.DoAnalysis("I am happy that you came, Thanks for that.");
Console.WriteLine(result.ToJson());
```
And result looks like:
```json
{
  "Sentiment": 0.7,
  "Anger": 0.06,
  "Disgust": 0.11,
  "Fear": 0.09,
  "Joy": 0.47,
  "Sadness": 0.06,
  "Inclusion": 0.13
}
```

## Example Projects
In spite of a fact, that library only works for english text, in example projects there is Multi-Language analyzer which uses **Google Cloud Translator**. 

### Multi-Language Translator
| ![](example4.jpg) | ![](example3.jpg) |
|--|--|
| ![](example1.jpg) | ![](example2.jpg) |

## 3rd Party Resources

* [Apache OpenNLP](http://opennlp.apache.org/) - We use OpenNLP library for dividing each sentence, by speech parts and its semantic values
*  English words data organized with emotion score by [saif.mohammad](https://saifmohammad.com/WebPages/NRC-Emotion-Lexicon.htm)
