using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AtosLearning.Models;
using MySql.Data.MySqlClient;

namespace AtosLearning.Pages;

public class IndexModel : PageModel
{
    public IList<Subject> Subjects { get; set; }
    public Teacher CurrentTeacher;
    [BindProperty]
    public Exam NewExam { get; set; }

    public IList<Question> Questions;
    
    public Subject? SelectedSubject { get; set; }

    public void OnGet()
    {
        CurrentTeacher = new Teacher(id: 1, name: "Santiago Quihui", image: "https://i.imgur.com/3ZtJZ1i.jpg");
        Debug.WriteLine(ViewData["Testing"]);
        GetSubjects();
        NewExam = new Exam();
        Questions = new List<Question>();
    }
    
    public void AddQuestion(string title, string option1, string option2, string option3, string option4, int answer)
    {
        Question question = new Question();
        question.Title = title;
        question.Option1 = option1;
        question.Option2 = option2;
        question.Option3 = option3;
        question.Option4 = option4;
        question.Answer = answer;
        Questions.Add(question);
    }

    public void UploadExam()
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=reto;Uid=root;password=Santi66051*;";
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = $"INSERT INTO Exams (exam_title, subject_id, exam_description) VALUES ('{NewExam.Name}', {NewExam.SubjectID}, '{NewExam.Description}')";
        cmd.ExecuteNonQuery();
        connection.Dispose();
        UploadQuestions();
    }

    private void UploadQuestions()
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=reto;Uid=root;password=Santi66051*;";
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = connection;
        
        foreach (var question in Questions)
        {
            cmd.CommandText = $"INSERT INTO Questions (question_title, exam_id, option_1, option_2, option_3, option_4, answer) VALUES ('{question.Title}', {NewExam.ID}, '{question.Option1}', '{question.Option2}', '{question.Option3}', '{question.Option4}', {question.Answer})";
            cmd.ExecuteNonQuery();
        }
        connection.Dispose();
    }
    
    public void OnPostAddQuestion()
    {
        AddQuestion(Request.Form["title"], Request.Form["option1"], Request.Form["option2"], Request.Form["option3"], Request.Form["option4"], Convert.ToInt32(Request.Form["answer"]));
    }
    
    
    public void OnPostSetSelectedSubject(int subjectId)
    {
        // GetTeacher();
        // GetSubjects();
        SelectedSubject = Subjects.FirstOrDefault(s => s.ID == subjectId);
    }
    

    public void OnPost()
    {
        
    }


    public void GetTeacher()
    {
        CurrentTeacher = new Teacher(id: 1, name: "Santiago Quihui", image: "https://i.imgur.com/3ZtJZ1i.jpg");
    }


    public void GetSubjects()
    {
        Subjects = new List<Subject>();
        string connectionString = "Server=127.0.0.1;Port=3306;Database=reto;Uid=root;password=Santi66051*;";
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = $"SELECT * FROM Subjects WHERE teacher_id = {CurrentTeacher.ID}";
        
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Subject subject = new Subject(id: Convert.ToInt32(reader["subject_id"]), name: reader["subject_name"].ToString(), teacherID: Convert.ToInt32(reader["teacher_id"]), courseID: Convert.ToInt32(reader["course_id"]));
                Subjects.Add(subject);
            }
        }
        
        connection.Dispose();
    }
}