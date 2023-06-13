namespace AtosLearning.Models;

public class ExamSubmission
{
    public int ExamId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int Score { get; set; }
    public DateTime Date{ get; set; }
}