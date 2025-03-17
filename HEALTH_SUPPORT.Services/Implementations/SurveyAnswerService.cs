using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class SurveyAnswerService : ISurveyAnswerService
    {
        private readonly IBaseRepository<SurveyAnswer, Guid> _surveyAnswerRepository;
        private readonly IBaseRepository<SurveyQuestion, Guid> _surveyQuestionRepository;
        private readonly IBaseRepository<SurveyQuestionAnswer, Guid> _surveyQuestionAnswerRepository;

        public SurveyAnswerService(IBaseRepository<SurveyAnswer, Guid> surveyAnswerRepository, IBaseRepository<SurveyQuestion, 
            Guid> surveyQuestionRepository, IBaseRepository<SurveyQuestionAnswer, Guid> surveyQuestionAnswerRepository)
        {
            _surveyAnswerRepository = surveyAnswerRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _surveyQuestionAnswerRepository = surveyQuestionAnswerRepository;
        }

        public async Task AddSurveyAnswerForSurveyQuestion(Guid surveyQuestionId, List<SurveyAnswerRequest.CreateSurveyAnswerRequest> model)
        {
            foreach (var item in model)
            {
                var answer = new SurveyAnswer
                {
                    Content = item.Content,
                    CreateAt = DateTime.Now,
                    Point = item.Point
                };
                await _surveyAnswerRepository.Add(answer);
                var surveyQuestionAnswer = new SurveyQuestionAnswer
                {
                    SurveyAnswersId = answer.Id,
                    SurveyQuestionsId = surveyQuestionId
                };
                await _surveyQuestionAnswerRepository.Add(surveyQuestionAnswer);
                await _surveyQuestionAnswerRepository.SaveChangesAsync();
            }
            await _surveyQuestionRepository.SaveChangesAsync();
        }

        public async Task<SurveyAnswerResponse.GetSurveyAnswerModel?> GetSurveyAnswerById(Guid id)
        {
            var surveyAnswer = await _surveyAnswerRepository.GetById(id);
            if(surveyAnswer is null || surveyAnswer.IsDeleted)
            {
                throw new Exception("Không tìm thấy câu trả lời.");
            }
            return new SurveyAnswerResponse.GetSurveyAnswerModel
            {
                Id = id,
                Content = surveyAnswer.Content,
                CreateAt = surveyAnswer.CreateAt,
                ModifiedAt = surveyAnswer.ModifiedAt,
                Point = surveyAnswer.Point,
                IsDelete = surveyAnswer.IsDeleted,
            };
        }

        public async Task<SurveyAnswerResponse.GetSurveyAnswerModel?> GetByIdDeleted(Guid id)
        {
            var surveyAnswer = await _surveyAnswerRepository.GetById(id);
            if (surveyAnswer is null)
            {
                throw new Exception("Không tìm thấy câu trả lời.");
            }
            return new SurveyAnswerResponse.GetSurveyAnswerModel
            {
                Id = id,
                Content = surveyAnswer.Content,
                CreateAt = surveyAnswer.CreateAt,
                ModifiedAt = surveyAnswer.ModifiedAt,
                Point = surveyAnswer.Point,
                IsDelete = surveyAnswer.IsDeleted,
            };
        }

        public async Task<List<SurveyAnswerResponse.GetSurveyAnswerModel?>> GetSurveyAnswerForQuestion(Guid questionIds)
        {
            var answerIdList = await _surveyQuestionAnswerRepository.GetAll()
                .Where(s => s.SurveyQuestionsId == questionIds).Select(s => s.SurveyAnswersId).ToListAsync();
            if(!answerIdList.Any())
            {
                return new List<SurveyAnswerResponse.GetSurveyAnswerModel?>();
            }
            var answerList = await _surveyAnswerRepository.GetAll()
                .Where(s => answerIdList.Contains(s.Id))
                .Select(s => new SurveyAnswerResponse.GetSurveyAnswerModel
                {
                    Id = s.Id,
                    Content = s.Content,
                    IsDelete = s.IsDeleted,
                    Point = s.Point
                }).ToListAsync();
            return answerList;
        }

        public async Task RemoveSurveyAnswer(Guid id)
        {
            var surveyAnswer = await _surveyAnswerRepository.GetById(id);
            if (surveyAnswer is null || surveyAnswer.IsDeleted)
            {
                throw new Exception("Không tìm thấy câu trả lời.");
            }
            surveyAnswer.ModifiedAt = DateTime.Now;
            surveyAnswer.IsDeleted = true;
            await _surveyAnswerRepository.Update(surveyAnswer);
            await _surveyAnswerRepository.SaveChangesAsync();
        }

        public async Task UpdateSurveyAnswer(Guid id, SurveyAnswerRequest.UpdateSurveyAnswerRequest model)
        {
            var surveyAnswer = await _surveyAnswerRepository.GetById(id);
            if (surveyAnswer is null)
            {
                throw new Exception("Không tìm thấy câu trả lời.");
            }
            surveyAnswer.Content = model.Content;
            surveyAnswer.IsDeleted = model.IsDelete.HasValue ? model.IsDelete.Value : surveyAnswer.IsDeleted;
            surveyAnswer.Point = model.Point;
            surveyAnswer.ModifiedAt = DateTime.Now;
            await _surveyAnswerRepository.Update(surveyAnswer);
            await _surveyAnswerRepository.SaveChangesAsync();
        }
    }
}
