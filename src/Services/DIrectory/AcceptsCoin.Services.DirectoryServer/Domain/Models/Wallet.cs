using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Wallet : BaseEntity, IEntity
    {
        [Key]
        public Guid WalletId { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        public string WalletAddress { get; set; }

        [Required]
        public bool Defaulted { get; set; }

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

        public ICollection<WalletCoin> WalletCoins { get; set; }
    }
}
