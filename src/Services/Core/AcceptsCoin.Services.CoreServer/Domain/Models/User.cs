using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AcceptsCoin.Services.CoreServer.Domain.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

    }
}
