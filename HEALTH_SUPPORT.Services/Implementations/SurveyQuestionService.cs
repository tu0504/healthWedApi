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
        private readonly ISurveyAnswerService _surveyAnswerService;
        public SurveyQuestionService(IBaseRepository<Survey, Guid> surveyRepository, IBaseRepository<SurveyQuestion, Guid> surveyQuestionRepository, ISurveyAnswerService surveyAnswerService)
        {
            _surveyRepository = surveyRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _surveyAnswerService = surveyAnswerService;
        }

        public async Task AddSurveyQuestionForSurvey(Guid surveyID, List<SurveyQuestionRequest.CreateSurveyQuestionRequest> model)
        {
            var survey = await _surveyRepository.GetById(surveyID);
            if (survey is null)
            {
                throw new Exception("Không tìm thấy bảng khảo sát.");
            }
<<<<<<< HEAD
            var surveyQuestions = new List<SurveyQuestion>();
=======
>>>>>>> develop
            foreach (var surveyQuestion in model)
            {
                var question = new SurveyQuestion
                {
                    CreateAt = DateTime.Now,
<<<<<<< HEAD
                    SurveyTypeId = survey.SurveyTypeId,
                    ContentQ = surveyQuestion.ContentQ,
                    SurveyAnswers = surveyQuestion.AnswersList?.Select(answer => new SurveyAnswer
                    {
                        Content = answer.Content,
                        Point = answer.Point,
                        CreateAt = DateTime.Now
                    }).ToList() ?? new List<SurveyAnswer>()
                };

                surveyQuestions.Add(question);
            }

            foreach (var question in surveyQuestions)
            {
                await _surveyQuestionRepository.Add(question);
            }
            await _surveyQuestionRepository.SaveChangesAsync();
=======
                    Options = surveyQuestion.Options,
                    SurveyId = surveyID,
                    SurveyTypeId = survey.SurveyTypeId,
                    Validity = surveyQuestion.Validity,
                    ContentQ = surveyQuestion.ContentQ
                };
                await _surveyQuestionRepository.Add(question);
                if(surveyQuestion.AnswersList.Any())
                {
                    surveyQuestion.AnswersList.ForEach(answer => answer.QuestionId = question.Id);
                }
            }
            await _surveyQuestionRepository.SaveChangesAsync();
            var answerList = model
                .SelectMany(surveyQuestion => surveyQuestion.AnswersList)
                .ToList();

            if (answerList.Any())
            {
                await _surveyAnswerService.AddSurveyAnswerForSurveyQuestion(answerList);
            }
>>>>>>> develop
        }

        public async Task<SurveyQuestionResponse.GetSurveyQuestionModel?> GetSurveyQuestionById(Guid id)
        {
            var question = await _surveyQuestionRepository.GetById(id);
<<<<<<< HEAD
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
=======
>>>>>>> develop
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
<<<<<<< HEAD
=======
                Options = question.Options,
                Validity = question.Validity,
>>>>>>> develop
                IsDelete = question.IsDeleted
            };
        }

<<<<<<< HEAD
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
=======
        public Task<List<SurveyQuestionResponse.GetSurveyQuestionModel>> GetSurveyQuestions()
        {
            throw new NotImplementedException();
>>>>>>> develop
        }

        public async Task<List<SurveyQuestionResponse.GetSurveyQuestionModel>> GetSurveyQuestionsForSurvey(Guid surveyId)
        {
<<<<<<< HEAD
            // Lấy danh sách SurveyQuestion từ bảng trung gian SurveyQuestionSurvey
            var questionList = await _surveyQuestionRepository.GetAll()
                .Where(q => q.Surveys.Any(s => s.Id == surveyId)) // Kiểm tra xem SurveyQuestion thuộc Survey nào
                .Select(q => new SurveyQuestionResponse.GetSurveyQuestionModel
                {
                    Id = q.Id,
                    SurveyId = surveyId, // SurveyQuestion không có trực tiếp SurveyId
                    ContentQ = q.ContentQ,
                    CreateAt = q.CreateAt,
                    ModifiedAt = q.ModifiedAt,
                    IsDelete = q.IsDeleted
                })
                .ToListAsync();

            // Lấy danh sách Answer tương ứng với QuestionId trong questionList
            var questionIds = questionList.Select(q => q.Id).ToList();
            var answerList = await _surveyAnswerService.GetSurveyAnswerForQuestion(questionIds);

            // Gán danh sách câu trả lời vào từng câu hỏi
            foreach (var question in questionList)
            {
                question.AnswerList = answerList.Where(a => a.QuestionId == question.Id).ToList();
=======
            var questionList = await _surveyQuestionRepository.GetAll().Where(s => s.SurveyId == surveyId).Select(s => new SurveyQuestionResponse.GetSurveyQuestionModel
            {
                Id= s.Id,
                SurveyId = s.SurveyId,
                ContentQ = s.ContentQ,
                CreateAt = s.CreateAt,
                Options= s.Options,
                ModifiedAt= s.ModifiedAt,
                Validity= s.Validity,
                IsDelete= s.IsDeleted
            }).ToListAsync();

            var answerList = await _surveyAnswerService.GetSurveyAnswerForQuestion(questionList.Select(s => s.Id).ToList());

            foreach (var item in questionList)
            {
                var answer = answerList.Where(s => s.QuestionId == item.Id).ToList();
                if (answer.Any())
                {
                    item.AnswerList.AddRange(answer);
                }
>>>>>>> develop
            }

            return questionList;
        }

        public async Task RemoveSurveyQuestion(Guid id)
        {
            var question = await _surveyQuestionRepository.GetById(id);
<<<<<<< HEAD
            if (question is null || question.IsDeleted)
=======
            if (question is null)
>>>>>>> develop
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
<<<<<<< HEAD
=======
            question.Options = model.Options;
            question.Validity = model.Validity;
>>>>>>> develop
            await _surveyQuestionRepository.Update(question);
            await _surveyQuestionRepository.SaveChangesAsync();
        }
    }
}
