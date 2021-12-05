using System;
using AcceptsCoin.Services.CoreServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.CoreServer.Data.Context
{
    public class AcceptsCoinCoreDbContext : DbContext
    {
        public AcceptsCoinCoreDbContext(DbContextOptions<AcceptsCoinCoreDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<PartnerCategory> PartnerCategories { get; set; }

        public DbSet<PartnerToken> PartnerTokens { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Token> Tokens { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            #region UserSeed

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
            });

            #endregion

            #region Category

            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = Guid.Parse("711425a4-07b9-4396-bb33-942a73ba6354"),
                Deleted = false,
                Name = "Food",
                CreatedDate = DateTime.Now,
                Published = true,
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Icon = "",
                Logo = "",
                Priority = 1,
            },
            new Category
            {
                CategoryId = Guid.Parse("ba626277-b49c-4e2b-9410-16ca496f278d"),
                Deleted = false,
                Name = "Car",
                CreatedDate = DateTime.Now,
                Published = true,
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Icon = "",
                Logo = "",
                Priority = 2,
            });

            #endregion

            #region LanguageSeed

            modelBuilder.Entity<Language>().HasData(new Language
            {
                Name = "ENGLISH",
                LanguageId = Guid.Parse("9934b846-e3f1-406a-9207-04926e553d1b"),
                Code = "en",
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Icon = "",
                Logo = "",
                Published = true,
                CreatedDate = DateTime.Now,
                Deleted = false,
            });

            #endregion

            #region PartnerSeed

            modelBuilder.Entity<Partner>().HasData(new Partner
            {
                PartnerId = Guid.Parse("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"),
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Published = true,
                CreatedDate = DateTime.Now,
                Deleted = false,
                Email = "info@acceptscoin.com",
                LanguageId = Guid.Parse("9934b846-e3f1-406a-9207-04926e553d1b"),
                WebSiteUrl = "https://acceptscoin.com",
                Name = "Accepts Coin",
                Logo = "",
                ContactNumber = "0000",
                ApiKey = Guid.Parse("a96b2fa5-fbf2-4173-92eb-c5c9ce77e7dc"),
            });

            #endregion

            #region PartnerToken
            modelBuilder.Entity<PartnerToken>()
                .HasKey(pt => new { pt.PartnerId, pt.TokenId });

            modelBuilder.Entity<PartnerToken>()
                .HasOne(pt => pt.Partner)
                .WithMany(p => p.PartnerTokens)
                .HasForeignKey(pt => pt.PartnerId);

            modelBuilder.Entity<PartnerToken>()
                .HasOne(pt => pt.Token)
                .WithMany(t => t.PartnerTokens)
                .HasForeignKey(pt => pt.TokenId);

            #endregion

            #region PartnerCategory
            modelBuilder.Entity<PartnerCategory>()
                .HasKey(pt => new { pt.PartnerId, pt.CategoryId });

            modelBuilder.Entity<PartnerCategory>()
                .HasOne(pt => pt.Partner)
                .WithMany(p => p.PartnerCategories)
                .HasForeignKey(pt => pt.PartnerId);

            modelBuilder.Entity<PartnerCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(t => t.PartnerCategories)
                .HasForeignKey(pt => pt.CategoryId);

            #endregion
        }
    }
}
