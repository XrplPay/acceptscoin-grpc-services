using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
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
