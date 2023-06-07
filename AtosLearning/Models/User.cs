namespace AtosLearning.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string? Nickname { get; set; }
    public int? CharacterId { get; set; }
    public string Image { get; set; }
    public float? TotalScore { get; set; }
    public bool IsTeacher { get; set; }
    public Course Course { get; set; }
}