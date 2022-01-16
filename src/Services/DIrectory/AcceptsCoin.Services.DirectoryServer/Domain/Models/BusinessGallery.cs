using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.DirectoryServer.Domain.Common;


namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
   

    public class BusinessGallery : BaseEntity, IEntity
    {
        [Key]
        public Guid BusinessGalleryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Extension { get; set; }

        [ForeignKey(nameof(Business))]
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
