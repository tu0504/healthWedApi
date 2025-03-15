using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class SurveyQuestionSurveyService : ISurveyQuestionSurveyService
    {
        private readonly IBaseRepository<SurveyQuestionSurvey, Guid> _surveyQuestionSurveyRepository;

        public SurveyQuestionSurveyService(IBaseRepository<SurveyQuestionSurvey, Guid> surveyQuestionSurveyRepository)
        {
            _surveyQuestionSurveyRepository = surveyQuestionSurveyRepository;
        }

        public async Task AddSurveyQuestionSurvey(SurveyQuestionSurveyRequest.AddSurveyQuestionSurvey model)
        {
            var surveyQuestion = new SurveyQuestionSurvey
            {
                SurveyQuestionsId = model.SurveyQuestionsId,
                SurveysId = model.SurveysId
            };
            await _surveyQuestionSurveyRepository.Add(surveyQuestion);
            await _surveyQuestionSurveyRepository.SaveChangesAsync();
        }

        public async Task RemoveSurveyQuestionSurvey(Guid surveysId, Guid surveyQuestionsId)
        {
            var surveyQuestion = await _surveyQuestionSurveyRepository.GetAll().FirstOrDefaultAsync(s => s.SurveyQuestionsId == surveyQuestionsId && s.SurveysId == surveysId);
            if(surveyQuestion is null)
            {
                throw new Exception("Không tìm thấy câu hỏi");
            }
            await _surveyQuestionSurveyRepository.Remove(surveyQuestion);
            await _surveyQuestionSurveyRepository.SaveChangesAsync();
        }
    }
}
