using System;
using AcceptsCoin.Services.SmsSenderServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.SmsSenderServer.Data.Context
{
    public class AcceptsCoinSmsSenderDbContext : DbContext
    {
        public AcceptsCoinSmsSenderDbContext(DbContextOptions<AcceptsCoinSmsSenderDbContext> options) : base(options)
        {
        }


        public DbSet<Message>  Messages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
