using IdentitySample.Models;
using ImageShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageShare.ImagesGridSystem
{
    public class ImagesAndUsers
    {
        public Image image { get; set; }

        public ApplicationUser user { get; set; }
    }
}