using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class CreatePartnerDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string WebSiteUrl { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Manager { get; set; }
        public string Owner { get; set; }
        public Guid LanguageId { get; set; }
    }
    public class UpdatePartnerDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string WebSiteUrl { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Manager { get; set; }
        public string Owner { get; set; }
        public Guid LanguageId { get; set; }

    }
}