using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.CoreServer.Domain.Common;

namespace AcceptsCoin.Services.CoreServer.Domain.Models
{
    public class Tag : BaseEntity, IEntity
    {
        [Key]
        public Guid TagId { get; set; }

        [Required]
        public string Title { get; set; }

        

        //public ICollection<BusinessTag> BusinessTags { get; set; }
    }
}
