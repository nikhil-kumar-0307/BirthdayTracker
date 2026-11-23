using System.ComponentModel.DataAnnotations;

namespace BirthdayTracker.Models.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Employee Number is required")]
        public string EmployeeNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        // "Employee" or "Admin" (from the tabs)
        public string LoginAs { get; set; }
    }
}