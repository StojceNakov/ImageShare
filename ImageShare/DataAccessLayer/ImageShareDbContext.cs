using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ImageShare.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ImageShare.DataAccessLayer
{
    public class ImageShareDbContext : DbContext
    {
        public ImageShareDbContext()
            : base("ImageShareDbContext")
        {

        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        static ImageShareDbContext()
        {
            Database.SetInitializer<ImageShareDbContext>(new ImageShareDbInitializer());
        }

        public static ImageShareDbContext Create()
        {
            return new ImageShareDbContext();
        }
    }
}