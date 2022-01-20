using System;
using AcceptsCoin.Services.DirectoryServer.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace AcceptsCoin.Services.DirectoryServer.Data.Context
{


    public class AcceptsCoinDirectoryDbContext : DbContext
    {
        public AcceptsCoinDirectoryDbContext(DbContextOptions<AcceptsCoinDirectoryDbContext> options) : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Business> Businesses { get; set; }

        public DbSet<BusinessGallery> BusinessGalleries { get; set; }

        public DbSet<BusinessTag> BusinessTags { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            #region UserSeed

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Email = "info@acceptscoin.com",
                Name = "Super Admin",
            });

            #endregion

            #region Category

            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = Guid.Parse("711425a4-07b9-4396-bb33-942a73ba6354"),
               
            },
            new Category
            {
                CategoryId = Guid.Parse("ba626277-b49c-4e2b-9410-16ca496f278d"),
                
            });

            #endregion

            

            #region PartnerSeed

            modelBuilder.Entity<Partner>().HasData(new Partner
            {
                PartnerId = Guid.Parse("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"),
                
            });

            #endregion

            #region PartnerToken
            modelBuilder.Entity<BusinessTag>()
                .HasKey(pt => new { pt.BusinessId, pt.TagId });

            modelBuilder.Entity<BusinessTag>()
                .HasOne(pt => pt.Business)
                .WithMany(p => p.BusinessTags)
                .HasForeignKey(pt => pt.BusinessId);

            modelBuilder.Entity<BusinessTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.BusinessTags)
                .HasForeignKey(pt => pt.TagId);

            #endregion

        }
    }
}
