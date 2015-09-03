using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageShare.ImagesGridSystem
{
    public class ContainerForImagesAndUsers
    {
        public ContainerForImagesAndUsers()
        {
            imagesAndUsers = new LinkedList<ImagesAndUsers>();
        }

        public LinkedList<ImagesAndUsers> imagesAndUsers { get; set; }
    }
}