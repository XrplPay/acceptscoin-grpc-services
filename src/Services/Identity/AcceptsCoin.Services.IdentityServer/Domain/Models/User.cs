using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AcceptsCoin.Services.IdentityServer.Domain.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }

        
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }




        public bool Activated { get; set; }

        public bool SubscribedNewsLetter { get; set; }

        public bool UpdatePassword { get; set; }

        public bool Published { get; set; }

        public bool Deleted { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


        [ForeignKey(nameof(Partner))]
        public Guid? PartnerId { get; set; }
        public Partner Partner { get; set; }

        //public ICollection<Partner> PartnerCreatorUser { get; set; }
        //public ICollection<Partner> PartnerUpdaterUser { get; set; }


        public ICollection<User> UserCreatorUser { get; set; }
        public ICollection<User> UserUpdaterUser { get; set; }


        public ICollection<UserRole> UserRoles { get; set; }


        public Guid? CreatedById { get; set; }

        public Guid? UpdatedById { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual User UpdatedBy { get; set; }
    }
}
