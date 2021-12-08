using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreateLanguageDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public int Priority { get; set; }
      

    }
    public class UpdateLanguageDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public int Priority { get; set; }


    }
};