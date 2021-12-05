using System;
using AcceptsCoin.Services.IdentityServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.IdentityServer.Data.Context
{
    public class AcceptsCoinIdentityDbContext : DbContext
    {
        public AcceptsCoinIdentityDbContext(DbContextOptions<AcceptsCoinIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            #region UserSeed

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                UserName = "info@acceptscoin.com",
                Activated = true,
                Deleted = false,
                Email = "info@acceptscoin.com",
                Name = "Super Admin",
                //Password = PasswordHelper.EncodePasswordMd5("superAdmin@123"),
                Password= "superAdmin@123",
                CreatedDate = DateTime.Now,
                Published = true,
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                SubscribedNewsLetter = true,
                UpdatePassword = true,
            });

            #endregion

            #region User

            modelBuilder.Entity<User>()
                    .HasOne(m => m.CreatedBy)
                    .WithMany(t => t.UserCreatorUser)
                    .HasForeignKey(m => m.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<User>()
                    .HasOne(m => m.UpdatedBy)
                    .WithMany(t => t.UserUpdaterUser)
                    .HasForeignKey(m => m.UpdatedById)
                    .OnDelete(DeleteBehavior.NoAction);
            #endregion


            #region RoleSeed
            modelBuilder.Entity<Role>().HasData(new Role
            {
                RoleId = Guid.Parse("27fc6d20-b661-43c5-b48d-93eca8185ece"),
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Published = true,
                CreatedDate = DateTime.Now,
                Deleted = false,
                Name = "Administrator"

            },
            new Role
            {
                RoleId = Guid.Parse("90000f34-509f-4a81-877c-0c0cafadb573"),
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Published = true,
                CreatedDate = DateTime.Now,
                Deleted = false,
                Name = "Business"
            },
            new Role
            {
                RoleId = Guid.Parse("553ca261-6db1-4413-af03-4c1549a4d1de"),
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Published = true,
                CreatedDate = DateTime.Now,
                Deleted = false,
                Name = "Partner"
            },
            new Role
            {
                RoleId = Guid.Parse("25b3c861-2fd3-4b6e-a623-d24010a9500f"),
                CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                Published = true,
                CreatedDate = DateTime.Now,
                Deleted = false,
                Name = "User"
            });


            #endregion

            #region UserRoleSeed
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                RoleId = Guid.Parse("27fc6d20-b661-43c5-b48d-93eca8185ece"),
                //CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                //Published = true,
                //CreatedDate = DateTime.Now,
                //Deleted = false,
            }, new UserRole
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                RoleId = Guid.Parse("90000f34-509f-4a81-877c-0c0cafadb573"),
                //CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                //Published = true,
                //CreatedDate = DateTime.Now,
                //Deleted = false,
            }, new UserRole
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                RoleId = Guid.Parse("553ca261-6db1-4413-af03-4c1549a4d1de"),
                //CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                //Published = true,
                //CreatedDate = DateTime.Now,
                //Deleted = false,
            }, new UserRole
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                RoleId = Guid.Parse("25b3c861-2fd3-4b6e-a623-d24010a9500f"),
                //CreatedById = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                //Published = true,
                //CreatedDate = DateTime.Now,
                //Deleted = false,
            });

            #endregion

            #region UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(pt => new { pt.UserId, pt.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(pt => pt.RoleId);

            #endregion
        }
    }
}
