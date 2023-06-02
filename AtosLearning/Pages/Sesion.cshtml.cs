using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AtosLearning.Pages
{
	public class SesionModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            // Perform your login logic here
            // For example, check the email and password against a database
            if (Email == "example@example.com" && Password == "password")
            {
                // Successful login
                return RedirectToPage("/Dashboard"); // Redirect to the dashboard page
            }
            else
            {
                // Invalid credentials, show an error message
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }
        }
    }
}
