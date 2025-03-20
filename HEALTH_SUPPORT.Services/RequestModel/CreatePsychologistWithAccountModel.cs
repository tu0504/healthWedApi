using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class CreatePsychologistWithAccountModel
    {
        public PsychologistRequest.CreatePsychologistModel Psychologist { get; set; }
        public AccountRequest.CreateAccountModel Account { get; set; }
    }
}
