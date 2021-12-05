using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.IdentityServer.Domain.Common;

namespace AcceptsCoin.Services.IdentityServer.Domain.Models
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
