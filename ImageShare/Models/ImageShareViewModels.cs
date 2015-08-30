using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageShare.Models
{
    public class ImageViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Choose image(s)")]
        [Required(ErrorMessage = "You must select at least one picture!")]
        [DataType(DataType.Upload)]
        public IEnumerable<HttpPostedFileBase> FilesUpload { get; set; }

        [Required]
        [Display(Name = "Albums")]
        public long selectedAlbum { get; set; }
        [Display(Name = "Albums")]
        public IEnumerable<SelectListItem> AlbumsDropDown { get; set; }

        [Display(Name = "Categories")]
        [Required]
        public long selectedCategory { get; set; }
        [Display(Name = "Categories")]
        public IEnumerable<SelectListItem> CategoriesDropDown { get; set; }
    }

    public class AlbumViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }

        public int Type { get; set; }

        public string ApplicationUserID { get; set; }
    }

    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Name field is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}