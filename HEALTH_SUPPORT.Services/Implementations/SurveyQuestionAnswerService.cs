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
    public class SurveyQuestionAnswerService : ISurveyQuestionAnswerService
    {
        private readonly IBaseRepository<SurveyQuestionAnswer, Guid> _surveyQuestionAnswerRepository;

        public SurveyQuestionAnswerService(IBaseRepository<SurveyQuestionAnswer, Guid> SurveyQuestionAnswerRepository)
        {
            _surveyQuestionAnswerRepository = SurveyQuestionAnswerRepository;
        }
        public async Task AddSurveyQuestionAnswer(SurveyQuestionAnswerRequest.AddSurveyQuestionAnswer model)
        {
            var surveyAnswer = new SurveyQuestionAnswer
            {
                SurveyQuestionsId = model.SurveyQuestionsId,
                SurveyAnswersId = model.SurveyAnswersId
            };
            await _surveyQuestionAnswerRepository.Add(surveyAnswer);
            await _surveyQuestionAnswerRepository.SaveChangesAsync();
        }

        public async Task RemoveSurveyQuestionAnswer(Guid surveyQuestionsId, Guid surveyAnswersId)
        {
            var surveyQuestion = await _surveyQuestionAnswerRepository.GetAll().FirstOrDefaultAsync(s => s.SurveyQuestionsId == surveyQuestionsId && s.SurveyAnswersId == surveyAnswersId);
            if (surveyQuestion is null)
            {
                throw new Exception("Không tìm thấy câu hỏi");
            }
            await _surveyQuestionAnswerRepository.Remove(surveyQuestion);
            await _surveyQuestionAnswerRepository.SaveChangesAsync();
        }
    }
}
