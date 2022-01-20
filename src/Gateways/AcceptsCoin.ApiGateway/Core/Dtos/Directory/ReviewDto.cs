using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreateReviewDto
    {
        public string Messgae { get; set; }
        public int Rate { get; set; }
        public Guid BusinessId { get; set; }
      

    }
    public class UpdateReviewDto
    {
        public string Messgae { get; set; }
        public int Rate { get; set; }
        public Guid BusinessId { get; set; }


    }
}
