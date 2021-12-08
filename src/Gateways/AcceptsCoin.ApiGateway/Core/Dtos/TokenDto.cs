using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreateTokenDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Logo { get; set; }
        public int Priority { get; set; }

        public string Link { get; set; }
    }
    public class UpdateTokenDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Logo { get; set; }
        public int Priority { get; set; }

        public string Link { get; set; }

    }
}