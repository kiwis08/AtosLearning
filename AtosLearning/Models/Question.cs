namespace AtosLearning.Models;

public class Question
{
    public int ID { get; set; }
    public string Title { get; set; }
    public Answer[] Answers { get; set; }
}