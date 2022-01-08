using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Partner
    {
        [Key]
        public Guid PartnerId { get; set; }


    }
}
