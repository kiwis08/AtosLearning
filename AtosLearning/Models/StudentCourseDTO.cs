using System.Text.Json.Serialization;

namespace AtosLearning.Models;

public class StudentCourseDTO
{
    [JsonPropertyName("studentId")]
    public int StudentId { get; set; }
    
    [JsonPropertyName("code")]
    public string CourseCode { get; set; }
    
    public StudentCourseDTO()
    {
    }
    
    
}