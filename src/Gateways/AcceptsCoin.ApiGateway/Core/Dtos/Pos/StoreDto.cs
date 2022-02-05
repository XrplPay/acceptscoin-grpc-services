using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreateStoreDto
    {

        public string Name { get; set; }

        public string WebSite { get; set; }

        public string Email { get; set; }

        public string DefaultCurrency { get; set; }

        public int Threshold { get; set; }

        public int RefundDay { get; set; }

    }
    public class WoocommerceCreateStoreDto
    {
        public string Name { get; set; }

        public string WebSite { get; set; }

        public string Email { get; set; }

        public string DefaultCurrency { get; set; }

    }
    public class UpdateStoreDto
    {
        public string Name { get; set; }

        public string DefaultCurrency { get; set; }

        public int Threshold { get; set; }

        public int RefundDay { get; set; }

    }
}
