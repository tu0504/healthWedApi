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
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<SubscriptionProgress, Guid> _subscriptionProgressRepository;
        private readonly IBaseRepository<Psychologist, Guid> _psychologistRepository;

        public SubscriptionService(
            IBaseRepository<SubscriptionData, Guid> subscriptionRepository,
            IBaseRepository<Category, Guid> categoryRepository,
            IBaseRepository<Order, Guid> orderRepository,
            IBaseRepository<SubscriptionProgress, Guid> subscriptionProgressRepository,
            IBaseRepository<Psychologist, Guid> psychologistRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _subscriptionProgressRepository = subscriptionProgressRepository;
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
            var subscription = await _subscriptionRepository.GetAll().Include(s => s.Category).Include(s => s.Psychologists).FirstOrDefaultAsync(s => s.Id == id);

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
                subscription.Psychologists?.Name ?? "Unknown"
            );
        }

        public async Task<List<SubscriptionResponse.GetSubscriptionsModel>> GetSubscriptions()
        {
            //return await _subscriptionRepository.GetAll()
            //    .Where(s => !s.IsDeleted)
            //    .AsNoTracking()
            //    .Select(s => new SubscriptionResponse.GetSubscriptionsModel(
            //        s.Id,
            //        s.SubscriptionName,
            //        s.Description,
            //        s.Price,
            //        s.Duration,
            //        s.CategoryId,
            //        s.Category.CategoryName,
            //        "No progress" // Default value since GetSubscriptions() does not track progress
            //    ))
            //    .ToListAsync();
            return await Task.FromResult(new List<SubscriptionResponse.GetSubscriptionsModel>());
        }

        public async Task RemoveSubscription(Guid id)
        {
            //var subscription = await _subscriptionRepository.GetById(id);
            //if (subscription == null)
            //{
            //    throw new InvalidOperationException("Subscription not found");
            //}

            //subscription.IsDeleted = true;
            //subscription.ModifiedAt = DateTimeOffset.UtcNow;

            //await _subscriptionRepository.Update(subscription);
            //await _subscriptionRepository.SaveChangesAsync();
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

        public async Task RegisterSubscription(Guid accountId, SubscriptionRequest.RegisterSubscriptionModel model)
        {
            //var subscription = await _subscriptionRepository.GetById(model.SubscriptionId);
            //if (subscription == null || subscription.IsDeleted)
            //{
            //    throw new Exception("Invalid Subscription ID");
            //}

            //var order = new Order()
            //{
            //    Id = Guid.NewGuid(),
            //    SubscriptionDataId = model.SubscriptionId,
            //    AccountId = accountId,
            //    Quantity = model.Quantity,
            //    CreateAt = DateTimeOffset.UtcNow,
            //    IsActive = true
            //};

            //await _orderRepository.Add(order);
            //await _orderRepository.SaveChangesAsync(); // Save order first to get the ID

            //// Add Subscription Progress linked to Order
            //var progress = new SubscriptionProgress()
            //{
            //    Id = Guid.NewGuid(),
            //    OrderId = order.Id, // Now linked to Order instead of SubscriptionData
            //    Description = "Subscription started",
            //    StartDate = DateTimeOffset.UtcNow,
            //    EndDate = null
            //};

            //await _subscriptionProgressRepository.Add(progress);
            //await _subscriptionProgressRepository.SaveChangesAsync();
        }
        public async Task<List<SubscriptionResponse.GetSubscriptionsModel>> GetUserSubscriptions(Guid accountId)
        {
            //return await _orderRepository.GetAll()
            //    .Where(o => o.AccountId == accountId && o.IsActive)
            //    .Include(o => o.SubscriptionData)
            //    .Select(o => new SubscriptionResponse.GetSubscriptionsModel(
            //        o.SubscriptionData.Id,
            //        o.SubscriptionData.SubscriptionName,
            //        o.SubscriptionData.Description,
            //        o.SubscriptionData.Price,
            //        o.SubscriptionData.Duration,
            //        o.SubscriptionData.CategoryId,
            //        o.SubscriptionData.Category.CategoryName,
            //        null // Progress is not needed here
            //    ))
            //    .ToListAsync();
            return await Task.FromResult(new List<SubscriptionResponse.GetSubscriptionsModel>());
        }
        //public async Task<List<SubscriptionProgress>> GetSubscriptionProgressByOrderId(Guid orderId)
        //{
        //    var order = await _orderRepository.GetById(orderId);
        //    if (order == null)
        //    {
        //        throw new Exception("Order not found.");
        //    }

        //    return await _subscriptionProgressRepository.GetAll()
        //        .Where(sp => sp.OrderId == orderId)
        //        .OrderBy(sp => sp.StartDate)
        //        .ToListAsync();

        //}
        public async Task CancelSubscription(Guid orderId)
        {
            //var order = await _orderRepository.GetById(orderId);
            //if (order == null || !order.IsActive)
            //{
            //    throw new Exception("Subscription not found or already canceled");
            //}

            //order.IsActive = false;
            //await _orderRepository.Update(order);
            //await _orderRepository.SaveChangesAsync();

            //// Add a final SubscriptionProgress entry indicating cancellation
            //var progress = new SubscriptionProgress()
            //{
            //    Id = Guid.NewGuid(),
            //    OrderId = orderId,
            //    Description = "Subscription canceled",
            //    StartDate = DateTimeOffset.UtcNow,
            //    EndDate = DateTimeOffset.UtcNow
            //};

            //await _subscriptionProgressRepository.Add(progress);
            //await _subscriptionProgressRepository.SaveChangesAsync();
        }
    }
}
