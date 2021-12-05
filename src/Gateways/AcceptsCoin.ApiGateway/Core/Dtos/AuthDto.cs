using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.ApiGateway.Core.Dtos
{
    public class AuhenticateDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
