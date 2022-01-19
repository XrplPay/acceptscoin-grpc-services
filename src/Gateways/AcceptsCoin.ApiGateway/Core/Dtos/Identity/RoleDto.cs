using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.ApiGateway.Core.Dtos.Identity
{
    public class CreateRoleDto
    {

        [Required]
        public string Name { get; set; }


        

    }
    public class UserRoleDto
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
    public class UpdateRoleDto
    {

        [Required]
        public string Name { get; set; }




    }
}
