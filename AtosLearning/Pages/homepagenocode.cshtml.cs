using System.Text;
using AtosLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AtosLearning.Pages
{
    public class homepagenocodeModel : PageModel
    {
        
        private readonly IConfiguration _configuration;
        
        public homepagenocodeModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [BindProperty]
        public User CurrentUser { get; set; }
        
        
        [BindProperty]
        public string CourseCode { get; set; }
        
        public void OnGet()
        {
            var userBytes = HttpContext.Session.Get("User");
            var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
            CurrentUser = user;
            
        }

        public async Task<IActionResult> OnPost()
        {
            string connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + $"api/Courses";
            var userBytes = HttpContext.Session.Get("User");
            var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
            var dto = new StudentCourseDTO()
            {
                StudentId = user.Id,
                CourseCode = CourseCode
            };



            var client = new HttpClient();
            var dtoJson = JsonSerializer.Serialize(dto);
            var response = await client.PostAsync(url, 
                new StringContent(dtoJson, Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();
            var course = JsonSerializer.Deserialize<Course>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (course != null)
            {
                user.Course = course;
                HttpContext.Session.Set("User", JsonSerializer.SerializeToUtf8Bytes(user));
                return Redirect("/Homepage");
            }
            else
            {
                return Redirect("/homepagenocode");
            }
        }
    }
}
