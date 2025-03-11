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
        private readonly IBaseRepository<SubscriptionProgress, Guid> _progressRepository;
        private readonly IBaseRepository<Order, Guid> _orderRepository;

        public SubscriptionProgressService(IBaseRepository<SubscriptionProgress, Guid> progressRepository, IBaseRepository<Order, Guid> orderRepository)
        {
            _progressRepository = progressRepository;
            _orderRepository = orderRepository;
        }
        public async Task CreateSubscriptionProgress(SubscriptionProgressRequest.CreateSubscriptionProgressModel model)
        {
            var order = await _orderRepository.GetAll().Include(o => o.SubscriptionData).FirstOrDefaultAsync(o => o.Id == model.OrderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            var startDate = order.CreateAt;
            var endDate = startDate.AddDays(order.SubscriptionData.Duration);

            try
            {
                var subscriptionProgress = new SubscriptionProgress
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    StartDate = startDate,
                    EndDate = endDate
                };

                await _progressRepository.Add(subscriptionProgress);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<SubscriptionProgressResponse.GetSubscriptionProgressModel> GetSubscriptionProgressById(Guid progressId)
        {
            var subscriptionProgress = await _progressRepository.GetAll()
                .Include(sp => sp.Order)
                    .ThenInclude(o => o.Accounts)
                .Include(sp => sp.Order)
                    .ThenInclude(o => o.SubscriptionData)
                .FirstOrDefaultAsync(sp => sp.Id == progressId);

            if (subscriptionProgress == null || subscriptionProgress.IsDeleted)
            {
                return null;
            }

            return new SubscriptionProgressResponse.GetSubscriptionProgressModel(
                subscriptionProgress.Id,
                subscriptionProgress.Order.Accounts?.UseName ?? "N/A",
                subscriptionProgress.Order.SubscriptionData?.SubscriptionName ?? "N/A",
                subscriptionProgress.Order.SubscriptionData?.Description ?? "N/A",
                subscriptionProgress.Order.SubscriptionData?.Price ?? 0f,
                subscriptionProgress.StartDate,
                subscriptionProgress.EndDate
            );
        }
        public async Task<List<SubscriptionProgressResponse.GetSubscriptionProgressModel>> GetAllSubscriptionProgress()
        {
            return await _progressRepository.GetAll()
                .Where(sp => !sp.IsDeleted) // Exclude deleted progress records
                .AsNoTracking()
                .Select(sp => new SubscriptionProgressResponse.GetSubscriptionProgressModel(
                    sp.Id,
                    sp.Order.Accounts.UseName,
                    sp.Order.SubscriptionData.SubscriptionName,
                    sp.Order.SubscriptionData.Description,
                    (float)sp.Order.SubscriptionData.Price,
                    sp.StartDate,
                    sp.EndDate
                ))
                .ToListAsync();
        }

    }
}
