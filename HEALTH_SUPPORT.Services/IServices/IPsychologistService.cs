using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IPsychologistService
    {
        Task<List<PsychologistResponse.GetPsychologistsModel>> GetPsychologists();
        Task<PsychologistResponse.GetPsychologistsModel?> GetPsychologistById(Guid id);
        Task AddPsychologist(PsychologistRequest.CreatePsychologistModel model);
        Task UpdatePsychologist(Guid id, PsychologistRequest.UpdatePsychologistModel model);
        Task RemovePsychologist(Guid id);
    }
}
