using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class UserRole
    {

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }


        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        

    }
}
