using System.Text.Json.Serialization;

namespace AtosLearning.Models;

public class Subject
{
    public int ID { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int TeacherId { get; set; }
    
    public int CourseId { get; set; }
    
    public Subject(int id, string name, string description, int teacherId, int courseId)
    {
        ID = id;
        Name = name;
        Description = description;
        TeacherId = teacherId;
        CourseId = courseId;
    }
}