using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiShop.Controllers
{
    public class HomeController : Controller
    {
        private MultiShopDbContext db = new MultiShopDbContext();

        public ActionResult Index()
        {
            var model = db.Products.ToList().Take(16);
            ViewData["Home"] = true;
            return View(model);
        }

        public ActionResult Product()
        {
            var model = db.Products.ToList();
            ViewData["Product"] = true;
            return View("~/Views/Product/Index.cshtml", model);
        }

        public ActionResult Introduce()
        {
            ViewData["Introduce"] = true;
            return View("~/Views/Introduciton/Index.cshtml");
        }

        public ActionResult Info()
        {
            ViewData["Info"] = true;
            return View("~/Views/Info/Index.cshtml");
        }

        public ActionResult Contact()
        {
            ViewData["Contact"] = true;
            return View("~/Views/Contact/Index.cshtml");
        }

        public ActionResult Service()
        {
            ViewData["Service"] = true;
            return View("~/Views/Service/Index.cshtml");
        }

        public ActionResult Search()
        {
            var name = Request["term"];

            var data = db.Products
                .Where(p => p.Name.Contains(name))
                .Select(p => p.Name).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Category()
        {
            var model = new List<CategoryViewModel>();
            var parentCatelogies = db.Categories.Where(i => i.ParentId == 0).ToList();
            var allCategories = db.Categories.ToList();
            foreach (var parent in parentCatelogies)
            {
                model.Add(new CategoryViewModel
                {
                    Id = parent.Id,
                    DisplayOrder = parent.DisplayOrder,
                    Name = parent.Name,
                    NameVN = parent.NameVN
                });
                foreach (var item in allCategories)
                {
                    if (item.ParentId != parent.Id) continue;
                    var parent1 = parent;
                    var categoryViewModel = model.Where(i => i.Id == parent1.Id).ToList().FirstOrDefault();
                    categoryViewModel?.SubCategory.Add(item);
                }
            }

            return PartialView("_Category", model);
        }

        public ActionResult Special()
        {
            var model = db.Products.Where(p => p.Special == true).Take(5);
            return PartialView("_Special", model);
        }

        //Download source code tại Sharecode.vn
        public ActionResult Saleoff()
        {
            var model = db.Products.Where(p => p.Discount > 0).Take(1);
            return PartialView("_Saleoff", model);
        }
    }
}