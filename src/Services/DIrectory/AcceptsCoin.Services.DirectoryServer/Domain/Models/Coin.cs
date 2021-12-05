using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Coin : BaseEntity, IEntity
    {

        [Key]
        public Guid CoinId { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }

        public string Description { get; set; }


        public string Link { get; set; }

        public string Logo { get; set; }

        public int Priority { get; set; }

        public ICollection<PartnerCoin> PartnerCoins { get; set; }

        public ICollection<WalletCoin> WalletCoins { get; set; }

    }
}
