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
        Task CreateSubscriptionProgress(SubscriptionProgressRequest.CreateSubscriptionProgressModel model);
        Task <SubscriptionProgressResponse.GetSubscriptionProgressModel> GetSubscriptionProgressById(Guid progressId);
        Task<List<SubscriptionProgressResponse.GetSubscriptionProgressModel>> GetAllSubscriptionProgress();
    }
}
