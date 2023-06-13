namespace AtosLearning.Models;

public class Exam
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int SubjectId { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string ImageUrl { get; set; }
}