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
    public class OrderService : IOrderService
    {

        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;
        private readonly IBaseRepository<SubscriptionData, Guid> _subscriptionDataRepository;

        public OrderService(IBaseRepository<Order, Guid> orderRepository, IBaseRepository<Account, Guid> accountRepository, IBaseRepository<SubscriptionData, Guid> subscriptionDataRepository)
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _subscriptionDataRepository = subscriptionDataRepository;
        }
        public async Task CreateOrder(OrderRequest.CreateOrderModel model)
        {
            var subscription = await _subscriptionDataRepository.GetAll().FirstOrDefaultAsync(s => s.Id == model.SubscriptionId);
            var account = await _accountRepository.GetAll().FirstOrDefaultAsync(a => a.Id == model.AccountId);

            if (subscription == null || account == null)
            {
                throw new Exception("Subscription or Account not found.");
            }

            try
            {
                // Create Order object
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    SubscriptionDataId = subscription.Id,
                    AccountId = account.Id,
                    Quantity = model.Quantity,
                    IsDeleted = model.IsDelete,
                    CreateAt = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                // Save Order to database
                await _orderRepository.Add(order);
                await _orderRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<OrderResponse.GetOrderDetailsModel?> GetOrderDetails(Guid orderId)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.SubscriptionData)
                .Include(o => o.Accounts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.IsDeleted)
            {
                return null;
            }

            return new OrderResponse.GetOrderDetailsModel(
                order.Id,
                order.SubscriptionData.SubscriptionName,
                order.SubscriptionData.Description,
                (float)order.SubscriptionData.Price,
                order.Quantity,
                order.Accounts.Fullname,
                order.Accounts.Email,
                order.CreateAt,
                order.ModifiedAt,
                order.IsActive ? true : false
            );
        }
        public async Task<OrderResponse.GetOrderDetailsModel?> GetOrderDetailsDeleted(Guid orderId)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.SubscriptionData)
                .Include(o => o.Accounts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            return new OrderResponse.GetOrderDetailsModel(
                order.Id,
                order.SubscriptionData.SubscriptionName,
                order.SubscriptionData.Description,
                (float)order.SubscriptionData.Price,
                order.Quantity,
                order.Accounts.Fullname,
                order.Accounts.Email,
                order.CreateAt,
                order.ModifiedAt,
                order.IsActive ? true : false
            );
        }

        public async Task<List<OrderResponse.GetOrderDetailsModel>> GetOrders()
        {
            return await _orderRepository.GetAll()
            .Where(o => !o.IsDeleted) // Exclude deleted subscriptions
            .AsNoTracking()
            .Select(o => new OrderResponse.GetOrderDetailsModel(
                o.Id,
                o.SubscriptionData.SubscriptionName,
                o.SubscriptionData.Description,
                (float)o.SubscriptionData.Price,
                o.Quantity,
                o.Accounts.Fullname,
                o.Accounts.Email,
                o.CreateAt,
                o.ModifiedAt,
                o.IsActive ? true : false
            ))
            .ToListAsync();
        }
        public async Task CancelOrder(Guid orderId, OrderRequest.UpdateOrderModel model)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            order.IsActive = false; // Mark order as inactive (canceled)
            order.ModifiedAt = DateTimeOffset.UtcNow; // Track modification time

            await _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task RemoveOrder(Guid Id)
        {
            var order = await _orderRepository.GetById(Id);
            if (order is null || order.IsDeleted)
            {
                return;
            }
            order.IsDeleted = true;
            await _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
