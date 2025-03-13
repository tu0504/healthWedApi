using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISurveyQuestionService
    {
        Task<List<SurveyQuestionResponse.GetSurveyQuestionModel>> GetSurveyQuestions();
        Task<List<SurveyQuestionResponse.GetSurveyQuestionModel>> GetSurveyQuestionsForSurvey(Guid surveyId);
        Task<SurveyQuestionResponse.GetSurveyQuestionModel?> GetSurveyQuestionById(Guid id);
<<<<<<< HEAD
        Task<SurveyQuestionResponse.GetSurveyQuestionModel?> GetByIdDeleted(Guid id);
=======
>>>>>>> develop
        Task AddSurveyQuestionForSurvey(Guid surveyId, List<SurveyQuestionRequest.CreateSurveyQuestionRequest> model);
        Task UpdateSurveyQuestion(Guid id, SurveyQuestionRequest.UpdateSurveyQuestionRequest model);
        Task RemoveSurveyQuestion(Guid id);
    }
}
