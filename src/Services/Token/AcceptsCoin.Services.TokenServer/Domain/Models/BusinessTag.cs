using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class BusinessTag : BaseEntity, IEntity
    {

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }


        [ForeignKey(nameof(Tag))]
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }


        

    }
}
