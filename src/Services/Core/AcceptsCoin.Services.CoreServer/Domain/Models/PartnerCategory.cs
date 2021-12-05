using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.CoreServer.Domain.Common;

namespace AcceptsCoin.Services.CoreServer.Domain.Models
{
    public class PartnerCategory : BaseEntity, IEntity
    {
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }


        [ForeignKey(nameof(Partner))]
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; }

       
    }
}
