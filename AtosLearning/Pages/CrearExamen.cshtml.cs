using Microsoft.AspNetCore.Mvc.RazorPages;
using AtosLearning.Models;
using System.Text.Json;


namespace AtosLearning.Pages;
public class CrearExamen : PageModel
{
    public User CurrentUser;
    public void OnGet()
    {
        var userBytes = HttpContext.Session.Get("User");
        var user = userBytes == null ? null : JsonSerializer.Deserialize<User>(userBytes);
        CurrentUser = user;
    }
}