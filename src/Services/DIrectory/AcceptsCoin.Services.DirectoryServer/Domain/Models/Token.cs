using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Token
    {

        [Key]
        public Guid TokenId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }

        public string Icon { get; set; }

        public string Logo { get; set; }

        public ICollection<BusinessToken> BusinessTokens { get; set; }

    }
}
