using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISurveyAnswerService
    {
        Task<SurveyAnswerResponse.GetSurveyAnswerModel?> GetSurveyAnswerById(Guid id);
        Task<SurveyAnswerResponse.GetSurveyAnswerModel?> GetByIdDeleted(Guid id);
        Task<List<SurveyAnswerResponse.GetSurveyAnswerModel?>> GetSurveyAnswerForQuestion(Guid questionIds);
        Task AddSurveyAnswerForSurveyQuestion(Guid surveyQuestionId, List<SurveyAnswerRequest.CreateSurveyAnswerRequest> model);
        Task UpdateSurveyAnswer(Guid id, SurveyAnswerRequest.UpdateSurveyAnswerRequest model);
        Task RemoveSurveyAnswer(Guid id);
    }
}
