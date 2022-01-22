using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class BusinessToken
    {

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }


        [ForeignKey(nameof(Token))]
        public Guid TokenId { get; set; }
        public Token Token { get; set; }

    }
}
