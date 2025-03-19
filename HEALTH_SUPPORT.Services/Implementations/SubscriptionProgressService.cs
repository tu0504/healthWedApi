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
    public class SubscriptionProgressService : ISubscriptionProgressService
    {
        private readonly IBaseRepository<SubscriptionProgress, Guid> _subscriptionProgressRepository;
        private readonly IBaseRepository<SubscriptionData, Guid> _subscriptionDataRepository;
        public SubscriptionProgressService(IBaseRepository<SubscriptionProgress, Guid> subscriptionProgressRepository, IBaseRepository<SubscriptionData, Guid> subscriptionDataRepository)
        {
            _subscriptionProgressRepository = subscriptionProgressRepository;
            _subscriptionDataRepository = subscriptionDataRepository;
        }

        public async Task AddSubscriptionProgress(SubscriptionProgressRequest.CreateProgressModel model)
        {
            var subscription = await _subscriptionDataRepository.GetById(model.SubscriptionId);
            if (subscription == null)
            {
                throw new Exception("Subscription not found.");
            }
            try
            {
                var newProgress = new SubscriptionProgress
                {
                    Id = Guid.NewGuid(),
                    Section = model.Section,
                    Description = model.Description,
                    Date = model.Date,
                    SubscriptionId = model.SubscriptionId,
                    IsCompleted = model.IsCompleted,
                    CreateAt = DateTimeOffset.UtcNow
                };
                await _subscriptionProgressRepository.Add(newProgress);
                await _subscriptionProgressRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<SubscriptionProgressResponse.GetProgressModel>> GetSubscriptionProgress()
        {
            return await _subscriptionProgressRepository.GetAll()
                .Where(p => !p.IsDeleted).AsNoTracking()
                .Select(p => new SubscriptionProgressResponse.GetProgressModel
                {
                    Id = p.Id,
                    Section = (int)p.Section,
                    Description = p.Description,
                    Date = (int)p.Date,
                    SubscriptionName = p.SubscriptionDatas.SubscriptionName,
                    IsCompleted = p.IsCompleted,
                    CreateAt = p.CreateAt,
                    ModifiedAt = p.ModifiedAt
                })
                .ToListAsync();
        }

        public async Task<SubscriptionProgressResponse.GetProgressModel?> GetSubscriptionProgressById(Guid id)
        {
            var progress = await _subscriptionProgressRepository.GetById(id);
            if (progress is null || progress.IsDeleted)
            {
                return null;
            }
            return new SubscriptionProgressResponse.GetProgressModel
            {
                Id = progress.Id,
                Section = (int)progress.Section,
                Description = progress.Description,
                Date = (int)progress.Date,
                SubscriptionName = progress.SubscriptionDatas.SubscriptionName,
                IsCompleted = progress.IsCompleted,
                CreateAt = progress.CreateAt,
                ModifiedAt = progress.ModifiedAt
            };
        }

        public async Task<SubscriptionProgressResponse.GetProgressModel?> GetSubscriptionProgressByIdDeleted(Guid id)
        {
            var progress = await _subscriptionProgressRepository.GetById(id);
            if (progress is null)
            {
                return null;
            }
            return new SubscriptionProgressResponse.GetProgressModel
            {
                Id = progress.Id,
                Section = (int)progress.Section,
                Description = progress.Description,
                Date = (int)progress.Date,
                SubscriptionName = progress.SubscriptionDatas.SubscriptionName,
                IsCompleted = progress.IsCompleted,
                CreateAt = progress.CreateAt,
                ModifiedAt = progress.ModifiedAt
            };
        }

        public async Task RemoveSubscriptionProgress(Guid id)
        {
            var progress = await _subscriptionProgressRepository.GetById(id);
            if (progress == null)
            {
                throw new InvalidOperationException("Progress not found.");
            }
            progress.IsDeleted = true;
            progress.ModifiedAt = DateTimeOffset.UtcNow;

            await _subscriptionProgressRepository.Update(progress);
            await _subscriptionProgressRepository.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionProgress(Guid id, SubscriptionProgressRequest.UpdateProgressModel model)
        {
            var existedProgress = await _subscriptionProgressRepository.GetById(id);
            if (existedProgress is null)
            {
                return;
            }
            existedProgress.Section = model.Section > 0 ? model.Section : existedProgress.Section;
            existedProgress.Description = string.IsNullOrWhiteSpace(model.Description) ? existedProgress.Description : model.Description;
            existedProgress.Date = model.Date > 0 ? model.Date : existedProgress.Date;
            existedProgress.SubscriptionId = model.SubscriptionId != Guid.Empty ? model.SubscriptionId : existedProgress.SubscriptionId;
            existedProgress.IsCompleted = model.IsCompleted;
            existedProgress.IsDeleted = model.IsDeleted;

            existedProgress.ModifiedAt = DateTimeOffset.UtcNow;

            await _subscriptionProgressRepository.Update(existedProgress);
            await _subscriptionProgressRepository.SaveChangesAsync();
        }
    }
}
