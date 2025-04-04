﻿using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class Account : Entity<Guid>, IAuditable
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string? ImgUrl { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset LoginDate { get; set; }
        public ICollection<AccountSurvey> AccountSurveys { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<HealthData> HealthDatas { get; set; }
        public ICollection<UserProgress> UserProgresses { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public Psychologist Psychologist { get; set; }
    }
}