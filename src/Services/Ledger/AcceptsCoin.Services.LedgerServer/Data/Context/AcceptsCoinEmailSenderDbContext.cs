using System;
using AcceptsCoin.Services.LedgerServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptsCoin.Services.LedgerServer.Data.Context
{
    public class AcceptsCoinLedgerSenderDbContext : DbContext
    {
        public AcceptsCoinLedgerSenderDbContext(DbContextOptions<AcceptsCoinLedgerSenderDbContext> options) : base(options)
        {
        }


        public DbSet<Message>  Messages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
