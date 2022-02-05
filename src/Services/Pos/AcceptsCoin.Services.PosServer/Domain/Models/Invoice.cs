using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.PosServer.Domain.Common;

namespace AcceptsCoin.Services.PosServer.Domain.Models
{
    public class Invoice : BaseEntity, IEntity
    {

        [Key]
        public Guid InvoiceId { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        public Guid? OrderId { get; set; }

        public string CustomerEmail { get; set; }

        public string NotificationUrl { get; set; }

        public string NotificationEmail { get; set; }

        [ForeignKey(nameof(Store))]
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}
