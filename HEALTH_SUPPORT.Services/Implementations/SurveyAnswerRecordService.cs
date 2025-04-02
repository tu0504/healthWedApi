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
    public class SurveyAnswerRecordService : ISurveyAnswerRecordService
    {
        private readonly IBaseRepository<SurveyAnswerRecord, Guid> _surveyAnswerRecordRepository;

        public SurveyAnswerRecordService(IBaseRepository<SurveyAnswerRecord, Guid> surveyAnswerRecordRepository)
        {
            _surveyAnswerRecordRepository = surveyAnswerRecordRepository;
        }

        public async Task AddSurveyAnswerRecord(SurveyAnswerRecordRequest.AddSurveyAnswerRecordRequest model)
        {
            if (model.QuestionAndAnswerRequests.Any())
            {
                foreach(var item in model.QuestionAndAnswerRequests)
                {
                    var surveyRecord = new SurveyAnswerRecord
                    {
                        AccountSurveyId = model.AccountSurveyId,
                    };
                    surveyRecord.SurveyQuestionId = item.SurveyQuestionId;
                    surveyRecord.SurveyAnswerId = item.SurveyAnswerId;
                    await _surveyAnswerRecordRepository.Add(surveyRecord);
                }
            }
            await _surveyAnswerRecordRepository.SaveChangesAsync();
        }

        public async Task<List<SurveyAnswerRecordResponse.SurveyAnswerRecordResponseModel?>> GetSurveyAnswerRecordById(Guid accountSurveyId)
        {
            var userAnswers = await _surveyAnswerRecordRepository.GetAll()
                         .Where(s=> s.AccountSurveyId == accountSurveyId && s.IsDeleted == false)
                         .Include(s => s.SurveyQuestion)
                         .Include(s => s.SurveyAnswer)
                         .Select(s => new SurveyAnswerRecordResponse.SurveyAnswerRecordResponseModel
                         {
                              Id = s.Id,
                              AccountSurveyId = s.AccountSurveyId,
                              IsDeleted = s.IsDeleted,
                              SurveyQuestionId = s.SurveyQuestionId,
                              SurveyAnswerId = s.SurveyAnswerId,
                              SurveyQuestion = new SurveyQuestionResponse.GetSurveyQuestionModel
                              {
                                  ContentQ = s.SurveyQuestion.ContentQ,
                                  CreateAt = s.SurveyQuestion.CreateAt
                              },
                              SurveyAnswer = new SurveyAnswerResponse.GetSurveyAnswerModel
                              {
                                  Content = s.SurveyAnswer.Content,
                                  Point = s.SurveyAnswer.Point
                              }
                         })
                         .ToListAsync();
            return userAnswers;
        }

        public async Task RemoveSurveyAnswerRecord(Guid id)
        {
            var surveyRecord = await _surveyAnswerRecordRepository.GetById(id);
            if(surveyRecord is null)
            {
                throw new Exception("Không tìm thấy kết quả khảo sát");
            }
            surveyRecord.IsDeleted = true;
            surveyRecord.ModifiedAt = DateTime.Now;
            await _surveyAnswerRecordRepository.Update(surveyRecord);
            await _surveyAnswerRecordRepository.SaveChangesAsync();
        }
    }
}
