using System;
using System.Linq;
using System.Web.Mvc;
using BirthdayTracker.Data;
using BirthdayTracker.Models;
using BirthdayTracker.Models.ViewModels;

namespace BirthdayTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private static readonly int[] PageSizes = { 10, 25, 50, 100 };

        public ActionResult Index(string category, string search, int days = 30, int page = 1, int pageSize = 25)
        {
            if (days < 1 || days > 365) days = 30;
            if (!PageSizes.Contains(pageSize)) pageSize = 25;

            var today = DateTime.Today;
            var all = _db.PersonMasters.Where(p => p.IsActive).ToList();
            var items = all.Select(p => BuildItem(p, today)).ToList();

            var todayList = items
                .Where(i => i.DaysLeft == 0)
                .OrderBy(i => i.Person.Category)
                .ThenBy(i => i.Person.FullName)
                .ToList();

            var upcoming = items
                .Where(i => i.DaysLeft > 0 && i.DaysLeft <= days)
                .OrderBy(i => i.DaysLeft)
                .ToList();

            var upcomingByCategory = upcoming
                .GroupBy(i => i.Person.Category)
                .OrderBy(g => g.Key)
                .Select(g => new CategoryBirthdayGroup
                {
                    Category = g.Key,
                    Items = g.OrderBy(i => i.DaysLeft).ToList()
                })
                .ToList();

            // ---- filter ----
            var table = all.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(category))
                table = table.Where(p => p.Category == category);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.Trim().ToLower();
                table = table.Where(p =>
                    (p.FullName ?? "").ToLower().Contains(s) ||
                    (p.Designation ?? "").ToLower().Contains(s) ||
                    (p.Organization ?? "").ToLower().Contains(s) ||
                    (p.Area ?? "").ToLower().Contains(s) ||
                    (p.Email ?? "").ToLower().Contains(s) ||
                    (p.MobileNo ?? "").Contains(s));
            }

            var filtered = table.OrderBy(p => p.Category).ThenBy(p => p.FullName).ToList();

            // ---- paging ----
            int totalFiltered = filtered.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalFiltered / (double)pageSize));
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var pageItems = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var vm = new DashboardViewModel
            {
                TotalPersons = all.Count,
                TodayCount = todayList.Count,
                UpcomingCount = upcoming.Count,
                ThisMonthCount = all.Count(p => p.DateOfBirth.Month == today.Month),
                CategoryCount = all.Select(p => p.Category).Distinct().Count(),

                TodayBirthdays = todayList,
                UpcomingByCategory = upcomingByCategory,

                Persons = pageItems,
                Categories = PersonLists.Categories,
                SelectedCategory = category,
                Search = search,
                UpcomingDays = days,

                Page = page,
                PageSize = pageSize,
                TotalFiltered = totalFiltered,
                TotalPages = totalPages,
                FromRecord = totalFiltered == 0 ? 0 : (page - 1) * pageSize + 1,
                ToRecord = Math.Min(page * pageSize, totalFiltered)
            };

            return View(vm);
        }

        private static BirthdayItem BuildItem(PersonMaster p, DateTime today)
        {
            DateTime next = SafeDate(today.Year, p.DateOfBirth.Month, p.DateOfBirth.Day);
            if (next < today)
                next = SafeDate(today.Year + 1, p.DateOfBirth.Month, p.DateOfBirth.Day);

            return new BirthdayItem
            {
                Person = p,
                NextBirthday = next,
                DaysLeft = (next - today).Days,
                TurningAge = next.Year - p.DateOfBirth.Year
            };
        }

        private static DateTime SafeDate(int year, int month, int day)
        {
            if (month == 2 && day == 29 && !DateTime.IsLeapYear(year)) day = 28;
            return new DateTime(year, month, day);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}