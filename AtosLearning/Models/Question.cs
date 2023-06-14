using System.Text.Json.Serialization;

namespace AtosLearning.Models;

public class Question
{
    [JsonPropertyName("id")]
    public int ID { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("answers")]
    public Answer[] Answers { get; set; }
    
    [JsonPropertyName("timeLimit")]
    public int TimeLimit { get; set; }
    
    [JsonPropertyName("examId")]
    public int ExamId { get; set; }
    public int CorrectAnswerIndex { get; set; }
}