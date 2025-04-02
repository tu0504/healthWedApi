using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IUserProgressService
    {
        Task<List<UserProgressResponse.GetUserProgressModel>> GetUserProgress();
        Task<UserProgressResponse.GetUserProgressModel?> GetUserProgressById(Guid id);
        Task<UserProgressResponse.GetUserProgressModel?> GetUserProgressByIdDeleted(Guid id);
        Task AddUserProgress(UserProgressRequest.CreateUserProgressModel model);
        Task UpdateUserProgress(Guid id, UserProgressRequest.UpdateUserProgressModel model);
        Task RemoveUserProgress(Guid id);
    }
}
