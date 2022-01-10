using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.ApiGateway.Core.Dtos.Directory
{
    public class CreateBusinessDto
    {
      
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


        public double Latitude { get; set; }

        public double Longitude { get; set; }


        public string Description { get; set; }

        public string Address { get; set; }

        public string OfferedServices { get; set; }

        public Guid CategoryId { get; set; }


         public Guid PartnerId { get; set; }



    }
    public class UpdateBusinessDto
    {

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


        public double Latitude { get; set; }

        public double Longitude { get; set; }


        public string Description { get; set; }

        public string Address { get; set; }

        public string OfferedServices { get; set; }

        public Guid CategoryId { get; set; }


        public Guid PartnerId { get; set; }



    }
}
