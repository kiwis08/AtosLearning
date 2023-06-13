using AtosLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AtosLearning.Pages
{
    public class homepagenocodeModel : PageModel
    {


        [BindProperty]
        public User CurrentUser { get; set; }
        public void OnGet()
        {
            var userBytes = HttpContext.Session.Get("User");
            var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
            CurrentUser = user;

        }
    }
}
