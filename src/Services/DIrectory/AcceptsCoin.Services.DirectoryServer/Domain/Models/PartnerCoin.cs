using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class PartnerCoin : BaseEntity, IEntity
    {

        [ForeignKey(nameof(Partner))]
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; }


        [ForeignKey(nameof(Coin))]
        public Guid CoinId { get; set; }
        public Coin Coin { get; set; }

        

    }
}
