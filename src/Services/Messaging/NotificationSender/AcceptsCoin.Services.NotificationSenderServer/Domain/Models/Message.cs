﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcceptsCoin.Services.NotificationSenderServer.Domain.Common;

namespace AcceptsCoin.Services.NotificationSenderServer.Domain.Models
{

    public class Message : BaseEntity, IEntity
    {
        [Key]
        public Guid MessageId { get; set; }


        [Required]
        public Guid RefrenceId { get; set; }


        [Required]
        public string Source { get; set; }


        [Required]
        public string Subject { get; set; }


        [Required]
        public string Body { get; set; }


        [Required]
        [EmailAddress]
        public string From { get; set; }


        [Required]
        [EmailAddress]
        public string To { get; set; }
    }
}
