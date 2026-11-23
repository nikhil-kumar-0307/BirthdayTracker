using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BirthdayTracker.Data;
using BirthdayTracker.Models.DTOs;

namespace BirthdayTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginDto { LoginAs = "Employee" });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(model.LoginAs))
            {
                model.LoginAs = "Employee";
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string empNo = model.EmployeeNumber.Trim();

            var user = _db.Users.FirstOrDefault(u =>
                u.EmployeeNumber == empNo && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Employee Number or Password");
                return View(model);
            }

            bool isAdminAccount = user.Role == "Admin";
            bool adminTabSelected = model.LoginAs == "Admin";

            if (isAdminAccount != adminTabSelected)
            {
                ModelState.AddModelError("", adminTabSelected
                    ? "This account is not an Admin. Please use the Employee tab."
                    : "This is an Admin account. Please use the Admin tab.");
                return View(model);
            }

            user.LastLoginAt = DateTime.Now;
            _db.SaveChanges();

            // Put the role inside the login cookie (survives restarts)
            DateTime expires = model.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(60);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.EmployeeNumber,
                DateTime.Now,
                expires,
                model.RememberMe,
                user.Role,                       // userData = role
                FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true
            };
            if (model.RememberMe) cookie.Expires = expires;

            Response.Cookies.Add(cookie);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}