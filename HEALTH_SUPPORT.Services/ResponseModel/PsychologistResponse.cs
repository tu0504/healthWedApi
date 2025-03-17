﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class PsychologistResponse
    {
        public class GetPsychologistModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Specialization { get; set; }
            public string Description { get; set; }
            public string? ImgUrl { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class AvatarResponseModel
        {
            public string AvatarUrl { get; set; }
        }
    }
}
