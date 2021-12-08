using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class Token : BaseEntity, IEntity
    {

        [Key]
        public Guid TokenId { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }

        public string Icon { get; set; }
        public string Description { get; set; }


        public string Link { get; set; }

        public string Logo { get; set; }

        public int Priority { get; set; }

        //public ICollection<PartnerCoin> PartnerCoins { get; set; }

        //public ICollection<WalletCoin> WalletCoins { get; set; }

    }
}
