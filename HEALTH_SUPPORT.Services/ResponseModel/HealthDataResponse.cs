using HEALTH_SUPPORT.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class HealthDataResponse
    {
        public class GetHealthDataModel
        {
            public Guid Id { get; set; }
            public int level { get; set; }
            public string description { get; set; }
            public DateTimeOffset FollowUpAppoint { get; set; }
            public Guid AccountId { get; set; }
            public AccountResponse.GetAccountsModel Account { get; set; }
            public PsychologistResponse.GetPsychologistModel Psychologist { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}
