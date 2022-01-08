using System;
using System.ComponentModel.DataAnnotations;

namespace AcceptsCoin.Services.DirectoryServer.Domain.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }


    }
}
