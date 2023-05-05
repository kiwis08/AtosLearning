namespace AtosLearning.Models;

public class Teacher
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    
    public Teacher(int id, string name, string image)
    {
        ID = id;
        Name = name;
        Image = image;
    }
}