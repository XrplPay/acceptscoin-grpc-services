using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class WalletCoin : BaseEntity, IEntity
    {

        [ForeignKey(nameof(Wallet))]
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }


        [ForeignKey(nameof(Coin))]
        public Guid CoinId { get; set; }
        public Coin Coin { get; set; }

        

    }
}
