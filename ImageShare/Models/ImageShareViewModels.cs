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
        public string Title { get; set; }

        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IEnumerable<HttpPostedFileBase> FilesUpload { get; set; }

        public int selectedAlbum { get; set; }
        public IEnumerable<SelectListItem> AlbumsDropDown { get; set; }
    }

    public class AlbumViewModel
    {
        public string Name { get; set; }

        public int Type { get; set; }

        public string ApplicationUserID { get; set; }
    }
}