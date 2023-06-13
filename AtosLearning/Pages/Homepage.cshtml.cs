using System.Diagnostics;
using System.Text;
using System.Text.Json;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AtosLearning.Models;
using MySql.Data.MySqlClient;



namespace AtosLearning.Pages;

	public class HomepageModel : PageModel
{

    private readonly IConfiguration _configuration;

    public IList<Subject> Subjects { get; set; } = new List<Subject>();
    public IList<Exam> Exams { get; set; } = new List<Exam>();
    public IList<Exam> ActiveExams { get; set; } = new List<Exam>();
    public IList<Exam> ClosedExams { get; set; } = new List<Exam>();
    
    [BindProperty]
    public User CurrentUser { get; set; }
    [BindProperty]

    public Subject? SelectedSubject { get; set; }
    
    [BindProperty]
    public string NewSubjectName { get; set; }
    
    [BindProperty]
    public string NewSubjectDescription { get; set; }


    public HomepageModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task OnGet()
    {


        var userBytes = HttpContext.Session.Get("User");
        var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
        CurrentUser = user;
        
        ActiveExams = await GetActiveExams();
        ClosedExams = await GetInactiveExams();

        
        

        Subjects = await GetSubjects();

    }

    private async Task<IList<Exam>> GetExams()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"api/Exams/course/{CurrentUser.Course.Id}";

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var exams = JsonSerializer.Deserialize<List<Exam>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return exams ?? new List<Exam>();
    }
    
    private async Task<IList<Exam>> GetActiveExams()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"api/Exams/active/{CurrentUser.Course.Id}";

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var exams = JsonSerializer.Deserialize<List<Exam>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return exams ?? new List<Exam>();
    }
    
    private async Task<IList<Exam>> GetInactiveExams()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"api/Exams/inactive/{CurrentUser.Course.Id}";

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var exams = JsonSerializer.Deserialize<List<Exam>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return exams ?? new List<Exam>();
    }


    public async Task<List<Subject>> GetSubjects()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"Subjects/teacher/{CurrentUser.Id}";
        if (CurrentUser.IsTeacher == false)
        {
            url = connectionString + $"Subjects/student/{CurrentUser.Id}";
        }

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<List<Subject>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return subjects ?? new List<Subject>();
    }

    public void OnPostSetSelectedSubject(int subjectId)
    {
        LoadState();
        SelectedSubject = Subjects.FirstOrDefault(s => s.ID == subjectId);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(SelectedSubject);
        HttpContext.Session.Set("SelectedSubject", bytes);
    }
    private void LoadState()
    {
        // Subjects
        // Get the list of subjects from session state
        var subjectBytes = HttpContext.Session.Get("Subjects");
        var subjects = subjectBytes == null ? null : JsonSerializer.Deserialize<List<Subject>>(subjectBytes);
        // Set the list of subjects as a property on the model so it can be used in the view
        Subjects = subjects;

        // Selected subject
        // Get the selected subject from session state
        var selectedSubjectBytes = HttpContext.Session.Get("SelectedSubject");
        var selectedSubject = selectedSubjectBytes == null ? null : JsonSerializer.Deserialize<Subject>(selectedSubjectBytes);
        SelectedSubject = selectedSubject;
    }

    public async Task<IActionResult> OnPostCreateSubject()
    {
        string connectionString = _configuration.GetConnectionString("atoslearning");
        var url = connectionString + $"Subjects";
        
        var teacherBytes = HttpContext.Session.Get("User");
        var teacher = teacherBytes == null ? null : JsonSerializer.Deserialize<User>(teacherBytes);

        var client = new HttpClient();
        var response = await client.PostAsync(url, 
            new StringContent(JsonSerializer.Serialize(
                new Subject()
                {
                    Name = NewSubjectName, 
                    Description = NewSubjectDescription,
                    CourseId = teacher.Course.Id
                }), Encoding.UTF8, "application/json"));
        var json = await response.Content.ReadAsStringAsync();
        var success = JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return Redirect("/Homepage");
    }
    }
