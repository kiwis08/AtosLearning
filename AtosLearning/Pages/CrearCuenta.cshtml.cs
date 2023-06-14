using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AtosLearning.Pages
{
    public class CrearCuentaModel : PageModel
    {
        private readonly IConfiguration _configuration;
        
        public CrearCuentaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [BindProperty]
        public string Nombre { get; set; }
        
        [BindProperty]
        public string Apellido { get; set; }

        [BindProperty]
        public string Correo { get; set; }

        [BindProperty]
        public string Contraseña { get; set; }

        [BindProperty]
        public string Role { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var connectionString = _configuration.GetConnectionString("atoslearning");
            var url = connectionString + "Auth/register";
            
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name", Nombre),
                new KeyValuePair<string, string>("surname", Apellido),
                new KeyValuePair<string, string>("email", Correo),
                new KeyValuePair<string, string>("password", Contraseña),
                new KeyValuePair<string, string>("isTeacher", Role == "Profesor" ? "true" : "false"),
            };
            
            request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/CrearCuenta");
            }
            
            return RedirectToPage("/IniciarSesion");

        }
    }
}