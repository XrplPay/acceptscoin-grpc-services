using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }

        public string Logo { get; set; }

        public int Priority { get; set; }

    }
    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }

        public string Logo { get; set; }

        public int Priority { get; set; }

    }
}
