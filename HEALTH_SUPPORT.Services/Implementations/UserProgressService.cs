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
    public class UserProgressService : IUserProgressService
    {
        private readonly IBaseRepository<UserProgress, Guid> _userProgressRepository;
        private readonly IBaseRepository<SubscriptionData, Guid> _subscriptionDataRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;
        public UserProgressService(IBaseRepository<UserProgress, Guid> userProgressRepository, IBaseRepository<SubscriptionData, Guid> subscriptionDataRepository, IBaseRepository<Account, Guid> accountRepository)
        {
            _userProgressRepository = userProgressRepository;
            _subscriptionDataRepository = subscriptionDataRepository;
            _accountRepository = accountRepository;
        }

        public async Task AddUserProgress(UserProgressRequest.CreateUserProgressModel model)
        {
            var subscription = await _subscriptionDataRepository.GetById(model.SubscriptionId);
            if (subscription == null)
            {
                throw new Exception("Subscription not found.");
            }
            var account = await _accountRepository.GetById(model.AccountId);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }
            try
            {
                var newUserProgress = new UserProgress
                {
                    Id = Guid.NewGuid(),
                    Section = model.Section,
                    Description = model.Description,
                    Date = model.Date,
                    SubscriptionId = subscription.Id,
                    AccountId = account.Id,
                    CreateAt = DateTimeOffset.UtcNow
                };
                await _userProgressRepository.Add(newUserProgress);
                await _userProgressRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<UserProgressResponse.GetUserProgressModel>> GetUserProgress()
        {
            return await _userProgressRepository.GetAll()
                .Where(up => !up.IsDeleted).AsNoTracking()
                .Select(up => new UserProgressResponse.GetUserProgressModel
                {
                    Id = up.Id,
                    Section = (int)up.Section,
                    Description = up.Description,
                    Date = (int)up.Date,
                    SubscriptionName = up.SubscriptionData.SubscriptionName,
                    AccountName = up.Accounts.UserName,
                    IsCompleted = up.IsCompleted,
                    CreateAt = up.CreateAt,
                    ModifiedAt = up.ModifiedAt
                })
                .ToListAsync();
        }

        public async Task<UserProgressResponse.GetUserProgressModel?> GetUserProgressById(Guid id)
        {
            var userProgress = await _userProgressRepository.GetAll()
                .Include(up => up.SubscriptionData)
                .Include(up => up.Accounts)
                .FirstOrDefaultAsync(up => up.Id == id);

            if (userProgress == null || userProgress.IsDeleted)
            {
                return null;
            }

            return new UserProgressResponse.GetUserProgressModel
            {
                Id = userProgress.Id,
                Section = (int)userProgress.Section,
                Description = userProgress.Description,
                Date = (int)userProgress.Date,
                SubscriptionName = userProgress.SubscriptionData.SubscriptionName,
                AccountName = userProgress.Accounts.UserName,
                IsCompleted = userProgress.IsCompleted,
                CreateAt = userProgress.CreateAt,
                ModifiedAt = userProgress.ModifiedAt
            };
        }

        public async Task<UserProgressResponse.GetUserProgressModel?> GetUserProgressByIdDeleted(Guid id)
        {
            var userProgress = await _userProgressRepository.GetAll()
                .Include(up => up.SubscriptionData)
                .Include(up => up.Accounts)
                .FirstOrDefaultAsync(up => up.Id == id);

            if (userProgress == null)
            {
                return null;
            }

            return new UserProgressResponse.GetUserProgressModel
            {
                Id = userProgress.Id,
                Section = (int)userProgress.Section,
                Description = userProgress.Description,
                Date = (int)userProgress.Date,
                SubscriptionName = userProgress.SubscriptionData.SubscriptionName,
                AccountName = userProgress.Accounts.UserName,
                IsCompleted = userProgress.IsCompleted,
                CreateAt = userProgress.CreateAt,
                ModifiedAt = userProgress.ModifiedAt
            };
        }

        public async Task RemoveUserProgress(Guid id)
        {
            var userProgress = await _userProgressRepository.GetById(id);
            if (userProgress == null)
            {
                throw new InvalidOperationException("User Progress not found.");
            }
            userProgress.IsDeleted = true;
            userProgress.ModifiedAt = DateTimeOffset.UtcNow;

            await _userProgressRepository.Update(userProgress);
            await _userProgressRepository.SaveChangesAsync();
        }

        public async Task UpdateUserProgress(Guid id, UserProgressRequest.UpdateUserProgressModel model)
        {
            var existedUserProgress = await _userProgressRepository.GetById(id);
            if (existedUserProgress is null)
            {
                return;
            }
            existedUserProgress.Section = model.Section > 0 ? model.Section : existedUserProgress.Section;
            existedUserProgress.Description = string.IsNullOrWhiteSpace(model.Description) ? existedUserProgress.Description : model.Description;
            existedUserProgress.Date = model.Date > 0 ? model.Date : existedUserProgress.Date;
            existedUserProgress.SubscriptionId = model.SubscriptionId != Guid.Empty ? model.SubscriptionId : existedUserProgress.SubscriptionId;
            existedUserProgress.AccountId = model.AccountId != Guid.Empty ? model.AccountId : existedUserProgress.AccountId;
            existedUserProgress.IsCompleted = model.IsCompleted;
            existedUserProgress.IsDeleted = model.IsDeleted;

            existedUserProgress.ModifiedAt = DateTimeOffset.UtcNow;

            await _userProgressRepository.Update(existedUserProgress);
            await _userProgressRepository.SaveChangesAsync();
        }
    }
}
