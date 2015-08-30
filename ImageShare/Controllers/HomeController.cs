using IdentitySample.Models;
using System.Web.Mvc;

using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ImageShare.DataAccessLayer;
using System.Collections.Generic;
using ImageShare.Models;


namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(long categoryId = -1)
        {
            ImageShareDbContext imagesContext = HttpContext.GetOwinContext().Get<ImageShareDbContext>();
            LinkedList<Image> images = new LinkedList<Image>(imagesContext.Images.ToList());

            LinkedList<Category> categories = new LinkedList<Category>(imagesContext.Categories.ToList());
            LinkedList<CategoriesAndCounts> sidebarMenuData = new LinkedList<CategoriesAndCounts>();

            foreach(var cat in categories)
            {
                long catCount = imagesContext.Images.Where(i => i.CategoryID == cat.CategoryID).Count();
                sidebarMenuData.AddLast(new CategoriesAndCounts { category = cat, count = catCount });
            }

            if(categoryId != -1)
            {
                images = new LinkedList<Image>(imagesContext.Images.Where(i => i.CategoryID == categoryId).ToList());
                ViewBag.Images = images;
                ViewBag.PageDescription = "Category: " + imagesContext.Categories.Where(c => c.CategoryID == categoryId).FirstOrDefault().Name;
            }
            else
            {
                ViewBag.Images = images.Take(images.Count() - 5);
                ViewBag.PageDescription = "Latest Images";
            }

            
            ViewBag.Categories = sidebarMenuData;

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class CategoriesAndCounts
    {
        public Category category { get; set; }
        public long count { get; set; }
    }
}
