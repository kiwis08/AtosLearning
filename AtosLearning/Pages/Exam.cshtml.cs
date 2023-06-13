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
    public User CurrentUser;
    [BindProperty]
    public string ExamTitle { get; set; }
    [BindProperty]
    public string ExamDescription { get; set; }

    [BindProperty]
    public Exam CurrentExam { get; set; }
    
    [BindProperty]
    public int CurrentQuestionIndex { get; set; }
    
    public IList<Question> Questions;

    [BindProperty] public int QuestionsCount { get; set; } = 1;



    public Subject? SelectedSubject { get; set; }
    
    [BindProperty]
    public int CorrectAnswerIndex { get; set; }
    
    public ExamModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnGet()
    {
        // Get teacher from session state
        var userBytes = HttpContext.Session.Get("User");
        var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
        CurrentUser = user;
        
        
        CurrentExam = new Exam();   

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


    public async Task<List<Subject>> GetSubjects()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"Subjects/teacher/{CurrentUser.Id}";
        
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<List<Subject>>(json);
        return subjects ?? new List<Subject>();
    }
    
    
    
    public async Task UploadExam()
    {
        
    }

}