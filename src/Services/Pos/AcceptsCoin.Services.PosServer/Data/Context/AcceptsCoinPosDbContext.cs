using System;
using AcceptsCoin.Services.PosServer.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace AcceptsCoin.Services.PosServer.Data.Context
{


    public class AcceptsCoinPosDbContext : DbContext
    {
        public AcceptsCoinPosDbContext(DbContextOptions<AcceptsCoinPosDbContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



        }
    }
}
