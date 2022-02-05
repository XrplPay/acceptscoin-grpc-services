using System;
using AcceptsCoin.Services.NotificationSenderServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.NotificationSenderServer.Data.Context
{
    public class AcceptsCoinNotificationSenderDbContext : DbContext
    {
        public AcceptsCoinNotificationSenderDbContext(DbContextOptions<AcceptsCoinNotificationSenderDbContext> options) : base(options)
        {
        }


        public DbSet<Message>  Messages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
