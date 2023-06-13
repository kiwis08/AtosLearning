using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using AtosLearning.Models;
using AtosLearningAPI.Model;

namespace AtosLearning.Pages
{
    public class resultadoAlumno : PageModel
    {
        private readonly IConfiguration _configuration;
        
        public resultadoAlumno(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string ExamId { get; set; } = "10";

        public Exam CurrentExam { get; set; }
        public ExamSubmission CurrentExamSubmission { get; set; }
        
        public User CurrentUser { get; set; }
        public List<AnswerSubmission> AnswersData { get; set; }

        public async Task OnGet()
        {
            var userBytes = HttpContext.Session.Get("User");
            var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
            CurrentUser = user;
            CurrentExam = await GetExam();
            AnswersData = await GetAnswers();
            CurrentExamSubmission = await GetExamSubmission();
        }

        public async Task<ExamSubmission> GetExamSubmission()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"/api/Stats/submission/{CurrentUser.Id}/{ExamId}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var examSub = JsonSerializer.Deserialize<ExamSubmission>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return examSub ?? new ExamSubmission();

        }
        public async Task<List<AnswerSubmission>> GetAnswers()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"/api/Stats/users/{CurrentUser.Id}/{ExamId}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var answers = JsonSerializer.Deserialize<List<AnswerSubmission>>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return answers ?? new List<AnswerSubmission>();
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
