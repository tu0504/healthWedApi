using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class AccountSurveyService : IAccountSurveyService
    {
        private readonly IBaseRepository<Survey, Guid> _surveyRepository;
        private readonly IBaseRepository<AccountSurvey, Guid> _accountSurveyRepository;

        public AccountSurveyService(IBaseRepository<Survey, Guid> surveyRepository, IBaseRepository<AccountSurvey, Guid> accountSurveyRepository)
        {
            _surveyRepository = surveyRepository;
            _accountSurveyRepository = accountSurveyRepository;
        }

        public async Task AddAccountSurvey(AccountSurveyRequest.CreateAccountSurveyModel model)
        {
            var accountSurvey = new AccountSurvey
            {
                Id = Guid.NewGuid(),
                AccountId = model.AccountId,
                SurveyId = model.SurveyId,
                CreateAt = DateTime.Now,
            };
            await _accountSurveyRepository.Add(accountSurvey);
            await _surveyRepository.SaveChangesAsync();
        }

        public async Task<AccountSurveyResponse.GetAccountSurveysModel?> GetAccountSurveyById(Guid id)
        {
            var accountSurvey = await _accountSurveyRepository.GetById(id);
            if (accountSurvey is null || accountSurvey.IsDeleted)
            {
                throw new Exception("Không tìm thấy khảo sát.");
            }
            return new AccountSurveyResponse.GetAccountSurveysModel
            {
                CreateAt = accountSurvey.CreateAt,
                AccountId = accountSurvey.AccountId,
                ModifiedAt = accountSurvey.ModifiedAt,
                SurveyId = accountSurvey.SurveyId,
                Id = id,
                IsDelete = accountSurvey.IsDeleted,
            };
        }

        public async Task<List<AccountSurveyResponse.GetAccountSurveysModel>> GetAccountSurveys()
        {
            var accountSurveys = _accountSurveyRepository.GetAll().Where(s => !s.IsDeleted)
                .Select(s => new AccountSurveyResponse.GetAccountSurveysModel
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    CreateAt = s.CreateAt,
                    ModifiedAt = s.ModifiedAt,
                    SurveyId = s.SurveyId,
                    IsDelete = s.IsDeleted
                }).ToList();
            return accountSurveys;
        }

        public async Task<List<AccountSurveyResponse.GetAccountSurveysModel>> GetAccountSurveysByAccountId(Guid accountId)
        {
            var accountSurveys = _accountSurveyRepository.GetAll().Where(s => s.AccountId == accountId && s.IsDeleted == false)
                 .Select(s => new AccountSurveyResponse.GetAccountSurveysModel
                 {
                     Id = s.Id,
                     AccountId = s.AccountId,
                     CreateAt = s.CreateAt,
                     ModifiedAt = s.ModifiedAt,
                     SurveyId = s.SurveyId,
                     IsDelete = s.IsDeleted
                 }).ToList();
            if(!accountSurveys.Any())
            {
                throw new Exception("Không tìm thấy khảo sát.");
            }
            return accountSurveys;
        }

        public async Task<List<AccountSurveyResponse.GetAccountSurveysModel>> GetAccountSurveysBySurveyId(Guid surveyId)
        {
            var accountSurveys = _accountSurveyRepository.GetAll().Where(s => s.SurveyId == surveyId && s.IsDeleted == false)
                 .Select(s => new AccountSurveyResponse.GetAccountSurveysModel
                 {
                     Id = s.Id,
                     AccountId = s.AccountId,
                     CreateAt = s.CreateAt,
                     ModifiedAt = s.ModifiedAt,
                     SurveyId = s.SurveyId,
                     IsDelete = s.IsDeleted
                 }).ToList();
            if (!accountSurveys.Any())
            {
                throw new Exception("Không tìm thấy khảo sát.");
            }
            return accountSurveys;
        }

        public async Task RemoveAccountSurvey(Guid id)
        {
            var accountSurvey = await _accountSurveyRepository.GetById(id);
            if (accountSurvey is null)
            {
                throw new Exception("Không tìm thấy khảo sát.");
            }
            accountSurvey.IsDeleted = true;
            accountSurvey.ModifiedAt = DateTime.Now;
            await _accountSurveyRepository.Update(accountSurvey);
            await _accountSurveyRepository.SaveChangesAsync();
        }
    }
}
