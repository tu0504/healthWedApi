using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEALTH_SUPPORT.Repositories.Repository;
using Microsoft.EntityFrameworkCore;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IBaseRepository<SubscriptionData, Guid> _subscriptionRepository;
        private readonly IBaseRepository<Category, Guid> _categoryRepository;
        private readonly IBaseRepository<Psychologist, Guid> _psychologistRepository;

        public SubscriptionService(
            IBaseRepository<SubscriptionData, Guid> subscriptionRepository,
            IBaseRepository<Category, Guid> categoryRepository,
<<<<<<< HEAD
            IBaseRepository<Psychologist, Guid> psychologistRepository,
            IBaseRepository<Account, Guid> accountRepository)
=======
            IBaseRepository<Psychologist, Guid> psychologistRepository)
>>>>>>> bb3289919873a3575303b86a77b526a77031babb
        {
            _subscriptionRepository = subscriptionRepository;
            _categoryRepository = categoryRepository;
            _psychologistRepository = psychologistRepository;
        }

        public async Task AddSubscription(SubscriptionRequest.CreateSubscriptionModel model)
        {
            //Kiểm tra xem subscription đã tồn tại chưa
            var existingSubscription = await _subscriptionRepository.GetAll().AnyAsync(s => s.SubscriptionName == model.SubscriptionName);
            if (existingSubscription)
            {
                throw new Exception("Tên chương trình đã tồn tại!");
            }
            //Kiểm tra xem category có tồn tại không
            var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(c => c.CategoryName.ToLower() == model.CategoryName.ToLower());
            if (category == null)
            {
                throw new Exception("Vui lòng chọn loại chương trình!");
            }
            var psychologist = await _psychologistRepository.GetAll().FirstOrDefaultAsync(p => p.Name == model.PsychologistName);
            if (psychologist == null)
            {
                throw new Exception("Vui lòng chọn tên chuyên gia");
            }
            try
            {
                var subscription = new SubscriptionData()
                {
                    Id = Guid.NewGuid(),
                    SubscriptionName = model.SubscriptionName,
                    Description = model.Description,
                    Price = model.Price,
                    Duration = model.Duration,
                    CategoryId = category.Id,
                    PsychologistId = psychologist.Id,
                    Purpose = model.Purpose,
                    Criteria = model.Criteria,
                    FocusGroup = model.FocusGroup,
                    AssessmentTool = model.AssessmentTool,
                    CreateAt = DateTimeOffset.UtcNow
                };
                await _subscriptionRepository.Add(subscription);
                await _subscriptionRepository.SaveChangesAsync();
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<SubscriptionResponse.GetSubscriptionsModel?> GetSubscriptionById(Guid id)
        {
            var subscription = await _subscriptionRepository.GetAll()
                .Include(s => s.Category)
                .Include(s => s.Psychologists)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null || subscription.IsDeleted)
            {
                return null;
            }

            return new SubscriptionResponse.GetSubscriptionsModel(
                subscription.Id,
                subscription.SubscriptionName,
                subscription.Description,
                (float)subscription.Price,
                subscription.Duration,
                subscription.Category?.CategoryName ?? "Unknown",
                subscription.Psychologists?.Name ?? "Unknown",
                subscription.Purpose,
                subscription.Criteria,
                subscription.FocusGroup,
                subscription.AssessmentTool
            );
        }

        public async Task<List<SubscriptionResponse.GetSubscriptionsModel>> GetSubscriptions()
        {
            return await _subscriptionRepository.GetAll()
            .Where(s => !s.IsDeleted) // Exclude deleted subscriptions
            .AsNoTracking()
            .Select(s => new SubscriptionResponse.GetSubscriptionsModel(
                s.Id,
                s.SubscriptionName,
                s.Description,
                (float)s.Price,
                s.Duration,
                s.Category != null ? s.Category.CategoryName : "Unknown",
                s.Psychologists != null ? s.Psychologists.Name : "Unknown",
                s.Purpose,
                s.Criteria,
                s.FocusGroup,
                s.AssessmentTool
            ))
            .ToListAsync();
        }

        public async Task RemoveSubscription(Guid id)
        {
            var subscription = await _subscriptionRepository.GetById(id);
            if (subscription == null)
            {
                throw new InvalidOperationException("Subscription not found.");
            }

            subscription.IsDeleted = true;
            subscription.ModifiedAt = DateTimeOffset.UtcNow; // Track modification

            await _subscriptionRepository.Update(subscription);
            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task UpdateSubscription(Guid id, SubscriptionRequest.UpdateSubscriptionModel model)
        {
            try
            {
                // Find the subscription by Id
                var existedSubscription = await _subscriptionRepository.GetById(id);

                if (existedSubscription == null)
                {
                    throw new Exception("Subscription not found.");
                }

                // Update only if new values are provided
                existedSubscription.Description = string.IsNullOrWhiteSpace(model.Description) ? existedSubscription.Description : model.Description;
                existedSubscription.Price = model.Price > 0 ? model.Price : existedSubscription.Price;
                existedSubscription.Duration = model.Duration > 0 ? model.Duration : existedSubscription.Duration;
                existedSubscription.PsychologistId = model.PsychologistId != Guid.Empty ? model.PsychologistId : existedSubscription.PsychologistId;
                existedSubscription.Purpose = string.IsNullOrWhiteSpace(model.Purpose) ? existedSubscription.Purpose : model.Purpose;
                existedSubscription.Criteria = string.IsNullOrWhiteSpace(model.Criteria) ? existedSubscription.Criteria : model.Criteria;
                existedSubscription.FocusGroup = string.IsNullOrWhiteSpace(model.FocusGroup) ? existedSubscription.FocusGroup : model.FocusGroup;
                existedSubscription.AssessmentTool = string.IsNullOrWhiteSpace(model.AssessmentTool) ? existedSubscription.AssessmentTool : model.AssessmentTool;

                existedSubscription.ModifiedAt = DateTimeOffset.UtcNow;

                // Save changes to database
                await _subscriptionRepository.Update(existedSubscription);
                await _subscriptionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
