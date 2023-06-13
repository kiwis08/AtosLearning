using System.Diagnostics;
using System.Text.Json;
using AtosLearning.Models;
using AtosLearningAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AtosLearning.Pages
{
    public class ResultsModel : PageModel
    {
        
        private readonly IConfiguration _configuration;
        
        public ResultsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string ExamId { get; set; }
        public List<ExamSubmission> ExamSubmissions { get; set; } = new List<ExamSubmission>();
        public List<QuestionStat> QuestionStats { get; set; } = new List<QuestionStat>();
        
        public List<User> PendingSubmissions { get; set; } = new List<User>();
        public Exam CurrentExam { get; set; }
        
        public User CurrentTeacher { get; set; }

        public async Task OnGet(string examId)
        {
            ExamId = examId;
            var userBytes = HttpContext.Session.Get("User");
            var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
            CurrentTeacher = user;
            ExamSubmissions = await GetExamSubmissions();
            QuestionStats = await GetQuestionStats();
            PendingSubmissions = await GetPendingSubmissions();
            CurrentExam = await GetExam();
        }
        
        public async Task<List<ExamSubmission>> GetExamSubmissions()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"/api/Stats/exam/{ExamId}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var submissions = JsonSerializer.Deserialize<List<ExamSubmission>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return submissions ?? new List<ExamSubmission>();
        } 
        
        public async Task<List<QuestionStat>> GetQuestionStats()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"/api/Stats/{ExamId}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var stats = JsonSerializer.Deserialize<List<QuestionStat>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return stats ?? new List<QuestionStat>();
        }
        
        public async Task<List<User>> GetPendingSubmissions()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"/api/Stats/users/pending/{ExamId}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return users ?? new List<User>();
        }

        public async Task<Exam> GetExam()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"/api/Exams/{ExamId}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var exam = JsonSerializer.Deserialize<Exam>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return exam ?? new Exam();
        }
        
    }
}
