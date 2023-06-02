namespace AtosLearning.Models;

public class Exam
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int SubjectID { get; set; }
    public string Description { get; set; }
    public Question[] Questions { get; set; }
    public DateTime DueDateTime { get; set; }
}