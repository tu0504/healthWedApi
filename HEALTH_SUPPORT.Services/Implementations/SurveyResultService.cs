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
<<<<<<< HEAD

        public SurveyResultService(IBaseRepository<SurveyResults, Guid> surveyResultsRepository, IBaseRepository<SurveyAnswer, Guid> surveyAnswerRepository, IBaseRepository<Survey, Guid> surveyRepository)
=======
        private readonly IBaseRepository<AccountSurvey, Guid> _accountSurveyRepository;

        public SurveyResultService(IBaseRepository<SurveyResults, Guid> surveyResultsRepository, IBaseRepository<SurveyAnswer, Guid> surveyAnswerRepository, IBaseRepository<Survey, Guid> surveyRepository, IBaseRepository<AccountSurvey, Guid> accountSurveyRepository)
>>>>>>> develop
        {
            _surveyResultsRepository = surveyResultsRepository;
            _surveyAnswerRepository = surveyAnswerRepository;
            _surveyRepository = surveyRepository;
<<<<<<< HEAD
        }

        public async Task AddSurveyResult(Guid surveyId,SurveyResultRequest.AddSurveyResultRequest model)
        {
            var survey = _surveyRepository.GetAll().Where(s => s.Id == surveyId && s.IsDeleted).FirstOrDefault();
            if (survey is null)
=======
            _accountSurveyRepository = accountSurveyRepository;
        }

        public async Task AddSurveyResult(Guid accountID, SurveyResultRequest.AddSurveyResultRequest model)
        {
            int score = 0;
            //Nếu truyền ID câu hỏi
            if(model.SurveyAnswerList.Any())
            {
                var answerList = _surveyAnswerRepository.GetAll().Where(s => model.SurveyAnswerList.Contains(s.Id)).ToList();
                if(answerList.Any())
                    score = answerList.Sum(s => s.Point);
            }
            //Nếu truyền điểm
            if(model.ScoreList.Any())
            {
                score = model.ScoreList.Sum(s => s);
            }

            var survey = await _surveyRepository.GetById(model.SurveyId);
            if(survey.MaxScore < score)
            {
                score = survey.MaxScore;
            }

            var accountSurvey = _accountSurveyRepository.GetAll().Where(s => s.AccountId == accountID && s.SurveyId == survey.Id).FirstOrDefault();
            if (accountSurvey is null)
>>>>>>> develop
            {
                throw new Exception("Không tìm thấy bảng khảo sát.");
            }

            var result = new SurveyResults
            {
<<<<<<< HEAD
                CreateAt = DateTime.Now,
                ResultDescription = model.ResultDescription,
                SurveyId = model.SurveyId,
                MaxScore = model.MaxScore,
                MinScore = model.MinScore,
=======
                AccountSurveyId = accountSurvey.Id,
                CreateAt = DateTime.Now,
                ResultDescription = model.ResultDescription,
                SurveyId = model.SurveyId,
                Score = score
>>>>>>> develop
            };
            await _surveyResultsRepository.Add(result);
            await _surveyAnswerRepository.SaveChangesAsync();
        }

        public async Task<SurveyResultResponse.GetSurveyResultModel?> GetSurveyResultById(Guid id)
        {
            var surveyResult = await _surveyResultsRepository.GetById(id);
<<<<<<< HEAD
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
=======
            if (surveyResult is null)
            {
                throw new Exception("Không tìm thấy kết quả khảo sát.");
>>>>>>> develop
            }
            return new SurveyResultResponse.GetSurveyResultModel
            {
                SurveyId = surveyResult.SurveyId,
<<<<<<< HEAD
                CreateAt = surveyResult.CreateAt,
                ResultDescription = surveyResult.ResultDescription,
                ModifiedAt = surveyResult.ModifiedAt,
                MaxScore = surveyResult.MaxScore,
                MinScore = surveyResult.MinScore,
=======
                AccountSurveyId = surveyResult.AccountSurveyId,
                CreateAt = surveyResult.CreateAt,
                ResultDescription = surveyResult.ResultDescription,
                ModifiedAt = surveyResult.ModifiedAt,
                Score = surveyResult.Score,
>>>>>>> develop
                Id = surveyResult.Id,
                IsDelete = surveyResult.IsDeleted
            };
        }

<<<<<<< HEAD
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
=======
        public Task<List<SurveyResultResponse.GetSurveyResultModel>> GetSurveyResults(Guid accountID)
        {
            var surveyResult = _surveyResultsRepository.GetAll().Where(s => s.AccountSurveyId == accountID).Select(s => new SurveyResultResponse.GetSurveyResultModel
            {
                SurveyId = s.SurveyId,
                AccountSurveyId = s.AccountSurveyId,
                CreateAt = s.CreateAt,
                ResultDescription = s.ResultDescription,
                ModifiedAt = s.ModifiedAt,
                Score = s.Score,
                Id = s.Id,
                IsDelete = s.IsDeleted
            }).ToListAsync();
            if (surveyResult is null)
            {
                throw new Exception("Không tìm thấy kết quả khảo sát.");
            }
            return surveyResult;
>>>>>>> develop
        }

        public async Task RemoveSurveyResult(Guid id)
        {
            var surveyResult = await _surveyResultsRepository.GetById(id);
<<<<<<< HEAD
            if(surveyResult is null || surveyResult.IsDeleted)
=======
            if(surveyResult is null)
>>>>>>> develop
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
<<<<<<< HEAD
            surveyResult.IsDeleted = model.IsDelete.HasValue ? model.IsDelete.Value : surveyResult.IsDeleted;
            surveyResult.ModifiedAt = DateTime.Now;
            surveyResult.ResultDescription = model.ResultDescription;
            surveyResult.MaxScore = model.MaxScore;
            surveyResult.MinScore = model.MinScore;
            await _surveyResultsRepository.Update(surveyResult);
            await _surveyAnswerRepository.SaveChangesAsync();
        }

=======
            surveyResult.ModifiedAt = DateTime.Now;
            surveyResult.ResultDescription = model.ResultDescription;
            surveyResult.Score = model.Score;
            await _surveyResultsRepository.Update(surveyResult);
            await _surveyAnswerRepository.SaveChangesAsync();
        }
>>>>>>> develop
    }
}
