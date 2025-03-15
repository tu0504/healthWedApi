using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class PsychologistResponse
    {
        public record GetPsychologistsModel(
            Guid Id,
            string Name,
            string Email,
            string PhoneNumber,
            string Specialization,
            DateTimeOffset CreateAt
        );
    }
}
