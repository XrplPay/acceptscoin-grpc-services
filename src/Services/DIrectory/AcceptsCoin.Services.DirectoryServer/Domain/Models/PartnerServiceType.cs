using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
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
