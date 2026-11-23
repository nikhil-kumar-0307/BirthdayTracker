using System;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using BirthdayTracker.Data;
using BirthdayTracker.Models;

namespace BirthdayTracker.Controllers
{
    [Authorize]
    public class PersonMasterController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        private ActionResult NotAllowed()
        {
            TempData["Error"] = "Only an Admin can add, edit or delete records.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!IsAdmin()) return NotAllowed();
            return View(new PersonMaster { DateOfBirth = new DateTime(1970, 1, 1) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonMaster model)
        {
            if (!IsAdmin()) return NotAllowed();

            ValidateDob(model);
            if (!ModelState.IsValid) return View(model);

            model.IsActive = true;
            _db.PersonMasters.Add(model);
            _db.SaveChanges();

            TempData["Success"] = model.DisplayName + " was added successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!IsAdmin()) return NotAllowed();
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var person = _db.PersonMasters.Find(id);
            if (person == null) return HttpNotFound();

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonMaster model)
        {
            if (!IsAdmin()) return NotAllowed();

            ValidateDob(model);
            if (!ModelState.IsValid) return View(model);

            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();

            TempData["Success"] = model.DisplayName + " was updated successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!IsAdmin()) return NotAllowed();

            var person = _db.PersonMasters.Find(id);
            if (person != null)
            {
                string name = person.DisplayName;
                _db.PersonMasters.Remove(person);
                _db.SaveChanges();
                TempData["Success"] = name + " was deleted.";
            }

            return RedirectToAction("Index", "Home");
        }

        private void ValidateDob(PersonMaster model)
        {
            if (model.DateOfBirth.Year < 1900 || model.DateOfBirth.Date > DateTime.Today)
            {
                ModelState.AddModelError("DateOfBirth", "Enter a valid date of birth");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}