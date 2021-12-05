using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class PartnerServiceType : BaseEntity, IEntity
    {
        [ForeignKey(nameof(ServiceType))]
        public Guid ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }


        [ForeignKey(nameof(Partner))]
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; }

       
    }
}
