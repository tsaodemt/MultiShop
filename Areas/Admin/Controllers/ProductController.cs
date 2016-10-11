using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private int PageSize = 10;
        private MultiShopDbContext db = new MultiShopDbContext();

        public ActionResult GetPage(int PageNo = 0)
        {
            ViewBag.Products = db.Products.ToList()
                .Skip(PageSize * PageNo).Take(PageSize);
            return PartialView("_Product");
        }

        public ActionResult Index()
        {
            ViewBag.PageCount = Math.Ceiling(1.0 * db.Products.ToList().Count / PageSize);
            ViewBag.Products = db.Products;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");

            return View();
        }

        [ValidateInput(false)]
        public ActionResult Insert(Product model)
        {
            try
            {
                var f = Request.Files["uplLogo"];
                model.ProductDate = DateTime.Now;
                if (f != null && f.ContentLength > 0)
                {
                    model.Image = model.Id
                        + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    f.SaveAs(Server.MapPath("/Content/img/products/" + model.Image));
                }
                db.Products.Add(model);
                db.SaveChanges();
                ModelState.AddModelError("", "Inserted");
            }
            catch (Exception e)
            {
                var er = e.Message;
                throw;
                ModelState.AddModelError("", "Error");
            }

            ViewBag.Products = db.Products;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", model.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", model.SupplierId);

            return View("Index", model);
        }

        [ValidateInput(false)]
        public ActionResult Update(Product model)
        {
            try
            {
                model.ProductDate = DateTime.Now;
                var f = Request.Files["uplLogo"];
                if (f != null && f.ContentLength > 0)
                {
                    model.Image = model.Id
                        + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    f.SaveAs(Server.MapPath("/Content/img/products/" + model.Image));
                }
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.AddModelError("", "Updated");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }

            ViewBag.Products = db.Products;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", model.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", model.SupplierId);

            return View("Index", model);
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                var model = db.Products.Find(Id);
                db.Products.Remove(model);
                db.SaveChanges();
                ModelState.AddModelError("", "Deleted");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            var model = db.Products.Find(Id);

            ViewBag.Products = db.Products;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", model.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", model.SupplierId);

            return View("Edit", model);
        }

        public void Upload()
        {
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/Content/img/Uploads/"), fileName);
                    file.SaveAs(path);
                }
            }
        }
    }
}