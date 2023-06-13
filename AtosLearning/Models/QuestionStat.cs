namespace AtosLearningAPI.Model;

public class QuestionStat
{
    public int QuestionId { get; set; }
    public string Question { get; set; }
    public int ExamId { get; set; }
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
}