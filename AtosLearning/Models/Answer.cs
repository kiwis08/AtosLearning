using System.Text.Json.Serialization;

namespace AtosLearning.Models;

public class Answer
{
    [JsonPropertyName("id")] public int Id { get; set; }
    
    [JsonPropertyName("text")]
    public string Title { get; set; }
    
    public int QuestionId { get; set; }
    
    [JsonPropertyName("isCorrect")]
    public bool IsCorrect { get; set; }
}