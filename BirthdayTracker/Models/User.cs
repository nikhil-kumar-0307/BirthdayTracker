using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayTracker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string EmployeeNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; }          // "Admin" or "Employee"

        public DateTime? LastLoginAt { get; set; }
    }
}