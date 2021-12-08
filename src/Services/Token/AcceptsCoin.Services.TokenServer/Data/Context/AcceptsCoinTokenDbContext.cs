using System;
using AcceptsCoin.Services.TokenServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.TokenServer.Data.Context
{
    public class AcceptsCoinTokenDbContext : DbContext
    {
        public AcceptsCoinTokenDbContext(DbContextOptions<AcceptsCoinTokenDbContext> options) : base(options)
        {
        }

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

           
        }
    }
}
