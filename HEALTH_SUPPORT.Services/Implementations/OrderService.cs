using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class OrderService : IOrderService
    {

        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;
        private readonly IBaseRepository<SubscriptionData, Guid> _subscriptionDataRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // VNPay configuration
        private const string TmnCode = "S45E1GAV";
        private const string SecretKey = "5R93Q6N7UAJEJPCY10EM0Y5VD2TWUJDY";
        private const string VnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private const string ReturnUrlBase = "http://yourdomain.com/payment-result";

        public OrderService(IBaseRepository<Order, Guid> orderRepository, IBaseRepository<Account, Guid> accountRepository, IBaseRepository<SubscriptionData, Guid> subscriptionDataRepository, IHttpContextAccessor httpContextAccessor, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _subscriptionDataRepository = subscriptionDataRepository;
            _httpContextAccessor = httpContextAccessor;
            this._logger = logger;
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
                    CreateAt = DateTimeOffset.UtcNow
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
                order.IsJoined? true : false,
                order.IsSuccessful ? true : false
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
                order.IsJoined ? true : false,
                order.IsSuccessful ? true : false
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
                o.IsJoined ? true : false,
                o.IsSuccessful ? true : false
            ))
            .ToListAsync();
        }
        public async Task UpdateOrder(Guid id, OrderRequest.UpdateOrderModel model)
        {
            var existedOrder = await _orderRepository.GetById(id);
            if (existedOrder is null)
            {
                return;
            }
            existedOrder.SubscriptionDataId = model.SubscriptionDataId != Guid.Empty ? model.SubscriptionDataId : existedOrder.SubscriptionDataId;
            existedOrder.Quantity = model.Quantity > 0 ? model.Quantity : existedOrder.Quantity;
            existedOrder.IsJoined = model.IsJoined;
            existedOrder.IsSuccessful = model.IsSuccessful;
            existedOrder.IsDeleted = model.IsDeleted;

            existedOrder.ModifiedAt = DateTimeOffset.UtcNow;

            await _orderRepository.Update(existedOrder);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task RemoveOrder(Guid id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }
            order.IsDeleted = true;
            order.ModifiedAt = DateTimeOffset.UtcNow;

            await _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        // Method to create an order with VNPay payment
        public async Task<string> CreateOrderWithVnpayPayment(OrderRequest.CreateOrderModel model)
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
                var orderId = Guid.NewGuid();
                var order = new Order
                {
                    Id = orderId,
                    SubscriptionDataId = subscription.Id,
                    AccountId = account.Id,
                    Quantity = model.Quantity,
                    CreateAt = DateTimeOffset.UtcNow
                };

                // Calculate total price
                decimal totalPrice = (decimal)(subscription.Price * model.Quantity);

                // Save Order to database
                await _orderRepository.Add(order);
                await _orderRepository.SaveChangesAsync();

                // Generate VNPay payment URL
                return CreateVnpayUrl(order, totalPrice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //Method to create VNPay url
        private string CreateVnpayUrl(Order order, decimal totalPrice)
        {
            string createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            // Create list of VNPay parameters
            var vnpParams = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", TmnCode },
                { "vnp_Locale", "vn" },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", orderId },
                { "vnp_OrderInfo", "Thanh toan cho ma GD: " + orderId },
                { "vnp_OrderType", "other" },
                { "vnp_Amount", ((int)totalPrice * 100).ToString() },
                { "vnp_ReturnUrl", $"{ReturnUrlBase}?orderId={order.Id}" },
                { "vnp_CreateDate", createDate },
                { "vnp_IpAddr", GetIpAddress() }
            };

            // Build signature data
            var signData = new StringBuilder();
            foreach (var (key, value) in vnpParams)
            {
                signData.Append(WebUtility.UrlEncode(key)).Append("=").Append(WebUtility.UrlEncode(value)).Append("&");
            }

            // Remove last '&'
            signData.Remove(signData.Length - 1, 1);

            // Generate HMAC-SHA512 signature
            var signature = GenerateHmacSha512(SecretKey, signData.ToString());
            vnpParams.Add("vnp_SecureHash", signature);

            // Build URL with parameters
            var urlBuilder = new StringBuilder(VnpUrl).Append("?");
            foreach (var (key, value) in vnpParams)
            {
                urlBuilder.Append(WebUtility.UrlEncode(key)).Append("=").Append(WebUtility.UrlEncode(value)).Append("&");
            }

            // Remove last '&'
            urlBuilder.Remove(urlBuilder.Length - 1, 1);

            return urlBuilder.ToString();
        }

        //Method to geneate HmacSha512
        private string GenerateHmacSha512(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        //Method to get Ip address
        private string GetIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Request.Headers.TryGetValue("X-Forwarded-For", out var ipValues) == true)
            {
                var ipList = ipValues.ToString().Split(',');
                return ipList.FirstOrDefault()?.Trim() ?? "127.0.0.1";
            }

            return httpContext?.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        }
        // Method to update payment status after receiving response from VNPay
        public async Task UpdatePaymentStatus(Guid orderId, bool isSuccessful)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Update payment status logic here
            order.IsSuccessful = isSuccessful;
            // You might want to add a PaymentStatus property to your Order entity

            order.ModifiedAt = DateTimeOffset.UtcNow;

            await _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        // Method to verify VNPay response (to implement secure payment verification)

        public bool VerifyVnpayResponse(Dictionary<string, string> vnpResponse)
        {
            if (!vnpResponse.ContainsKey("vnp_SecureHash"))
            {
                _logger.LogWarning("VNPay signature is missing.");
                return false;
            }

            string secureHash = vnpResponse["vnp_SecureHash"];
            vnpResponse.Remove("vnp_SecureHash");
            vnpResponse.Remove("vnp_SecureHashType");

            var sortedParams = new SortedDictionary<string, string>(vnpResponse, StringComparer.Ordinal);
            var signData = new StringBuilder();

            foreach (var (key, value) in sortedParams)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    signData.Append(WebUtility.UrlEncode(key)).Append("=").Append(WebUtility.UrlEncode(value)).Append("&");
                }
            }

            if (signData.Length > 0)
            {
                signData.Remove(signData.Length - 1, 1);
            }

            string checkSignature = GenerateHmacSha512(SecretKey, signData.ToString());

            _logger.LogInformation($"String to sign: {signData}");
            _logger.LogInformation($"Received Signature: {secureHash}");
            _logger.LogInformation($"Generated Signature: {checkSignature}");

            return secureHash.Equals(checkSignature, StringComparison.OrdinalIgnoreCase);
        }


    }
}
