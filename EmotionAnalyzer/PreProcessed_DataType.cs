namespace SentEmo
{
    /// <summary>
    /// Library Data Types
    /// Positive: Text File contains all the words considered as Positive meaning
    /// Negative: Text File contains all the words considered as Negative meaning
    /// Inclusive: Text File contains all the words considered as Inclusive meaning
    /// Exclusive: Text File contains all the words considered as Exclusive meaning
    /// Emotion: Text File contains words with their Emotion Value
    /// </summary>
    enum PreProcessed_DataType
    {
        Positive,
        Negative,
        Inclusive,
        Exclusive,
        Emotion
    }
}