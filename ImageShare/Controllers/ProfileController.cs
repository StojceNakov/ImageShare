using ImageShare.DataAccessLayer;
using ImageShare.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using IdentitySample.Models;
using ImageShare.ImagesGridSystem;

namespace ImageShare.Controllers
{
    public class ProfileController : Controller
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
        // GET: /Profile/
        public ActionResult Index(string userId = "", long albumId = -1, int imagesForARow = -1)
        {
            string profileUserId = userId;

            ApplicationDbContext usersDbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            if (String.IsNullOrEmpty(userId) || !usersDbContext.Users.Any(u => u.Id == userId))
            {
                return View("NoUserError");
            }

            ViewBag.UserId = profileUserId;

            List<Album> albums = ImagesContext.Albums.Where(a => a.ApplicationUserID == profileUserId).ToList();
            LinkedList<Image> images = new LinkedList<Image>();
            Album album = null;
            long AllPhotosCount = 0;

            if (albumId > 0)
            {
                album = findAlbumById(albumId, albums);
                ViewBag.PageDescription = album.Name;
            }
            else if (albumId == -1)
            {
                album = findNoAlbum(albums);
                ViewBag.PageDescription = album.Name;
            }
            else
                ViewBag.PageDescription = "All Photos";
                
                

            foreach (var al in albums)
            {
                if (albumId == 0)
                {
                    foreach (var img in ImagesContext.Images.Where(i => i.AlbumID == al.AlbumID).ToList())
                    {
                        images.AddLast(img);
                    }

                    AllPhotosCount = images.Count;
                }
                else
                {
                    AllPhotosCount += ImagesContext.Images.Where(i => i.AlbumID == al.AlbumID).Count();
                }

            }

            if (album != null)
            {
                images = new LinkedList<Image>(ImagesContext.Images.Where(i => i.AlbumID == album.AlbumID).ToList());
            }

            int columns;

            if (imagesForARow != -1)
            {
                Session["byRow"] = columns = imagesForARow;
            }
            else if (Session["byRow"] == null)
            {
                Session["byRow"] = columns = 3;
            }
            else
            {
                columns = (int)(Session["byRow"]);
            }
                

            ViewBag.Albums = albums;
            ViewBag.AlbumsCount = albums.Count;
            ViewBag.AllPhotosCount = AllPhotosCount;
            ViewBag.ImagesGrid = new ImagesGridSystemLogic(images, ImagesContext, usersDbContext, columns).create();
            ViewBag.columns = columns;
            ViewBag.UserId = profileUserId;

            return View();
        }

        private Album findAlbumById(long id, List<Album> albums)
        {
            foreach (Album a in albums)
            {
                if (a.AlbumID == id)
                {
                    return a;
                }
            }

            return null;
        }

        private Album findNoAlbum(List<Album> albums)
        {
            foreach (Album a in albums)
            {
                if (a.Type == AlbumType.NoAlbum)
                {
                    return a;
                }
            }

            return null;
        }
	}
}