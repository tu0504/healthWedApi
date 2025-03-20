using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HEALTH_SUPPORT.Services.RequestModel.AccountRequest;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IPsychologistService
    {
        Task<List<PsychologistResponse.GetPsychologistModel>> GetPsychologists();
        Task<PsychologistResponse.GetPsychologistModel?> GetPsychologistById(Guid id);
        Task AddPsychologist(PsychologistRequest.CreatePsychologistModel model);
        Task UpdatePsychologist(Guid id, PsychologistRequest.UpdatePsychologistModel model);
        Task RemovePsychologist(Guid id);

        Task<PsychologistResponse.AvatarResponseModel> UploadAvatarAsync(Guid id, PsychologistRequest.UploadAvatarModel model);
        Task<PsychologistResponse.AvatarResponseModel> UpdateAvatarAsync(Guid id, PsychologistRequest.UploadAvatarModel model);
        Task<List<PsychologistResponse.GetPsychologistModel>> GetPsychologistDetailByManager();
        Task<List<PsychologistResponse.GetPsychologistWithAccountModel>> GetPsychologistProfileByManager();
        Task AddPsychologistToManager(CreateAccountAndPsychologistModel model);
        Task UpdatePsychologistToManager(Guid id, PsychologistRequest.UpdatePsychologistModel model);
        Task DeletePsychologistById(Guid id);
        Task RemoveAvatarAsync(Guid id);
    }
}
