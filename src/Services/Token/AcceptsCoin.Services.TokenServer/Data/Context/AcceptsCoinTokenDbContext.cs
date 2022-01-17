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

        public DbSet<Partner> Partners { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<PartnerToken> PartnerTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            #region PartnerSeed

            modelBuilder.Entity<Partner>().HasData(new Partner
            {
                PartnerId = Guid.Parse("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"),
            });

            #endregion

            #region UserSeed

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.Parse("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
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

        }
    }
}
