using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ImageShare.DataAccessLayer;
using ImageShare.Models;
using IdentitySample.Models;

namespace ImageShare.ImagesGridSystem
{
    public class ImagesGridSystemLogic
    {
        public ImagesGridSystemLogic(LinkedList<Image> images, ImageShareDbContext cntx1, ApplicationDbContext cntx2, int columns)
        {
            ImageShareDatabase = cntx1;
            UsersDatabase = cntx2;
            Columns = columns;
            Images = new LinkedList<Image>(images);
        }

        private LinkedList<Image> Images;

        public ImageShareDbContext ImageShareDatabase { get; set; }

        public ApplicationDbContext UsersDatabase { get; set; }

        public int Columns { get; set; }

        public LinkedList<ContainerForImagesAndUsers> create()
        {
            int imgIndex = 0;
            LinkedList<ContainerForImagesAndUsers> imagesGrid = new LinkedList<ContainerForImagesAndUsers>();

            for (int i = 0; i < (Images.Count / Columns); ++i)
            {
                ContainerForImagesAndUsers tempContainer = new ContainerForImagesAndUsers();

                for (int j = 0; j < Columns; ++j)
                {
                    Image img = new Image();
                    img = Images.ElementAt(imgIndex);

                    long albumid = img.AlbumID;
                    string userid = ImageShareDatabase.Albums.Where(a => a.AlbumID == albumid).FirstOrDefault().ApplicationUserID;

                    ApplicationUser user = UsersDatabase.Users.Where(u => u.Id == userid).FirstOrDefault();

                    tempContainer.imagesAndUsers.AddLast(new ImagesAndUsers { image = img, user = user });

                    ++imgIndex;
                }

                imagesGrid.AddLast(tempContainer);
            }

            ContainerForImagesAndUsers data = new ContainerForImagesAndUsers();

            for (int i = imgIndex; i < Images.Count; ++i)
            {
                Image img = new Image();
                img = Images.ElementAt(i);

                long albumid = img.AlbumID;
                string userid = ImageShareDatabase.Albums.Where(a => a.AlbumID == albumid).FirstOrDefault().ApplicationUserID;

                ApplicationUser user = UsersDatabase.Users.Where(u => u.Id == userid).FirstOrDefault();

                data.imagesAndUsers.AddLast(new ImagesAndUsers { image = img, user = user });
            }

            imagesGrid.AddLast(data);

            return imagesGrid;
        }

    }
}