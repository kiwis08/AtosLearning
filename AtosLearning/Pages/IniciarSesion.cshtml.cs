using System.Text.Json;
using AtosLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AtosLearning.Pages
{
    public class IniciarSesionModel : PageModel
    {
        
        private readonly IConfiguration _configuration;
        
        public IniciarSesionModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate user credentials against the REST API
            var url = _configuration.GetConnectionString("atoslearning") + "Auth";

            
            // Send a POST request to the REST API with the user's credentials in the form data
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", Email),
                new KeyValuePair<string, string>("password", Password)
            };
            request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);
            
            // If the response is not successful, return to the IniciarSesion page
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/IniciarSesion");
            }
            
            // Get the user's details from the REST API
            var user = JsonSerializer.Deserialize<User>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            
            // Save the user's details in session state
            HttpContext.Session.Set("User", JsonSerializer.SerializeToUtf8Bytes(user));
            
            // Redirect to the Homepage
            if (user.Course == null)
            {
                return RedirectToPage("/homepagenocode");
            }
            if (user.IsTeacher)
            {
                return RedirectToPage("/Homepage");
            }

            // Default redirect to the IniciarSesion page
            return RedirectToPage("/IniciarSesion");
        }
    }
}