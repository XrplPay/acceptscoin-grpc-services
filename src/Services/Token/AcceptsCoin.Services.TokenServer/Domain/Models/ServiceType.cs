using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class ServiceType : BaseEntity, IEntity
    {
        [Key]
        public Guid ServiceTypeId { get; set; }


        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Logo { get; set; }

        public int Priority { get; set; }


        //[ForeignKey(nameof(ServiceType))]
        //public Guid? ParentId { get; set; }
        //public ServiceType Parent { get; set; }

        public ICollection<Business> Businesses { get; set; }

        public ICollection<PartnerServiceType> PartnerServiceTypes { get; set; }
    }
}
