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
    
    public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();
    public User CurrentUser;
    [BindProperty]
    public string ExamTitle { get; set; }
    [BindProperty]
    public string ExamDescription { get; set; }

    [BindProperty]
    public Exam CurrentExam { get; set; }
    
    [BindProperty]
    public int CurrentQuestionIndex { get; set; }

    [BindProperty] public int QuestionsCount { get; set; } = 1;


    [BindProperty]
    public int SelectedSubjectId { get; set; }

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

        Subjects = await GetSubjects();
        
        CurrentExam = new Exam();
    }


    public async Task<List<Subject>> GetSubjects()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"Subjects/teacher/{CurrentUser.Id}";
        
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<List<Subject>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return subjects ?? new List<Subject>();
    }
    
    
    
    public async Task<bool> UploadExam()
    {
        for (int i = 0; i < CurrentExam.Questions.Length; i++)
        {
            CurrentExam.Questions[i].Answers[CurrentExam.Questions[i].CorrectAnswerIndex].IsCorrect = true;
        }
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"api/Exams/add";
        
        var client = new HttpClient();
        var jsonExam = JsonSerializer.Serialize(CurrentExam);
        var content= new StringContent(jsonExam, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();
        var success = JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return success;
    }

    public async Task<IActionResult> OnPost()
    {

        CurrentExam.SubjectId = SelectedSubjectId;
        await UploadExam();
        return RedirectToPage("Homepage");
    }

}