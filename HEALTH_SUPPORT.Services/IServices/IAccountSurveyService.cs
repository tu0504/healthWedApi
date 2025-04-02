using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IAccountSurveyService
    {
        Task<List<AccountSurveyResponse.GetAccountSurveysModel>> GetAccountSurveysByAccountId(Guid accountId);
        Task<List<AccountSurveyResponse.GetAccountSurveysModel>> GetAccountSurveysBySurveyId(Guid surveyId);
        Task<List<AccountSurveyResponse.GetAccountSurveysModel>> GetAccountSurveys();
        Task<AccountSurveyResponse.GetAccountSurveysModel?> GetAccountSurveyById(Guid id);
        Task AddAccountSurvey(AccountSurveyRequest.CreateAccountSurveyModel model);
        Task RemoveAccountSurvey(Guid id);
    }
}
