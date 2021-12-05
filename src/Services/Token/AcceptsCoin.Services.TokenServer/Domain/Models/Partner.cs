using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class Partner
    {
        [Key]
        public Guid PartnerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }

        [Required]
        [Url]
        public string WebSiteUrl { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string ContactNumber { get; set; }

        public string Manager { get; set; }

        public string Owner { get; set; }

        public string Description { get; set; }

        public Guid ApiKey { get; set; }

        public bool Published { get; set; }

        public bool Deleted { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


        public Guid CreatedById { get; set; }

        public Guid? UpdatedById { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual User UpdatedBy { get; set; }


        [ForeignKey(nameof(Language))]
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }

        public ICollection<Business> Businesses { get; set; }

        public ICollection<PartnerServiceType> PartnerServiceTypes { get; set; }

        public ICollection<PartnerCoin> PartnerCoins { get; set; }

    }
}
