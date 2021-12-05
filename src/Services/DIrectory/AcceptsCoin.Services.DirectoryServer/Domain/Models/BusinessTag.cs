using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class BusinessTag : BaseEntity, IEntity
    {

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }


        [ForeignKey(nameof(Tag))]
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }


        

    }
}
