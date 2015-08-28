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


namespace ImageShare.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private static List<Album> dropDownAlbums;

        //property for Images and Albums databases context
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
        // GET: /ImageShare/
        public ActionResult Index(long albumId = -1)
        {
            string currentUserId = HttpContext.User.Identity.GetUserId();
            List<Album> albums = ImagesContext.Albums.Where(a => a.ApplicationUserID == currentUserId).ToList();
            List<Image> images = new List<Image>();

            Album album = null;

            foreach(Album a in albums)
            {
                if(albumId == -1)
                {
                    album = findNoAlbum(albums);
                }
                else
                {
                    album = findAlbumById(albumId, albums);
                }
                    
            }

            if(album != null)
            {
                images = ImagesContext.Images.Where(i => i.AlbumID == album.AlbumID).ToList();
            }

            ViewBag.Images = images;
            ViewBag.Albums = albums;

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

        public ActionResult AddImages()
        {
            ImageViewModel imagesView = new ImageViewModel();
            string currentUserId = HttpContext.User.Identity.GetUserId();

            var albums = ImagesContext.Albums.Where(x => x.ApplicationUserID == currentUserId && x.Type != AlbumType.NoAlbum);

            List<Album> al = new List<Album>();

            Album defaultAlbum = new Album();
            defaultAlbum.AlbumID = -1;
            defaultAlbum.Name = "No Album";

            al.Add(defaultAlbum);
            foreach(Album a in albums)
            {
                al.Add(a);
            }

            dropDownAlbums = new List<Album>(al);

            SelectList selectlist = new SelectList(al.AsEnumerable(), "AlbumID", "Name");
            imagesView.AlbumsDropDown = selectlist;
            imagesView.selectedAlbum = 1;
            
            return View(imagesView);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImages(ImageViewModel model)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (model.FilesUpload == null || model.FilesUpload.Count() == 0)
            {
                ModelState.AddModelError("FilesUpload", "This field is required");
            }

            if (ModelState.IsValid)
            {

                Album selectedAlbum = dropDownAlbums.Find(a => a.AlbumID == model.selectedAlbum);
                bool error = false;

                //System.Diagnostics.Debug.WriteLine("Image URL " + "asd");

                foreach (var imageToUpload in model.FilesUpload)
                {
                    if(!validImageTypes.Contains(imageToUpload.ContentType))
                    {
                        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                        error = true;
                        break;
                    }

                    if (imageToUpload != null && imageToUpload.ContentLength > 0)
                    {
                        Image image = new Image();
                        image.AltText = model.AltText;
                        image.Caption = model.Caption;
                        image.Title = imageToUpload.FileName;

                        string uploadDir = AppDomain.CurrentDomain.BaseDirectory + "UploadedImages\\";
                        //var imagePath = Path.Combine(Server.MapPath(uploadDir), imageToUpload.FileName);
                        //var imageUrl = Path.Combine(uploadDir, imageToUpload.FileName);
                        string filename = Path.GetFileName(imageToUpload.FileName);
                        imageToUpload.SaveAs(Path.Combine(uploadDir, filename));
                        image.ImageUrl = Path.Combine(uploadDir, filename);

                        if (selectedAlbum.AlbumID != -1)
                        {
                            image.AlbumID = selectedAlbum.AlbumID;
                        }
                        else
                        {
                            image.AlbumID = this.initializeDefaultAlbum();
                        }

                        ImagesContext.Images.Add(image);
                        ImagesContext.SaveChanges();
                    }
                    else
                    {
                        error = true;
                        break;
                    }
                }
                
                return RedirectToAction("AddImages", "Images");
            }

            return RedirectToAction("AddImages", "Images");
        }

        private long initializeDefaultAlbum()
        {
            string currentUserId = HttpContext.User.Identity.GetUserId();
            Album al = ImagesContext.Albums.Where(a => a.ApplicationUserID == currentUserId && a.Type == AlbumType.NoAlbum).FirstOrDefault();

            if (al == null)
            {
                Album createDefaultAlbum = new Album();
                createDefaultAlbum.ApplicationUserID = currentUserId;
                createDefaultAlbum.Name = "No Album";
                createDefaultAlbum.Type = AlbumType.NoAlbum;

                ImagesContext.Albums.Add(createDefaultAlbum);
                ImagesContext.SaveChanges();
                Album added = ImagesContext.Albums.Where(a => a.ApplicationUserID == currentUserId && a.Type == AlbumType.NoAlbum).FirstOrDefault();

                return added.AlbumID;
            }
            else
            {
                return al.AlbumID;
            }
        }

        public FileResult RenderImage(string filePath)
        {
            return File(filePath, "image/jpg");
           
        }

    }
}