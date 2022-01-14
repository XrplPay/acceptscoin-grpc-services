using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.CoreServer.Domain.Common;

namespace AcceptsCoin.Services.CoreServer.Domain.Models
{
    public class Category : BaseEntity, IEntity
    {
        [Key]
        public Guid CategoryId { get; set; }


        [Required]
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Logo { get; set; }

        public int Priority { get; set; }


        [ForeignKey(nameof(Category))]
        public Guid? ParentId { get; set; }
        public Category Parent { get; set; }

       // public ICollection<Business> Businesses { get; set; }

        public ICollection<PartnerCategory> PartnerCategories { get; set; }

        public ICollection<Category> Children { get; set; }
    }
}
