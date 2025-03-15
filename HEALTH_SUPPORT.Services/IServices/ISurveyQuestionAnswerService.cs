using HEALTH_SUPPORT.Services.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISurveyQuestionAnswerService
    {
        Task AddSurveyQuestionAnswer(SurveyQuestionAnswerRequest.AddSurveyQuestionAnswer model);
        Task RemoveSurveyQuestionAnswer(Guid surveyQuestionsId, Guid surveyAnswersId);
    }
}
