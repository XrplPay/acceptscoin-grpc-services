using System;
using AcceptsCoin.Services.EmailSenderServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.EmailSenderServer.Data.Context
{
    public class AcceptsCoinEmailSenderDbContext : DbContext
    {
        public AcceptsCoinEmailSenderDbContext(DbContextOptions<AcceptsCoinEmailSenderDbContext> options) : base(options)
        {
        }


        public DbSet<Message>  Messages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
