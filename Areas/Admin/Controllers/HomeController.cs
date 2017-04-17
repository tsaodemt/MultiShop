using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public ActionResult GetDetaiPage(string id, int PageNo = 0)
        {
            var studentId = Guid.Parse(id);
            ViewBag.DetailStudent = db.StudentDetails.ToList().Where(i => i.StudentID == studentId)
                .Skip(PageSize * PageNo).Take(PageSize);
            return PartialView("_StudentDetail");
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
            //model.Gender = "M";
            model.NoteDate = DateTime.Now;

            db.Students.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AddDetail(string id)
        {
            var model = new StudentDetail();
            model.StudentID = Guid.Parse(id);
            model.Date = DateTime.Now;

            return View("AddDetail", model);
        }

        [ValidateInput(false)]
        public ActionResult AddDetailReponse(StudentDetail model)
        {
            var detail = new StudentDetail
            {
                StudentID = model.StudentID,
                Id = Guid.NewGuid(),
                Violate = model.Violate,
                Study = model.Study,
                Date = model.Date,
                Supervisor = model.Supervisor
            };

            db.StudentDetails.Add(detail);
            db.SaveChanges();

            return new JsonResult { Data = model.StudentID };
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

        public ActionResult DeleteDetail(string id)
        {
            try
            {
                var studentId = Guid.Parse(id);
                var model = db.StudentDetails.Find(studentId);
                db.StudentDetails.Remove(model);
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
            model.Blood = model.Blood.Trim();

            return View("Edit", model);
        }

        public ActionResult EditDetail(string id)
        {
            var studentId = Guid.Parse(id);
            var model = db.StudentDetails.Find(studentId);

            return View("EditDetail", model);
        }

        public ActionResult Detail(string id)
        {
            var studentId = Guid.Parse(id);
            var student = db.Students.Find(studentId);
            var model = new ListStudentDetail();
            model.Name = student.FullName;
            model.StudentID = studentId;
            model.Details = new List<StudentDetail>();

            ViewBag.PageCount = 10;

            return View("Detail", model);
        }

        //[ValidateInput(false)]
        public ActionResult Update(Student model)
        {
            try
            {
                //model.Gender = "M";
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