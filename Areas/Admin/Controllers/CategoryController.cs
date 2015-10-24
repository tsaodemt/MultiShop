using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiShop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        MultiShopDbContext db = new MultiShopDbContext();

        public ActionResult Index()
        {
            ViewBag.Category = db.Categories;
            return View();
        }

        public ActionResult Insert(Category model)
        {
            try
            {
                //var f = Request.Files["uplLogo"];
                //if (f != null && f.ContentLength > 0)
                //{
                //    model.Logo = model.Id
                //        + f.FileName.Substring(f.FileName.LastIndexOf("."));
                //    f.SaveAs(Server.MapPath("~/images/suppliers/" + model.Logo));
                     
                //}

                model.Name = model.NameVN;
                db.Categories.Add(model);
                db.SaveChanges();
                ModelState.AddModelError("", "Inserted");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }

            ViewBag.Category = db.Categories;
            return View("Index", model);
        }

        
        public ActionResult Delete(int Id)
        {
            try
            {
                var model = db.Categories.Find(Id);
                db.Categories.Remove(model);
                db.SaveChanges();
                ModelState.AddModelError("", "Deleted");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }

            ViewBag.Category = db.Categories;
            return View("Index");
        }

        public ActionResult Edit(String Id)
        {
            var model = db.Categories.Find(Id);

            ViewBag.Category = db.Categories;
            return View("Index", model);
        }

        public ActionResult Upload()
        {
            var f = Request.Files["uplLogo"];
            f.SaveAs(Server.MapPath("/Content/img/photos/" + f.FileName));
            return Content(f.FileName);
        }
    }
}