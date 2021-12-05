using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Common;

namespace AcceptsCoin.Services.TokenServer.Domain.Models
{
    public class Role : BaseEntity, IEntity
    {

        [Key]
        public Guid RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
}
