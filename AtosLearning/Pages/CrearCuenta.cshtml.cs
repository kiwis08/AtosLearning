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
                new KeyValuePair<string, string>("Name", Nombre),
                new KeyValuePair<string, string>("Surname", Apellido),
                new KeyValuePair<string, string>("Email", Correo),
                new KeyValuePair<string, string>("Password", Contraseña),
                new KeyValuePair<string, string>("IsTeacher", Role == "Profesor" ? "true" : "false"),
                new KeyValuePair<string, string>("CharacterId", "0"),
                new KeyValuePair<string, string>("TotalScore", "0"),
                new KeyValuePair<string, string>("Nickname", Nombre),
                new KeyValuePair<string, string>("Image", "https://i.imgur.com/3Z4nB9K.png"),
                new KeyValuePair<string, string>("Id", "0"),
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