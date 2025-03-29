using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderResponse.GetOrderDetailsModel?> GetOrderDetails(Guid orderId);
        Task<OrderResponse.GetOrderDetailsModel?> GetOrderDetailsDeleted(Guid orderId);
        Task<List<OrderResponse.GetOrderDetailsModel>> GetOrders();
        Task CreateOrder(OrderRequest.CreateOrderModel model);
        Task UpdateOrder(Guid id, OrderRequest.UpdateOrderModel model);
        Task RemoveOrder(Guid id);
        Task<string> CreateOrderWithVnpayPayment(OrderRequest.CreateOrderModel model);
        Task UpdatePaymentStatus(Guid orderId, bool isSuccessful);
        bool VerifyVnpayResponse(Dictionary<string, string> vnpResponse);
    }
}
