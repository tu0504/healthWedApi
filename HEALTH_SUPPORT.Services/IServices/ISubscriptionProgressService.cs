using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISubscriptionProgressService
    {
        Task<List<SubscriptionProgressResponse.GetProgressModel>> GetSubscriptionProgress();
        Task<SubscriptionProgressResponse.GetProgressModel?> GetSubscriptionProgressById(Guid id);
        Task<SubscriptionProgressResponse.GetProgressModel?> GetSubscriptionProgressByIdDeleted(Guid id);
        Task AddSubscriptionProgress(SubscriptionProgressRequest.CreateProgressModel model);
        Task UpdateSubscriptionProgress(Guid id, SubscriptionProgressRequest.UpdateProgressModel model);
        Task RemoveSubscriptionProgress(Guid id);

        Task<List<DateTimeOffset?>> GetStartDatesByPsychologistNameAsync(string psychologistName);
    }
}
