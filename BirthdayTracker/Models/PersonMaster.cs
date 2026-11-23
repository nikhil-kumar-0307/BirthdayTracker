using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirthdayTracker.Models
{
    public class PersonMaster
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [Display(Name = "Salutation")]
        public string Salutation { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [MaxLength(100)]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(50)]
        public string Category { get; set; }

        [MaxLength(150)]
        [Display(Name = "Office / Organization")]
        public string Organization { get; set; }

        [MaxLength(100)]
        [Display(Name = "District / Constituency")]
        public string Area { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        [MaxLength(15)]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [MaxLength(15)]
        [Display(Name = "Office / Alternate Phone")]
        public string OfficePhone { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(100)]
        [Display(Name = "PA / Contact Person")]
        public string PaName { get; set; }

        [MaxLength(15)]
        [Display(Name = "PA Mobile")]
        public string PaMobile { get; set; }

        [MaxLength(300)]
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public PersonMaster()
        {
            IsActive = true;
        }

        [NotMapped]
        public string DisplayName
        {
            get { return string.IsNullOrWhiteSpace(Salutation) ? FullName : Salutation + " " + FullName; }
        }

        [NotMapped]
        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
                return age;
            }
        }
    }

    public static class PersonLists
    {
        public static readonly List<string> Categories = new List<string>
        {
            "Administration", "Political", "Police", "Judiciary", "Media",
            "Medical", "Education", "Business", "Social", "Other"
        };

        public static readonly List<string> Salutations = new List<string>
        {
            "Shri", "Smt.", "Dr.", "Prof.", "Er.", "Hon'ble", "Justice", "Ms.", "Mr."
        };

        public static readonly List<string> Designations = new List<string>
        {
            "DM", "ADM", "SDM", "BDO", "SP", "DSP", "SHO", "MLA", "MLC", "MP",
            "Mayor", "Zila Parishad Chairman", "District Judge", "Civil Surgeon",
            "Vice Chancellor", "Editor", "Journalist", "Chairman", "Industrialist"
        };
    }
}