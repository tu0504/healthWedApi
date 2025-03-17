using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IHealthDataService
    {
        Task<HealthDataResponse.GetHealthDataModel?> GetHealthDataById(Guid id);
        Task<List<HealthDataResponse.GetHealthDataModel>> GetHealthDatas();
        Task<List<HealthDataResponse.GetHealthDataModel>> GetHealthDataByAccountId(Guid accountId);
        Task<List<HealthDataResponse.GetHealthDataModel>> GetHealthDataByPsychologistId(Guid psychologistId);
        Task AddHealthData(HealthDataRequest.AddHealthDataRequest model);
        Task UpdateHealthData(Guid id, HealthDataRequest.UpdateHealthDataRequest model);
        Task RemoveHealthData(Guid id);
    }
}
