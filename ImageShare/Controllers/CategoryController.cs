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
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Category/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
    }
}
