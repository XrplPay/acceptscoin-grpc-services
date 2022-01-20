using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Review : BaseEntity, IEntity
    {
        [Key]
        public Guid ReviewId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int Rate { get; set; }


        [ForeignKey(nameof(Business))]
        [Required]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }


        //public ICollection<BusinessTag> BusinessTags { get; set; }
    }
}
