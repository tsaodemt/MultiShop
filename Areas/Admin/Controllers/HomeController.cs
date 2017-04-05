using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private int PageSize = 10;
        private MultiShopDbContext db = new MultiShopDbContext();

        public ActionResult GetPage(int PageNo = 0)
        {
            ViewBag.Students = db.Students.ToList()
                .Skip(PageSize * PageNo).Take(PageSize);
            return PartialView("_Student");
        }

        public ActionResult Index()
        {
            ViewBag.PageCount = Math.Ceiling(1.0 * db.Students.ToList().Count / PageSize);
            ViewBag.Students = db.Students;

            return View();
        }

        //[ValidateInput(false)]
        public ActionResult Add(Student model)
        {
            model.StudentID = Guid.NewGuid();
            model.Gender = "M";
            model.NoteDate = DateTime.Now;

            db.Students.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AddDetail(string id)
        {
            var model = new StudentDetail();

            return View("AddDetail", model);
        }

        public ActionResult Delete(string id)
        {
            try
            {
                var studentId = Guid.Parse(id);
                var model = db.Students.Find(studentId);
                db.Students.Remove(model);
                db.SaveChanges();
                //ModelState.AddModelError("", "Deleted");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var studentId = Guid.Parse(id);
            var model = db.Students.Find(studentId);

            return View("Edit", model);
        }

        public ActionResult EditDetail(string id)
        {
            var studentId = Guid.Parse(id);
            var student = db.Students.Find(studentId);
            var model = new ListStudentDetail();
            model.Name = student.FullName;
            model.StudentID = studentId;
            model.Details = new List<StudentDetail>();

            return View("Edit", model);
        }

        //[ValidateInput(false)]
        public ActionResult Update(Student model)
        {
            try
            {
                model.Gender = "M";
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                //ModelState.AddModelError("", "Error");
            }

            return RedirectToAction("Index");
        }
    }
}