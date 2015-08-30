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
            long AllPhotosCount = 0;

            if (albumId > 0)
                album = findAlbumById(albumId, albums);

            if (albumId == -1)
                album = findNoAlbum(albums);

            foreach (var al in albums)
            {
                if (albumId == 0)
                {
                    images.AddRange(ImagesContext.Images.Where(i => i.AlbumID == al.AlbumID));
                    AllPhotosCount = images.Count;
                }
                else
                {
                    AllPhotosCount += ImagesContext.Images.Where(i => i.AlbumID == al.AlbumID).Count();
                }

            }

            if (album != null)
            {
                images = ImagesContext.Images.Where(i => i.AlbumID == album.AlbumID).ToList();
            }

            ViewBag.Images = images;
            ViewBag.Albums = albums;
            ViewBag.AlbumsCount = albums.Count;
            ViewBag.AllPhotosCount = AllPhotosCount;

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

            var albums = ImagesContext.Albums.Where(x => x.ApplicationUserID == currentUserId);

            if (albums.Count() == 0)
            {
                initializeDefaultAlbum();
                albums = ImagesContext.Albums.Where(x => x.ApplicationUserID == currentUserId);
            }

            var categories = ImagesContext.Categories.ToList();

            SelectList selectlist = new SelectList(albums.AsEnumerable(), "AlbumID", "Name");
            imagesView.AlbumsDropDown = selectlist;
            imagesView.selectedAlbum = 1;

            SelectList selectCategories = new SelectList(categories.AsEnumerable(), "CategoryID", "Name");
            imagesView.CategoriesDropDown = selectCategories;
            imagesView.selectedCategory = 1;

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
                ModelState.AddModelError("Upload image(s)", "This field is required");
            }

            if (ModelState.IsValid)
            {
                bool error = false;
                LinkedList<string> imageTitles = new LinkedList<string>();

                if(!String.IsNullOrEmpty(model.Title))
                    imageTitles = new LinkedList<string>(model.Title.Split(',').ToList());


                //System.Diagnostics.Debug.WriteLine("Image URL " + "asd");

                int titleIndex = 0;

                foreach (var imageToUpload in model.FilesUpload)
                {
                    if (imageToUpload == null)
                        break;

                    if (!validImageTypes.Contains(imageToUpload.ContentType))
                    {
                        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                        error = true;
                        break;
                    }

                    if (imageToUpload != null && imageToUpload.ContentLength > 0)
                    {
                        Image image = new Image();

                        if (titleIndex < imageTitles.Count)
                            image.Title = imageTitles.ElementAt(titleIndex);
                        else
                            image.Title = imageToUpload.FileName;

                        ++titleIndex;

                        string uploadDir = AppDomain.CurrentDomain.BaseDirectory + "UploadedImages\\";
                        //var imagePath = Path.Combine(Server.MapPath(uploadDir), imageToUpload.FileName);
                        //var imageUrl = Path.Combine(uploadDir, imageToUpload.FileName);
                        string filename = Path.GetFileName(imageToUpload.FileName);
                        imageToUpload.SaveAs(Path.Combine(uploadDir, filename));
                        image.ImageUrl = Path.Combine(uploadDir, filename);
                        image.AlbumID = model.selectedAlbum;
                        image.CategoryID = model.selectedCategory;

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

        private void initializeDefaultAlbum()
        {
            string currentUserId = HttpContext.User.Identity.GetUserId();

            Album createDefaultAlbum = new Album();
            createDefaultAlbum.ApplicationUserID = currentUserId;
            createDefaultAlbum.Name = "No Album";
            createDefaultAlbum.Type = AlbumType.NoAlbum;

            ImagesContext.Albums.Add(createDefaultAlbum);
            ImagesContext.SaveChanges();
        }

        [AllowAnonymous]
        public FileResult RenderImage(string filePath)
        {
            return File(filePath, "image/jpg");

        }

        [HttpGet]
        public ActionResult CreateNewAlbum()
        {
            return PartialView(new AlbumViewModel());
        }

    }
}