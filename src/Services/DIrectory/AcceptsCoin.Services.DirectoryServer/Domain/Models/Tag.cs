using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Tag 
    {
        [Key]
        public Guid TagId { get; set; }

        public ICollection<BusinessTag> BusinessTags { get; set; }
    }
}
