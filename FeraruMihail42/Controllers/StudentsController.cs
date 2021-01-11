using FeraruMihail42.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FeraruMihail42.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        public ActionResult Search(string email, string domeniu)
        {
            if (email != null && email != "") ViewBag.emailK = email;
            if (domeniu != null && domeniu != "") ViewBag.domeniuK = domeniu;

            if (email != "" && domeniu != "")
            {
                var students = db.Students.Where(s => s.Domain.Denumire.Contains(domeniu) &&
                                    s.Email.Contains(email)).ToList();
                return View(students);
            }
            else if (email != "")
            {
                var students = db.Students.Where(s => s.Email.Contains(email)).ToList();
                return View(students);
            }
            else if (domeniu != "")
            {
                var students = db.Students.Where(s => s.Domain.Denumire.Contains(domeniu)).ToList();
                return View(students);
            }

            return View(db.Students.ToList());
        }

        [ActionName("AfisareStudent")]
        public ActionResult Show(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = db.Students.Find(id);
            if (student == null) return HttpNotFound();

            return View(student);
        }

        public ActionResult New()
        {
            ViewBag.DomainId = new SelectList(db.Domains, "Id", "Denumire");
            return View();
        }

        private bool IsValid(Student student)
        {
            if (student.DataNastere > DateTime.Now)
            {
                ModelState.AddModelError("DataNastere", "Data nu poate fi din viitor.");
                return false;
            }

            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Now - student.DataNastere;
            int yearsDiff = (zeroTime + span).Year - 1;

            if (yearsDiff < 18)
            {
                ModelState.AddModelError("DataNastere", "Varsta minima este 18 ani.");
                return false;
            }

            return true;
        }

        [HttpPost]
        public ActionResult New([Bind(Include = "Id,Nume,Email,DataNastere,DomainId")] Student student)
        {
            if (ModelState.IsValid && IsValid(student))
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DomainId = new SelectList(db.Domains, "Id", "Denumire", student.DomainId);
            return View(student);
        }

        // BUG: In edit e un mic bug la afisarea datei
        // O converteste intr-un format invalid cand o pune in textbox 
        // Asa ca trebuie reintrodusa de mana, n-am mai apucat sa rezolv :(
        // UPDATE: l-am rezolvat, cred :P
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Student student = db.Students.Find(id);
            if (student == null) return HttpNotFound();

            ViewBag.DomainId = new SelectList(db.Domains, "Id", "Denumire", student.DomainId);
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Nume,Email,DataNastere,DomainId")] Student student)
        {
            if (ModelState.IsValid && IsValid(student))
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.SuccMsg = "Studentul a fost editat cu success!";
                ViewBag.DomainId = new SelectList(db.Domains, "Id", "Denumire", student.DomainId);
                return View(student);
            }
            ViewBag.DomainId = new SelectList(db.Domains, "Id", "Denumire", student.DomainId);
            ViewBag.SuccMsg = "Nu s-a putut actualiza!";
            return View(student);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}