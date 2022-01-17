using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class Partner
    {
        [Key]
        public Guid PartnerId { get; set; }

        public ICollection<PartnerToken> PartnerTokens { get; set; }

    }
}
