using System.Text.Json.Serialization;

namespace AtosLearning.Models;

public class Exam
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("subjectId")]
    public int SubjectId { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }
    
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = "https://via.placeholder.com/150";
    
    [JsonPropertyName("questions")]
    public Question[] Questions { get; set; }
    
}