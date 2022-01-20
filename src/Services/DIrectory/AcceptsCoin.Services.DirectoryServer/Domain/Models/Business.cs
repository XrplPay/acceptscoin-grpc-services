using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{

    public class Business : BaseEntity, IEntity
    {

        [Key]
        public Guid BusinessId { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [Url]
        public string WebSiteUrl { get; set; }


        [Required]
        [Phone]
        public string ContactNumber { get; set; }


        [Required]
        public string Logo { get; set; }


        [Required]
        public string Owner { get; set; }


        public string Manager { get; set; }


        [Url]
        public string Twitter { get; set; }

        [Url]
        public string FaceBook { get; set; }

        [Url]
        public string Instagram { get; set; }


       

        public bool Verified { get; set; }

        [NotMapped]
        [Column(TypeName = "geography")]
        public Point Location { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }


        public string Description { get; set; }
        
        public string Address { get; set; }

        public string OfferedServices { get; set; }


        [ForeignKey(nameof(Category))]
        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }


        [ForeignKey(nameof(Partner))]
        [Required]
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; }


        

       // public ICollection<Comment> Comments { get; set; }

       // public ICollection<Rate> Rates { get; set; }

       // public ICollection<Wallet> Wallets { get; set; }

        public ICollection<BusinessTag> BusinessTags { get; set; }

       public ICollection<BusinessGallery> BusinessGalleries { get; set; }

       public ICollection<Review> Reviews { get; set; }

    }
}
