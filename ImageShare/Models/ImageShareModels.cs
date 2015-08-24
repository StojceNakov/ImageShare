using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using IdentitySample.Models;
using System.ComponentModel.DataAnnotations;


namespace ImageShare.Models
{

    public static class AlbumType
    {
        public const int NoAlbum = 0;

        public const int Album = 1;
    }

    public class Album
    {
        public long AlbumID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string ApplicationUserID { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }

    public class Image
    {
        public long ImageID { get; set; }

        [Required]
        public string Title { get; set; }

        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        private DateTime? createdDate;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }

        public long AlbumID { get; set; }

        public virtual Album ImageAlbum { get; set; }

    }
}