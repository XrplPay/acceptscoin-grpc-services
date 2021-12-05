using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.CoreServer.Domain.Common;

namespace AcceptsCoin.Services.CoreServer.Domain.Models
{
    public class Token : BaseEntity, IEntity
    {

        [Key]
        public Guid TokenId { get; set; }

        public ICollection<PartnerToken> PartnerTokens { get; set; }

    }
}
