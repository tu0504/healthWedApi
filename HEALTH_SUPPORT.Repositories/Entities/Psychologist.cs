﻿using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class Psychologist : Entity<Guid>,IAuditable
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Specialization { get; set; }
        public string Description { get; set; }
        public string Achievements { get; set; }
        public string Expertise { get; set; }
        public string? UrlMeet { get; set; }
        public string? ImgUrl { get; set; }

        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedAt { get; set; }
        public ICollection<HealthData> HealthDatas { get; set; }
        public ICollection<SubscriptionData> SubscriptionDatas { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        [Required] 
        public Guid AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
