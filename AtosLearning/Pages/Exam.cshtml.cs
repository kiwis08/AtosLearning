using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AtosLearning.Models;
using MySql.Data.MySqlClient;

namespace AtosLearning.Pages;

public class ExamModel : PageModel
{
    
    private readonly IConfiguration _configuration;
    
    public IList<Subject> Subjects { get; set; } = new List<Subject>();
    public Teacher CurrentTeacher;
    [BindProperty]
    public string ExamTitle { get; set; }
    [BindProperty]
    public string ExamDescription { get; set; }

    public IList<Question> Questions;
    
    [BindProperty]
    public Question CurrentQuestion { get; set; }



    public Subject? SelectedSubject { get; set; }
    
    [BindProperty]
    public int CorrectAnswerIndex { get; set; }
    
    public ExamModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async void OnGet()
    {
        CurrentTeacher = new Teacher(id: 2, name: "Santiago Quihui", image: "https://i.imgur.com/3ZtJZ1i.jpg");

        // Get the list of subjects from session state
        var subjectBytes = HttpContext.Session.Get("Subjects"); 
        var subjects = subjectBytes == null ? null : JsonSerializer.Deserialize<List<Subject>>(subjectBytes);

        // If the list is null (i.e. this is the first time the page has been loaded),
        // create a new list and add it to session state
        if (subjects == null)
        {
            subjects = await GetSubjects();
            var bytes = JsonSerializer.SerializeToUtf8Bytes(subjects);
            HttpContext.Session.Set("Subjects", bytes);
        }

        // Set the list of subjects as a property on the model so it can be used in the view
        Subjects = subjects;
    }
    
    public void OnPost()
    {
        // TODO: Save exam
        Debug.WriteLine($"{CurrentQuestion.Answers[0].Title}");
    }


    public void OnPostAddQuestion()
    {
        Debug.WriteLine(ExamTitle);
        // Get the list of subjects from session state
        var questionsBytes = HttpContext.Session.Get("Questions"); 
        var questions = questionsBytes == null ? null : JsonSerializer.Deserialize<List<Question>>(questionsBytes);

        // If the list is null (i.e. this is the first question being added),
        // create a new list and add it to session state
        if (questions == null)
        {
            questions = new List<Question>();
            questions.Add(CurrentQuestion);
            var bytes = JsonSerializer.SerializeToUtf8Bytes(questions);
            HttpContext.Session.Set("Questions", bytes);
        }

        // Set the list of questions as a property on the model so it can be used in the view
        Questions = questions;
        
        LoadState();
    }


    public void OnPostSaveExam()
    {
        LoadState();
        UploadExam();
    }
    
    public void OnPostSetSelectedSubject(int subjectId)
    {
        LoadState();
        SelectedSubject = Subjects.FirstOrDefault(s => s.ID == subjectId);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(SelectedSubject);
        HttpContext.Session.Set("SelectedSubject", bytes);
    }
    

    public async Task<List<Subject>> GetSubjects()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + "Subjects/teacher/1";
        
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<List<Subject>>(json);
        return subjects ?? new List<Subject>();
    }


    private void LoadState()
    {
        // Subjects
        // Get the list of subjects from session state
        var subjectBytes = HttpContext.Session.Get("Subjects"); 
        var subjects = subjectBytes == null ? null : JsonSerializer.Deserialize<List<Subject>>(subjectBytes);
        // Set the list of subjects as a property on the model so it can be used in the view
        Subjects = subjects;
        
        // Questions
        // Get the list of questions from session state
        var questionsBytes = HttpContext.Session.Get("Questions");
        var questions = questionsBytes == null ? null : JsonSerializer.Deserialize<List<Question>>(questionsBytes);
        Questions = questions;
        
        
        // Selected subject
        // Get the selected subject from session state
        var selectedSubjectBytes = HttpContext.Session.Get("SelectedSubject");
        var selectedSubject = selectedSubjectBytes == null ? null : JsonSerializer.Deserialize<Subject>(selectedSubjectBytes);
        SelectedSubject = selectedSubject;



        Debug.WriteLine("This is the exam title:");
        Debug.WriteLine(ExamTitle);
        // get exam title and description
        ExamTitle = Request.Form["ExamTitle"];
        ExamDescription = Request.Form["ExamDescription"];
        Debug.WriteLine(ExamTitle);
    }

    // Upload to database methods
    
    
    public void UploadExam()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = $"INSERT INTO Exams (exam_title, subject_id, exam_description) VALUES ('{ExamTitle}', {SelectedSubject.ID}, '{ExamDescription}')";
        cmd.ExecuteNonQuery();
        cmd.CommandText = $"SELECT * FROM Exams WHERE exam_title = '{ExamTitle}' AND subject_id = {SelectedSubject.ID} AND exam_description = '{ExamDescription}'";
        int examId = 0;
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                examId = Convert.ToInt32(reader["exam_id"]);
            }
        }
        
        connection.Dispose();
        //UploadQuestions(examId);
    }

}