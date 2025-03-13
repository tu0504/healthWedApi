using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class SurveyResultService : ISurveyResultsService
    {
        private readonly IBaseRepository<SurveyResults, Guid> _surveyResultsRepository;
        private readonly IBaseRepository<SurveyAnswer, Guid> _surveyAnswerRepository;
        private readonly IBaseRepository<Survey, Guid> _surveyRepository;

        public SurveyResultService(IBaseRepository<SurveyResults, Guid> surveyResultsRepository, IBaseRepository<SurveyAnswer, Guid> surveyAnswerRepository, IBaseRepository<Survey, Guid> surveyRepository)
        {
            _surveyResultsRepository = surveyResultsRepository;
            _surveyAnswerRepository = surveyAnswerRepository;
            _surveyRepository = surveyRepository;
        }

        public async Task AddSurveyResult(Guid surveyId,SurveyResultRequest.AddSurveyResultRequest model)
        {
            var survey = _surveyRepository.GetAll().Where(s => s.Id == surveyId && s.IsDeleted).FirstOrDefault();
            if (survey is null)
            {
                throw new Exception("Không tìm thấy bảng khảo sát.");
            }

            var result = new SurveyResults
            {
                CreateAt = DateTime.Now,
                ResultDescription = model.ResultDescription,
                SurveyId = model.SurveyId,
                MaxScore = model.MaxScore,
                MinScore = model.MinScore,
            };
            await _surveyResultsRepository.Add(result);
            await _surveyAnswerRepository.SaveChangesAsync();
        }

        public async Task<SurveyResultResponse.GetSurveyResultModel?> GetSurveyResultById(Guid id)
        {
            var surveyResult = await _surveyResultsRepository.GetById(id);
            if (surveyResult is null || surveyResult.IsDeleted)
            {
                return null;
            }
            return new SurveyResultResponse.GetSurveyResultModel
            {
                SurveyId = surveyResult.SurveyId,
                CreateAt = surveyResult.CreateAt,
                ResultDescription = surveyResult.ResultDescription,
                ModifiedAt = surveyResult.ModifiedAt,
                MaxScore = surveyResult.MaxScore,
                MinScore = surveyResult.MinScore,
                Id = surveyResult.Id,
                IsDelete = surveyResult.IsDeleted
            };
        }

        public async Task<SurveyResultResponse.GetSurveyResultModel?> GetByIdDeleted(Guid id)
        {
            var surveyResult = await _surveyResultsRepository.GetById(id);
            if (surveyResult is null)
            {
                return null;
            }
            return new SurveyResultResponse.GetSurveyResultModel
            {
                SurveyId = surveyResult.SurveyId,
                CreateAt = surveyResult.CreateAt,
                ResultDescription = surveyResult.ResultDescription,
                ModifiedAt = surveyResult.ModifiedAt,
                MaxScore = surveyResult.MaxScore,
                MinScore = surveyResult.MinScore,
                Id = surveyResult.Id,
                IsDelete = surveyResult.IsDeleted
            };
        }

        public async Task<List<SurveyResultResponse.GetSurveyResultModel>> GetSurveyResults()
        {
            return await _surveyResultsRepository.GetAll().Where(s => !s.IsDeleted).Select(s => new SurveyResultResponse.GetSurveyResultModel
            {
                SurveyId = s.SurveyId,
                MaxScore = s.MaxScore,
                MinScore = s.MinScore,
                CreateAt = s.CreateAt,
                ResultDescription = s.ResultDescription,
                ModifiedAt = s.ModifiedAt,
                Id = s.Id,
                IsDelete = s.IsDeleted
            }).ToListAsync();
        }

        public async Task RemoveSurveyResult(Guid id)
        {
            var surveyResult = await _surveyResultsRepository.GetById(id);
            if(surveyResult is null || surveyResult.IsDeleted)
            {
                throw new Exception("Không tìm thấy kết quả khảo sát.");
            }
            surveyResult.IsDeleted = true;
            surveyResult.ModifiedAt = DateTime.Now;
            await _surveyResultsRepository.Update(surveyResult);
            await _surveyAnswerRepository.SaveChangesAsync();
        }

        public async Task UpdateSurveyResult(Guid id, SurveyResultRequest.UpdateSurveyResultRequest model)
        {
            var surveyResult = await _surveyResultsRepository.GetById(id);
            if (surveyResult is null)
            {
                throw new Exception("Không tìm thấy kết quả khảo sát.");
            }
            surveyResult.IsDeleted = model.IsDelete.HasValue ? model.IsDelete.Value : surveyResult.IsDeleted;
            surveyResult.ModifiedAt = DateTime.Now;
            surveyResult.ResultDescription = model.ResultDescription;
            surveyResult.MaxScore = model.MaxScore;
            surveyResult.MinScore = model.MinScore;
            await _surveyResultsRepository.Update(surveyResult);
            await _surveyAnswerRepository.SaveChangesAsync();
        }

    }
}
