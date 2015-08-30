namespace ImageShare.Migrations.ImageShareDbContext
{
    using ImageShare.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ImageShare.DataAccessLayer.ImageShareDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\ImageShareDbContext";
        }

        protected override void Seed(ImageShare.DataAccessLayer.ImageShareDbContext context)
        {
            Category uncategorized = new Category();
            uncategorized.Name = "Uncategorized";

            context.Categories.Add(uncategorized);
            context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
