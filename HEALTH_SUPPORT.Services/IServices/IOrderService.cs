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
        Task CreateOrder(OrderRequest.CreateOrderModel model);

        Task<OrderResponse.GetOrderDetailsModel?> GetOrderDetails(Guid orderId);
    }
}
