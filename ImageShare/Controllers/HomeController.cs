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
using ImageShare.ImagesGridSystem;


namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(long categoryId = -1, int imagesForARow = -1)
        {
            ImageShareDbContext imagesContext = HttpContext.GetOwinContext().Get<ImageShareDbContext>();
            ApplicationDbContext usersContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
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
                ViewBag.PageDescription = "Category: " + imagesContext.Categories.Where(c => c.CategoryID == categoryId).FirstOrDefault().Name;
            }
            else
            {

                int takeLastXImages = (int)(images.Count() * 0.3);
                LinkedList<Image> temp = new LinkedList<Image>();

                for (int i = images.Count() - 1; i >= images.Count() - takeLastXImages; --i)
                {
                    temp.AddLast(images.ElementAt(i));
                }

                images = new LinkedList<Image>(temp);
                ViewBag.PageDescription = "Latest Images";
            }


            if (imagesForARow == -1 && Session["byRow"] == null)
            {
                Session["byRow"] = 3;
            }

            if(imagesForARow != -1)
            {
                Session["byRow"] = imagesForARow;
            }

            int columns = (int)(Session["byRow"]);

            

            ViewBag.Categories = sidebarMenuData;
            ViewBag.ImagesGrid = new ImagesGridSystemLogic(images, imagesContext, usersContext, columns).create();
            ViewBag.columns = columns;

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

        public string getColumns(int col)
        {
            return (12/col).ToString();
        }
    }

    public class CategoriesAndCounts
    {
        public Category category { get; set; }
        public long count { get; set; }
    }
}
