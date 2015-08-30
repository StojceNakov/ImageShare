using ImageShare.DataAccessLayer;
using ImageShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using IdentitySample.Models;

using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ImageShare.Controllers
{
    public class AlbumsController : Controller
    {
        //
        // GET: /CreateAlbum/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAlbum()
        {
            return View(new AlbumViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAlbum(AlbumViewModel model)
        {
            if (ModelState.IsValid)
            {

                ImageShareDbContext ImagesContext = HttpContext.GetOwinContext().Get<ImageShareDbContext>();

                Album album = new Album();
                album.Name = model.Name;
                album.ApplicationUserID = HttpContext.User.Identity.GetUserId();
                album.Type = AlbumType.Album;
                ImagesContext.Albums.Add(album);
                ImagesContext.SaveChanges();


                return RedirectToAction("AddImages", "Images");
            }

            return View(model);
        }
	}
}