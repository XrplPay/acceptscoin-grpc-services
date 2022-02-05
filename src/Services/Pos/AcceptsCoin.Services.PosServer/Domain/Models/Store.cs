using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.PosServer.Domain.Common;

namespace AcceptsCoin.Services.PosServer.Domain.Models
{
    public class Store : BaseEntity, IEntity
    {

        [Key]
        public Guid StoreId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string WebSite { get; set; }

        [Required]
        public string Email { get; set; }

        public string DefaultCurrency { get; set; }

        public int Threshold { get; set; }

        public int RefundDay { get; set; }

        public ICollection<Invoice> Invoices { get; set; }

    }
}
