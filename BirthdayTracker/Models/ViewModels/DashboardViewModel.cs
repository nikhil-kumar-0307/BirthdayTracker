using System;
using System.Collections.Generic;

namespace BirthdayTracker.Models.ViewModels
{
    public class BirthdayItem
    {
        public PersonMaster Person { get; set; }
        public DateTime NextBirthday { get; set; }
        public int DaysLeft { get; set; }
        public int TurningAge { get; set; }
    }

    public class CategoryBirthdayGroup
    {
        public string Category { get; set; }
        public List<BirthdayItem> Items { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalPersons { get; set; }
        public int TodayCount { get; set; }
        public int UpcomingCount { get; set; }
        public int ThisMonthCount { get; set; }
        public int CategoryCount { get; set; }

        public List<BirthdayItem> TodayBirthdays { get; set; }
        public List<CategoryBirthdayGroup> UpcomingByCategory { get; set; }

        public List<PersonMaster> Persons { get; set; }   // current page only
        public List<string> Categories { get; set; }
        public string SelectedCategory { get; set; }
        public string Search { get; set; }
        public int UpcomingDays { get; set; }

        // paging
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalFiltered { get; set; }
        public int TotalPages { get; set; }
        public int FromRecord { get; set; }
        public int ToRecord { get; set; }

        public DashboardViewModel()
        {
            TodayBirthdays = new List<BirthdayItem>();
            UpcomingByCategory = new List<CategoryBirthdayGroup>();
            Persons = new List<PersonMaster>();
            Categories = new List<string>();
            Page = 1;
            PageSize = 25;
        }
    }
}