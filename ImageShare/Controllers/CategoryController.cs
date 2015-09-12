using ImageShare.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ImageShare.Models;

namespace ImageShare.Controllers
{
    public class CategoryController : Controller
    {
        public ImageShareDbContext ImagesContext
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ImageShareDbContext>();
            }
            private set
            {
                ImagesContext = value;
            }
        }

        //
        // GET: /Category/
        public ActionResult Index()
        {
            return View(ImagesContext.Categories.ToList());
        }

        //
        // GET: /Category/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Category/Create
        public ActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        //
        // POST: /Category/Create
        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            try
            {
                Category newCategory = new Category();
                newCategory.Name = model.Name;

                ImagesContext.Categories.Add(newCategory);
                ImagesContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Category/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Category cat = ImagesContext.Categories.Where(c => c.CategoryID == id).First();
            CategoryViewModel category = new CategoryViewModel();

            category.Name = cat.Name;
            category.OldName = cat.Name;
          
            return PartialView(category);
        }

        //
        // POST: /Category/Edit/5
        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            Category cat = ImagesContext.Categories.Where(c => c.Name == model.OldName).First();

            ImagesContext.Categories.Remove(cat);
            ImagesContext.SaveChanges();

            cat.Name = model.Name;
            ImagesContext.Categories.Add(cat);
            ImagesContext.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Category/Delete/5
        public ActionResult Delete(int id)
        {
            if(ImagesContext.Categories.Any(c => c.CategoryID == id))
            {
                long uncategorizedId = ImagesContext.Categories.Where(c => c.Name == "Uncategorized").First().CategoryID;

                foreach(Image i in ImagesContext.Images.Where(i => i.CategoryID == id).ToList())
                {
                    ImagesContext.Images.Remove(i);

                    i.CategoryID = uncategorizedId;
                    ImagesContext.Images.Add(i);
                    ImagesContext.SaveChanges();
                }

                ImagesContext.Categories.Remove(ImagesContext.Categories.Where(c => c.CategoryID == id).First());
                ImagesContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //
        // POST: /Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateNewCategoryModal()
        {
            return PartialView(new CategoryViewModel());
        }
    }
}
