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
    
    [BindProperty]
    public User CurrentUser { get; set; }

    [BindProperty]
    public Exam CurrentExam { get; set; }
    
    [BindProperty]
    public Question CurrentQuestion { get; set; }

    [BindProperty] public int CurrentQuestionIndex { get; set; }

    

    public ExamModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        var userBytes = HttpContext.Session.Get("User");
        var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
        CurrentUser = user;
        
        
        var examJson = TempData["NewExam"] as string;
        var exam = examJson == null ? null : JsonSerializer.Deserialize<Exam>(examJson);
        CurrentExam = exam ?? new Exam();
        
        // Save exam again in TempData
        TempData["NewExam"] = JsonSerializer.Serialize(CurrentExam);
        
        CurrentQuestionIndex = TempData["CurrentQuestionIndex"] == null ? 0 : (int)TempData["CurrentQuestionIndex"];
        
        // Save current question index again in TempData
        TempData["CurrentQuestionIndex"] = CurrentQuestionIndex;

    }



    public async Task<bool> UploadExam(Exam exam)
    {
        for (int i = 0; i < exam.Questions.Count; i++)
        {
            exam.Questions[i].Answers[exam.Questions[i].CorrectAnswerIndex].IsCorrect = true;
        }
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"api/Exams/add";
        
        var client = new HttpClient();
        var jsonExam = JsonSerializer.Serialize(exam);
        var content= new StringContent(jsonExam, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();
        var success = JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return success;
    }

    public IActionResult OnPostSaveQuestion()
    {
        var examJson = TempData["NewExam"] as string;
        var exam = examJson == null ? null : JsonSerializer.Deserialize<Exam>(examJson);
        CurrentExam = exam;
        
        CurrentQuestionIndex = TempData["CurrentQuestionIndex"] == null ? 0 : (int)TempData["CurrentQuestionIndex"];
        
        if (CurrentExam.Questions == null)
        {
            CurrentExam.Questions = new List<Question>();
        }
        
        // Save current question to exam object
        CurrentExam.Questions.Insert(CurrentQuestionIndex, CurrentQuestion);
        // Reset current question
        CurrentQuestion = new Question();
        // Increment question index
        CurrentQuestionIndex++;
        // Save exam again in TempData
        TempData["NewExam"] = JsonSerializer.Serialize(CurrentExam);
        
        // Save question index in TempData
        TempData["CurrentQuestionIndex"] = CurrentQuestionIndex;
        
        // Return to same page
        return RedirectToPage("Exam");
    }
    
    public async Task<IActionResult> OnPostSaveExam()
    {
        // Get exam from TempData
        var examJson = TempData["NewExam"] as string;
        var exam = examJson == null ? null : JsonSerializer.Deserialize<Exam>(examJson);
        CurrentExam = exam;
        
        CurrentQuestionIndex = TempData["CurrentQuestionIndex"] == null ? 0 : (int)TempData["CurrentQuestionIndex"];
        
        // Save current question to exam object
        CurrentExam.Questions.Insert(CurrentQuestionIndex, CurrentQuestion);
        // Reset current question
        CurrentQuestion = new Question();
        // Increment question index
        CurrentQuestionIndex++;

        // Upload exam to database
        var success = await UploadExam(CurrentExam);
        // If upload was successful, redirect to homepage
        if (success)
        {
            return RedirectToPage("Homepage");
        }
        // Otherwise, return to same page
        return RedirectToPage("Exam");
    }

}