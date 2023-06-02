using System.Text.Json.Serialization;

namespace AtosLearning.Models;

public class Subject
{
    [JsonPropertyName("id")]
    public int ID { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("teacherId")]
    public int TeacherId { get; set; }
    
    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }
    
    public Subject(int id, string name, int teacherId, int courseId)
    {
        ID = id;
        Name = name;
        TeacherId = teacherId;
        CourseId = courseId;
    }
}