using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class Rate : BaseEntity, IEntity
    {
        [Key]
        public Guid RateId { get; set; }

        public int Value { get; set; }

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

        
    }
}
