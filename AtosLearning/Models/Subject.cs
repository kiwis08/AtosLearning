namespace AtosLearning.Models;

public class Subject
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int teacherID { get; set; }
    public int courseID { get; set; }
    
    public Subject(int id, string name, int teacherID, int courseID)
    {
        ID = id;
        Name = name;
        this.teacherID = teacherID;
        this.courseID = courseID;
    }
}