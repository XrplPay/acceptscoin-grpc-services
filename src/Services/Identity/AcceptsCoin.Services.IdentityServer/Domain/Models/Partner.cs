using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.Services.IdentityServer.Domain.Models
{
    public class Partner
    {
        [Key]
        public Guid PartnerId { get; set; }


    }
}
