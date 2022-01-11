using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.ApiGateway.Core.Dtos.Identity
{
    public class CreateUserDto
    {

        [Required]
        public string Name { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }


    }

    public class UpdateUserDto
    {

        [Required]
        public string Name { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        public string Password { get; set; }


    }
}
