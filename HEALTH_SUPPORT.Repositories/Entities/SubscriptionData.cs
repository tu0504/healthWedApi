﻿using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class SubscriptionData : Entity<Guid>, IAuditable
    {
        [Required]
        [MaxLength(255)]
        public string SubscriptionName { get; set; }

        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Duration { get; set; }
        public string Purpose { get; set; }
        public string Criteria { get; set; }
        public string FocusGroup { get; set; }
        public string AssessmentTool { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid PsychologistId { get; set; }
        [ForeignKey("PsychologistId")]
        public Psychologist Psychologists { get; set; }
        public ICollection<SubscriptionProgress> SubscriptionProgresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<UserProgress> UserProgresses { get; set; }
    }
}
