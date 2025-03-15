using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISurveyQuestionSurveyService
    {
        Task AddSurveyQuestionSurvey(SurveyQuestionSurveyRequest.AddSurveyQuestionSurvey model);
        Task RemoveSurveyQuestionSurvey(Guid surveysId, Guid surveyQuestionsId);
    }
}
