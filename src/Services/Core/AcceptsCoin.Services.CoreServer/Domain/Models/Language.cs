using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.CoreServer.Domain.Common;

namespace AcceptsCoin.Services.CoreServer.Domain.Models
{
    public class Language : BaseEntity, IEntity
    {

        [Key]
        public Guid LanguageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Logo { get; set; }

        public string Icon { get; set; }

        public int Priority { get; set; }

        public ICollection<Partner> Partners { get; set; }

    }
}
