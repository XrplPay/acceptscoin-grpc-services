using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreateInvoiceDto
    {

        public Guid InvoiceId { get; set; }

        public Guid? OrderId { get; set; }


        public float Amount { get; set; }

        public string Currency { get; set; }

        public string CustomerEmail { get; set; }

        public string NotificationUrl { get; set; }

        public string NotificationEmail { get; set; }

    }
    public class UpdateInvoiceDto
    {
        public float Amount { get; set; }

        public string Currency { get; set; }

        public string CustomerEmail { get; set; }

        public string NotificationUrl { get; set; }

        public string NotificationEmail { get; set; }

    }
}
