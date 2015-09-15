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

        public ImageShareDbContext imagesContext
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ImageShareDbContext>();
            }
            private set
            {
                imagesContext = value;
            }
        }

        public ApplicationDbContext usersContext
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }

            private set
            {
                usersContext = value;
            }
        }


        public ActionResult Index(int imagesForARow = -1)
        {
            LinkedList<Image> images = new LinkedList<Image>(imagesContext.Images.ToList());

            LinkedList<Category> categories = new LinkedList<Category>(imagesContext.Categories.ToList());
            LinkedList<CategoriesAndCounts> sidebarMenuData = new LinkedList<CategoriesAndCounts>();

            foreach (var cat in categories)
            {
                long catCount = imagesContext.Images.Where(i => i.CategoryID == cat.CategoryID).Count();
                sidebarMenuData.AddLast(new CategoriesAndCounts { category = cat, count = catCount });
            }


            int takeLastXImages = (int)(images.Count() * 0.6);
            LinkedList<Image> temp = new LinkedList<Image>();

            for (int i = images.Count() - 1; i >= images.Count() - takeLastXImages; --i)
            {
                temp.AddLast(images.ElementAt(i));
            }

            images = new LinkedList<Image>(temp);
            ViewBag.PageDescription = "Latest Images";


            if (imagesForARow == -1 && Session["byRow"] == null)
            {
                Session["byRow"] = 3;
            }

            if (imagesForARow != -1)
            {
                Session["byRow"] = imagesForARow;
            }

            int columns = (int)(Session["byRow"]);



            ViewBag.Categories = sidebarMenuData;
            ViewBag.ImagesGrid = new ImagesGridSystemLogic(images, imagesContext, usersContext, columns).create();
            ViewBag.columns = columns;

            return View();
        }

        public ActionResult ByCategory(long categoryid, int imagesForARow = -1)
        {

            LinkedList<Image> images = new LinkedList<Image>(imagesContext.Images.ToList());
            LinkedList<Category> categories = new LinkedList<Category>(imagesContext.Categories.ToList());
            LinkedList<CategoriesAndCounts> sidebarMenuData = new LinkedList<CategoriesAndCounts>();

            foreach (var cat in categories)
            {
                long catCount = imagesContext.Images.Where(i => i.CategoryID == cat.CategoryID).Count();
                sidebarMenuData.AddLast(new CategoriesAndCounts { category = cat, count = catCount });
            }

            if (imagesContext.Categories.Any(c => c.CategoryID == categoryid))
            {
                images = new LinkedList<Image>(imagesContext.Images.Where(i => i.CategoryID == categoryid).ToList());
                ViewBag.PageDescription = "Category: " + imagesContext.Categories.Where(c => c.CategoryID == categoryid).FirstOrDefault().Name;
            }
            else
            {
                return PartialView("NoCategory");
            }

            if (imagesForARow == -1 && Session["byRow"] == null)
            {
                Session["byRow"] = 3;
            }

            if (imagesForARow != -1)
            {
                Session["byRow"] = imagesForARow;
            }

            int columns = (int)(Session["byRow"]);

            ViewBag.Categories = sidebarMenuData;
            ViewBag.ImagesGrid = new ImagesGridSystemLogic(images, imagesContext, usersContext, columns).create();
            ViewBag.columns = columns;

            return View("Index");
        }

        public ActionResult TopFavourited(int imagesForARow = -1)
        {
            LinkedList<long> distinctFavouritedImages = new LinkedList<long>(imagesContext.ImageFavourites.Select(d => d.ImageID).Distinct().ToList());
            SortedList<long, long> topFavouritedImages = new SortedList<long, long>(new DuplicateKeyComparer<long>());

            foreach (long imageId in distinctFavouritedImages)
            {
                topFavouritedImages.Add(imagesContext.ImageFavourites.Count(t => t.ImageID == imageId), imageId);
            }

            LinkedList<Image> images = new LinkedList<Image>();

            foreach (KeyValuePair<long, long> pair in topFavouritedImages)
            {
                images.AddFirst(imagesContext.Images.Where(i => i.ImageID == pair.Value).FirstOrDefault());
            }

            LinkedList<Category> categories = new LinkedList<Category>(imagesContext.Categories.ToList());
            LinkedList<CategoriesAndCounts> sidebarMenuData = new LinkedList<CategoriesAndCounts>();

            foreach (var cat in categories)
            {
                long catCount = imagesContext.Images.Where(i => i.CategoryID == cat.CategoryID).Count();
                sidebarMenuData.AddLast(new CategoriesAndCounts { category = cat, count = catCount });
            }

            if (imagesForARow == -1 && Session["byRow"] == null)
            {
                Session["byRow"] = 3;
            }

            if (imagesForARow != -1)
            {
                Session["byRow"] = imagesForARow;
            }

            int columns = (int)(Session["byRow"]);

            ViewBag.Categories = sidebarMenuData;
            ViewBag.ImagesGrid = new ImagesGridSystemLogic(images, imagesContext, usersContext, columns).create();
            ViewBag.columns = columns;
            ViewBag.PageDescription = "Top favourited images";

            return View("Index");
        }

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            #region IComparer<TKey> Members

            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;   // Handle equality as beeing greater
                else
                    return result;
            }

            #endregion
        }

        public ActionResult Search()
        {
            return PartialView(new Search());
        }

        [HttpPost]
        public ActionResult Search(Search model)
        {
            LinkedList<Image> images = new LinkedList<Image>();

            LinkedList<Category> categories = new LinkedList<Category>(imagesContext.Categories.ToList());
            LinkedList<CategoriesAndCounts> sidebarMenuData = new LinkedList<CategoriesAndCounts>();

            foreach (var cat in categories)
            {
                long catCount = imagesContext.Images.Where(i => i.CategoryID == cat.CategoryID).Count();
                sidebarMenuData.AddLast(new CategoriesAndCounts { category = cat, count = catCount });
            }

            foreach(Image img in imagesContext.Images.ToList())
            {
                if(img.Title.ToLower().Contains(model.SearchWord.ToLower()))
                {
                    images.AddLast(img);
                }
            }

            if(images.Count == 0)
            {
                ViewBag.PageDescription = "No images found with title: " + model.SearchWord;
            }
            else
                ViewBag.PageDescription = "Search Images";


            if (Session["byRow"] == null)
            {
                Session["byRow"] = 3;
            }

            int columns = (int)(Session["byRow"]);



            ViewBag.Categories = sidebarMenuData;
            ViewBag.ImagesGrid = new ImagesGridSystemLogic(images, imagesContext, usersContext, columns).create();
            ViewBag.columns = columns;


            return View("Index");
        }

        [HttpGet]
        public ActionResult ShowImage(string imgUrl, string title)
        {
            ViewBag.ImageUrl = imgUrl;
            ViewBag.Title = title;
            return PartialView();
        }


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
            return (12 / col).ToString();
        }
    }

    public class CategoriesAndCounts
    {
        public Category category { get; set; }
        public long count { get; set; }
    }
}
