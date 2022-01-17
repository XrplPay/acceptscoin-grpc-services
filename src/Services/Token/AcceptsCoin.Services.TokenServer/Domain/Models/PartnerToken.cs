using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class PartnerToken : BaseEntity, IEntity
    {

        [ForeignKey(nameof(Partner))]
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; }


        [ForeignKey(nameof(Token))]
        public Guid TokenId { get; set; }
        public Token Token { get; set; }

        

    }
}
