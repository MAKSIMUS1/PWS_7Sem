using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WSKMOModel;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        WSKMOEntities service = new WSKMOEntities(new Uri("http://localhost:15015/Service1.svc/"));

        public ActionResult Index()
        {
            var students = service.student.Expand("Note").ToList();
            return View(students);
        }

        [HttpPost]
        public ActionResult AddStudent(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var student = new student { name = name };
                service.AddTostudent(student);
                service.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            var student = service.student.Where(s => s.id == id).SingleOrDefault();
            if (student != null)
            {
                var notes = service.note.Where(n => n.stud_id == id).ToList();
                foreach (var note in notes)
                {
                    service.DeleteObject(note);
                }

                service.DeleteObject(student);
                service.SaveChanges();
            }
            else
            {
                ViewBag.ErrorMessage = "Студент не найден.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateStudent(int id, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                student student = service.student.Where(s => s.id == id).SingleOrDefault();
                if (student != null)
                {
                    student.name = name;
                    service.UpdateObject(student);
                    service.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddNote(int studentId, string subject, int noteValue)
        {
            if (!string.IsNullOrEmpty(subject) && noteValue >= 1 && noteValue <= 5)
            {
                var note = new note
                {
                    stud_id = studentId,
                    subject = subject,
                    note1 = noteValue
                };
                service.AddTonote(note);
                service.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteNote(int noteId)
        {
            var note = service.note.Where(n => n.id == noteId).SingleOrDefault();
            if (note != null)
            {
                service.DeleteObject(note);
                service.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateNote(int noteId, string subject, int noteValue)
        {
            if (!string.IsNullOrEmpty(subject) && noteValue >= 1 && noteValue <= 5)
            {
                note note = service.note.Where(n => n.id == noteId).SingleOrDefault();
                if (note != null)
                {
                    note.subject = subject;
                    note.note1 = noteValue;
                    service.UpdateObject(note);
                    service.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}