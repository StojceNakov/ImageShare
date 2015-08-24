using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageShare.DataAccessLayer
{
    public class ImageShareDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ImageShareDbContext>
    {
        protected override void Seed(ImageShareDbContext context)
        {
            base.Seed(context);
        }
    }
}