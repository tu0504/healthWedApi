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
    public class SurveyQuestionService : ISurveyQuestionService
    {
        private readonly IBaseRepository<Survey, Guid> _surveyRepository;
        private readonly IBaseRepository<SurveyQuestion, Guid> _surveyQuestionRepository;
        private readonly IBaseRepository<SurveyAnswer, Guid> _surveyAnswerRepository;
        private readonly IBaseRepository<SurveyQuestionSurvey, Guid> _surveyQuestionSurveyRepository;
        private readonly ISurveyAnswerService _surveyAnswerService;
        private readonly ISurveyQuestionSurveyService _surveyQuestionSurveyService;
        private readonly ISurveyQuestionAnswerService _surveyQuestionAnswerService;

        public SurveyQuestionService(IBaseRepository<Survey, Guid> surveyRepository, IBaseRepository<SurveyQuestion, Guid> surveyQuestionRepository, 
            IBaseRepository<SurveyAnswer, Guid> surveyAnswerRepository, IBaseRepository<SurveyQuestionSurvey, Guid> surveyQuestionSurveyRepository, 
            ISurveyAnswerService surveyAnswerService, ISurveyQuestionSurveyService surveyQuestionSurveyService, ISurveyQuestionAnswerService surveyQuestionAnswerService)
        {
            _surveyRepository = surveyRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _surveyAnswerRepository = surveyAnswerRepository;
            _surveyQuestionSurveyRepository = surveyQuestionSurveyRepository;
            _surveyAnswerService = surveyAnswerService;
            _surveyQuestionSurveyService = surveyQuestionSurveyService;
            _surveyQuestionAnswerService = surveyQuestionAnswerService;
        }

        public async Task AddSurveyQuestionForSurvey(Guid surveyID, List<SurveyQuestionRequest.CreateSurveyQuestionRequest> model)
        {
            var survey = await _surveyRepository.GetById(surveyID);
            if (survey is null)
            {
                throw new Exception("Không tìm thấy bảng khảo sát.");
            }
            //var surveyAnswers = new List<SurveyAnswer>();
            var surveyQuestionSurveyModel = new SurveyQuestionSurveyRequest.AddSurveyQuestionSurvey();
            var surveyQuestionAnswerModel = new SurveyQuestionAnswerRequest.AddSurveyQuestionAnswer();

            foreach (var surveyQuestion in model)
            {
                var question = new SurveyQuestion
                {
                    CreateAt = DateTime.Now,
                    SurveyTypeId = survey.SurveyTypeId,
                    ContentQ = surveyQuestion.ContentQ
                };

                await _surveyQuestionRepository.Add(question);
                surveyQuestionSurveyModel.SurveyQuestionsId = question.Id;
                surveyQuestionSurveyModel.SurveysId = surveyID;
                await _surveyQuestionSurveyService.AddSurveyQuestionSurvey(surveyQuestionSurveyModel);

                var surveyAnswerByQuestion = surveyQuestion.AnswersList?.Select(answer => new SurveyAnswer
                {
                    Content = answer.Content,
                    Point = answer.Point,
                    CreateAt = DateTime.Now
                }).ToList() ?? new List<SurveyAnswer>();

                if (surveyAnswerByQuestion.Any())
                {
                    foreach (var answer in surveyAnswerByQuestion)
                    {
                        await _surveyAnswerRepository.Add(answer);
                        surveyQuestionAnswerModel.SurveyQuestionsId = question.Id;
                        surveyQuestionAnswerModel.SurveyAnswersId = answer.Id;
                        await _surveyQuestionAnswerService.AddSurveyQuestionAnswer(surveyQuestionAnswerModel);
                    }
                    await _surveyAnswerRepository.SaveChangesAsync();
                }
            }
            await _surveyQuestionRepository.SaveChangesAsync();
            return;
        }

        public async Task<SurveyQuestionResponse.GetSurveyQuestionModel?> GetSurveyQuestionById(Guid id)
        {
            var question = await _surveyQuestionRepository.GetById(id);
            if (question is null || question.IsDeleted)
            {
                throw new Exception("Không tìm thấy câu hỏi.");
            }
            return new SurveyQuestionResponse.GetSurveyQuestionModel
            {
                Id = id,
                ContentQ = question.ContentQ,
                CreateAt = question.CreateAt,
                ModifiedAt = question.ModifiedAt,
                IsDelete = question.IsDeleted
            };
        }

        public async Task<SurveyQuestionResponse.GetSurveyQuestionModel?> GetByIdDeleted(Guid id)
        {
            var question = await _surveyQuestionRepository.GetById(id);
            if (question is null)
            {
                throw new Exception("Không tìm thấy câu hỏi.");
            }
            return new SurveyQuestionResponse.GetSurveyQuestionModel
            {
                Id = id,
                ContentQ = question.ContentQ,
                CreateAt = question.CreateAt,
                ModifiedAt = question.ModifiedAt,
                IsDelete = question.IsDeleted
            };
        }

        public async Task<List<SurveyQuestionResponse.GetSurveyQuestionModel>> GetSurveyQuestions()
        {
            return await _surveyQuestionRepository.GetAll()
                .Where(q => !q.IsDeleted).AsNoTracking()
                .Select(q => new SurveyQuestionResponse.GetSurveyQuestionModel
                {
                    Id = q.Id,
                    ContentQ = q.ContentQ,
                    CreateAt = q.CreateAt,
                    ModifiedAt = q.ModifiedAt,
                    IsDelete = q.IsDeleted
                })
                .ToListAsync(); 
        }

        public async Task<List<SurveyQuestionResponse.GetSurveyQuestionModel>> GetSurveyQuestionsForSurvey(Guid surveyId)
        {
            // Lấy danh sách SurveyQuestion từ bảng trung gian SurveyQuestionSurvey
            var questionIdList = await _surveyQuestionSurveyRepository.GetAll().Where(s => s.SurveysId == surveyId).Select(s => s.SurveyQuestionsId).ToListAsync();
            if (!questionIdList.Any())
            {
                return new List<SurveyQuestionResponse.GetSurveyQuestionModel>();
            }
            var questionList = await _surveyQuestionRepository
                .GetAll()
                .Include(s => s.SurveyQuestionAnswers)
                .ThenInclude(s => s.SurveyAnswer)
                .Where(s => questionIdList.Contains(s.Id))
                .Select(q => new SurveyQuestionResponse.GetSurveyQuestionModel
                {
                    Id = q.Id,
                    SurveyId = surveyId, // SurveyQuestion doesn't have SurveyId directly
                    ContentQ = q.ContentQ,
                    CreateAt = q.CreateAt,
                    ModifiedAt = q.ModifiedAt,
                    IsDelete = q.IsDeleted,
                    AnswerList = q.SurveyQuestionAnswers.Select(sqs => new SurveyAnswerResponse.GetSurveyAnswerModel
                    {
                        Id = sqs.SurveyAnswer.Id,  // Assuming SurveyAnswer is accessible this way
                        Content = sqs.SurveyAnswer.Content,
                        Point = sqs.SurveyAnswer.Point,
                        QuestionId = q.Id,
                        IsDelete = sqs.SurveyAnswer.IsDeleted
                    }).ToList()  // Convert to List
                })
                .ToListAsync();

            return questionList;
        }

        public async Task RemoveSurveyQuestion(Guid id)
        {
            var question = await _surveyQuestionRepository.GetById(id);
            if (question is null || question.IsDeleted)
            {
                throw new Exception("Không tìm thấy câu hỏi.");
            }
            question.IsDeleted = true;
            question.ModifiedAt = DateTime.Now;
            await _surveyQuestionRepository.Update(question);
            await _surveyQuestionRepository.SaveChangesAsync();
        }

        public async Task UpdateSurveyQuestion(Guid id, SurveyQuestionRequest.UpdateSurveyQuestionRequest model)
        {
            var question = await _surveyQuestionRepository.GetById(id);
            if (question is null)
            {
                throw new Exception("Không tìm thấy câu hỏi.");
            }
            question.ContentQ = model.ContentQ;
            question.IsDeleted = model.IsDelete.HasValue ? model.IsDelete.Value : question.IsDeleted;
            question.ModifiedAt = DateTime.Now;
            await _surveyQuestionRepository.Update(question);
            await _surveyQuestionRepository.SaveChangesAsync();
        }
    }
}
