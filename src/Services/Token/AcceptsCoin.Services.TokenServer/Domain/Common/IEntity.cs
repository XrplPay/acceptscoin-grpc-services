using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.TokenServer.Domain.Models;

namespace AcceptsCoin.Services.TokenServer.Domain.Common
{
    public interface IEntity { }

    public class BaseEntity<T> { }

    public class BaseEntity : BaseEntity<Guid>
    {
        
        public bool Published { get; set; }

        public bool Deleted { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


        [ForeignKey(nameof(User))]
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? UpdatedById { get; set; }
        public User UpdatedBy { get; set; }
    }
}
