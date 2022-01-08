using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class BusinessTag
    {

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }


        [ForeignKey(nameof(Tag))]
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
