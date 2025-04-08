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
using System.Web;

namespace HEALTH_SUPPORT.Services.Implementations
{
    /// <summary>
    /// Service responsible for handling all payment-related operations including VNPay integration
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly IBaseRepository<Transaction, Guid> _transactionRepository;
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly ILogger<TransactionService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // VNPay configuration constants
        private const string TmnCode = "DSZH2ESZ";                    // VNPay merchant code
        private const string SecretKey = "PLUCNCBWGKE6O1Y4K8J6R6Y814ZUFUEM";  // VNPay secret key for signature
        private const string VnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";  // VNPay sandbox URL

        public TransactionService(
            IBaseRepository<Transaction, Guid> transactionRepository,
            IBaseRepository<Order, Guid> orderRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a transaction by its ID with associated order details
        /// </summary>
        public async Task<TransactionResponse.GetTransactionModel?> GetTransactionById(Guid id)
        {
            var transaction = await _transactionRepository.GetAll()
                .Include(t => t.Order)
                .Where(t => !t.IsDeleted)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return null;

            return new TransactionResponse.GetTransactionModel(
                transaction.Id,
                transaction.OrderId,
                transaction.Amount,
                transaction.PaymentMethod,
                transaction.PaymentStatus,
                transaction.VnpayTransactionId,
                transaction.PaymentTime,
                transaction.CreateAt,
                transaction.ModifiedAt,
                transaction.VnpayOrderInfo,
                transaction.VnpayResponseCode,
                transaction.RedirectUrl
            );
        }

        /// <summary>
        /// Retrieves a transaction by its ID, including deleted transactions
        /// </summary>
        public async Task<TransactionResponse.GetTransactionModel?> GetTransactionByIdDeleted(Guid id)
        {
            var transaction = await _transactionRepository.GetAll()
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return null;

            return new TransactionResponse.GetTransactionModel(
                transaction.Id,
                transaction.OrderId,
                transaction.Amount,
                transaction.PaymentMethod,
                transaction.PaymentStatus,
                transaction.VnpayTransactionId,
                transaction.PaymentTime,
                transaction.CreateAt,
                transaction.ModifiedAt,
                transaction.VnpayOrderInfo,
                transaction.VnpayResponseCode,
                transaction.RedirectUrl
            );
        }

        /// <summary>
        /// Retrieves all transactions associated with a specific order
        /// </summary>
        public async Task<List<TransactionResponse.GetTransactionModel>> GetTransactionsByOrderId(Guid orderId)
        {
            return await _transactionRepository.GetAll()
                .Where(t => t.OrderId == orderId && !t.IsDeleted)
                .Select(t => new TransactionResponse.GetTransactionModel(
                    t.Id,
                    t.OrderId,
                    t.Amount,
                    t.PaymentMethod,
                    t.PaymentStatus,
                    t.VnpayTransactionId,
                    t.PaymentTime,
                    t.CreateAt,
                    t.ModifiedAt,
                    t.VnpayOrderInfo,
                    t.VnpayResponseCode,
                    t.RedirectUrl
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all transactions in the system
        /// </summary>
        public async Task<List<TransactionResponse.GetTransactionModel>> GetAllTransactions()
        {
            return await _transactionRepository.GetAll()
                .Where(t => !t.IsDeleted)
                .Select(t => new TransactionResponse.GetTransactionModel(
                    t.Id,
                    t.OrderId,
                    t.Amount,
                    t.PaymentMethod,
                    t.PaymentStatus,
                    t.VnpayTransactionId,
                    t.PaymentTime,
                    t.CreateAt,
                    t.ModifiedAt,
                    t.VnpayOrderInfo,
                    t.VnpayResponseCode,
                    t.RedirectUrl
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new transaction record
        /// </summary>
        public async Task<TransactionResponse.GetTransactionModel> CreateTransaction(TransactionRequest.CreateTransactionModel model)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                OrderId = model.OrderId,
                Amount = model.Amount,
                PaymentMethod = "VNPay",
                PaymentStatus = "success",
                CreateAt = DateTimeOffset.UtcNow,
                PaymentTime = DateTimeOffset.UtcNow,
                VnpayResponseCode = "00",
                RedirectUrl = "http://localhost:5199/api/Transaction/vnpay/callback/redirect"
            };

            // Update order status if payment is successful
            if (transaction.PaymentStatus == "success")
            {
                var order = await _orderRepository.GetAll()
                    .FirstOrDefaultAsync(o => o.Id == model.OrderId && !o.IsDeleted);

                if (order != null)
                {
                    order.IsSuccessful = true;
                    order.ModifiedAt = DateTimeOffset.UtcNow;
                    await _orderRepository.Update(order);
                    _logger.LogInformation("Updated order {OrderId} IsSuccessful flag to true", order.Id);
                }
            }

            await _transactionRepository.Add(transaction);
            await _transactionRepository.SaveChangesAsync();

            return new TransactionResponse.GetTransactionModel(
                transaction.Id,
                transaction.OrderId,
                transaction.Amount,
                transaction.PaymentMethod,
                transaction.PaymentStatus,
                transaction.VnpayTransactionId,
                transaction.PaymentTime,
                transaction.CreateAt,
                transaction.ModifiedAt,
                transaction.VnpayOrderInfo,
                transaction.VnpayResponseCode,
                transaction.RedirectUrl
            );
        }

        /// <summary>
        /// Updates the status of a transaction
        /// </summary>
        public async Task UpdateTransactionStatus(Guid id, string status)
        {
            var transaction = await _transactionRepository.GetAll()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return;
            }

            transaction.PaymentStatus = status;
            transaction.ModifiedAt = DateTimeOffset.UtcNow;

            await _transactionRepository.Update(transaction);
            await _transactionRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a transaction's properties
        /// </summary>
        public async Task UpdateTransaction(Guid id, TransactionRequest.UpdateTransactionModel model)
        {
            var existedTransaction = await _transactionRepository.GetAll()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existedTransaction == null)
            {
                return;
            }

            existedTransaction.PaymentStatus = model.PaymentStatus ?? existedTransaction.PaymentStatus;
            existedTransaction.VnpayTransactionId = model.VnpayTransactionId ?? existedTransaction.VnpayTransactionId;
            existedTransaction.VnpayResponseCode = model.VnpayResponseCode ?? existedTransaction.VnpayResponseCode;
            existedTransaction.RedirectUrl = model.RedirectUrl ?? existedTransaction.RedirectUrl;
            existedTransaction.IsDeleted = model.IsDeleted;
            existedTransaction.ModifiedAt = DateTimeOffset.UtcNow;

            await _transactionRepository.Update(existedTransaction);
            await _transactionRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves payment statistics for a given date range
        /// </summary>
        public async Task<Dictionary<string, float>> GetPaymentStatistics(DateTime startDate, DateTime endDate)
        {
            var transactions = await _transactionRepository.GetAll()
                .Where(t => t.CreateAt >= startDate && t.CreateAt <= endDate && !t.IsDeleted)
                .ToListAsync();

            return new Dictionary<string, float>
            {
                { "totalAmount", transactions.Sum(t => t.Amount) },
                { "successfulAmount", transactions.Where(t => t.PaymentStatus == "success").Sum(t => t.Amount) },
                { "pendingAmount", transactions.Where(t => t.PaymentStatus == "pending").Sum(t => t.Amount) },
                { "failedAmount", transactions.Where(t => t.PaymentStatus == "failed").Sum(t => t.Amount) }
            };
        }

        /// <summary>
        /// Retrieves all transactions with a specific status
        /// </summary>
        public async Task<List<TransactionResponse.GetTransactionModel>> GetTransactionsByStatus(string status)
        {
            return await _transactionRepository.GetAll()
                .Where(t => t.PaymentStatus == status && !t.IsDeleted)
                .Select(t => new TransactionResponse.GetTransactionModel(
                    t.Id,
                    t.OrderId,
                    t.Amount,
                    t.PaymentMethod,
                    t.PaymentStatus,
                    t.VnpayTransactionId,
                    t.PaymentTime,
                    t.CreateAt,
                    t.ModifiedAt,
                    t.VnpayOrderInfo,
                    t.VnpayResponseCode,
                    t.RedirectUrl
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Soft deletes a transaction by setting IsDeleted flag
        /// </summary>
        public async Task RemoveTransaction(Guid id)
        {
            var transaction = await _transactionRepository.GetAll()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return;
            }

            transaction.IsDeleted = true;
            transaction.ModifiedAt = DateTimeOffset.UtcNow;

            await _transactionRepository.Update(transaction);
            await _transactionRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Processes the response from VNPay payment gateway
        /// </summary>
        /// <param name="vnpResponse">Dictionary containing VNPay response parameters</param>
        /// <returns>Dictionary containing processed payment result</returns>
        public async Task<Dictionary<string, object>> ProcessVnPayResponse(Dictionary<string, string> vnpResponse)
        {
            _logger.LogInformation("Received VNPay Response: {Response}",
                string.Join(", ", vnpResponse.Select(kv => $"{kv.Key}={kv.Value}")));

            var response = new Dictionary<string, object>();

            // First verify the signature to ensure the response is legitimate
            if (!VerifySignature(vnpResponse))
            {
                response["paymentStatus"] = "failed";
                response["message"] = "Invalid signature";
                return response;
            }

            // Extract transaction reference from the response
            if (!vnpResponse.TryGetValue("vnp_TxnRef", out string txnRef))
            {
                _logger.LogWarning("Invalid or missing transaction reference in VNPay response.");
                response["paymentStatus"] = "failed";
                response["message"] = "Invalid transaction reference";
                return response;
            }

            // Update payment status and create transaction record
            await UpdatePaymentStatus(txnRef, vnpResponse);

            // Get the latest transaction for this reference
            var transaction = await _transactionRepository.GetAll()
                .Where(t => t.VnpayTransactionId == txnRef)
                .OrderByDescending(t => t.CreateAt)
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                response["paymentStatus"] = "failed";
                response["message"] = "Transaction not found";
                response["redirectUrl"] = GenerateVnPayCallbackUrl(Guid.Empty); // Fallback URL
                return response;
            }

            // Prepare response with transaction details
            response["paymentStatus"] = transaction.PaymentStatus;
            response["redirectUrl"] = transaction.RedirectUrl ?? GenerateVnPayCallbackUrl(transaction.OrderId);
            response["transactionId"] = transaction.VnpayTransactionId;
            response["amountPaid"] = transaction.Amount;
            response["paymentTime"] = transaction.PaymentTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

            return response;
        }

        /// <summary>
        /// Updates the payment status and creates a transaction record based on VNPay response
        /// </summary>
        private async Task UpdatePaymentStatus(string txnRef, Dictionary<string, string> vnpResponse)
        {
            // Extract order ID from the transaction reference
            var order = await _orderRepository.GetAll()
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.CreateAt)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                _logger.LogWarning("No active order found for transaction reference: {TxnRef}", txnRef);
                throw new InvalidOperationException("No active order found for the transaction.");
            }

            // Create new transaction with order ID
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,  // Set the order ID
                PaymentMethod = "VNPay",
                CreateAt = DateTimeOffset.UtcNow,
                PaymentTime = DateTimeOffset.UtcNow,
                VnpayTransactionId = txnRef
            };

            // Set payment status based on VNPay response code
            if (vnpResponse.TryGetValue("vnp_ResponseCode", out string responseCode))
            {
                transaction.PaymentStatus = responseCode switch
                {
                    "00" => "success",  // VNPay success code
                    "pending" => "pending",
                    _ => "failed"
                };
                transaction.VnpayResponseCode = responseCode;
            }

            // Update transaction details from VNPay response
            if (vnpResponse.TryGetValue("vnp_Amount", out string amountStr) && float.TryParse(amountStr, out float amount))
            {
                transaction.Amount = amount / 100; // VNPay amount is in VND * 100
            }

            if (vnpResponse.TryGetValue("vnp_OrderInfo", out string orderInfo))
            {
                transaction.VnpayOrderInfo = orderInfo;
            }

            if (vnpResponse.TryGetValue("vnp_ReturnUrl", out string redirectUrl))
            {
                transaction.RedirectUrl = redirectUrl;
            }

            // Save changes to database
            await _transactionRepository.Add(transaction);
            await _transactionRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Verifies the signature of the VNPay response to ensure it's legitimate
        /// </summary>
        private bool VerifySignature(Dictionary<string, string> vnpResponse)
        {
            if (!vnpResponse.ContainsKey("vnp_SecureHash"))
            {
                _logger.LogWarning("VNPay signature is missing.");
                return false;
            }

            string secureHash = vnpResponse["vnp_SecureHash"];

            // Create a new dictionary excluding hash-related fields
            var signParams = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var (key, value) in vnpResponse.Where(x => x.Key != "vnp_SecureHash" && x.Key != "vnp_SecureHashType"))
            {
                signParams[key] = HttpUtility.UrlDecode(value);
            }

            // Build signature string
            var signData = new StringBuilder();
            foreach (var (key, value) in signParams)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    signData.Append(key).Append("=").Append(WebUtility.UrlEncode(value)).Append("&");
                }
            }

            // Remove last '&'
            if (signData.Length > 0)
            {
                signData.Remove(signData.Length - 1, 1);
            }

            string signDataStr = signData.ToString();
            _logger.LogInformation("Verification Signature Data: {SignDataStr}", signDataStr);

            // Generate signature and compare
            string checkSignature = GenerateHmacSha512(SecretKey, signDataStr);

            _logger.LogInformation("Generated Signature: {Signature}", checkSignature);
            _logger.LogInformation("Received SecureHash: {SecureHash}", secureHash);

            return secureHash.Equals(checkSignature, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Generates HMAC-SHA512 signature for VNPay
        /// </summary>
        private string GenerateHmacSha512(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// Gets the client's IP address from the request
        /// </summary>
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

        /// <summary>
        /// Generates a payment URL for VNPay
        /// </summary>
        /// <param name="model">Model containing order ID and return URL</param>
        /// <returns>Payment URL for VNPay</returns>
        public async Task<string> GenerateVnPayUrl(TransactionRequest.GenerateVnPayUrlModel model)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.SubscriptionData)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Calculate total amount in VND (VNPay uses VND * 100)
            var totalAmount = (long)(order.SubscriptionData.Price * order.Quantity * 100);

            // Get current timestamp in correct format
            string createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string txnRef = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            // Create payment parameters
            var vnpParams = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", TmnCode },
                { "vnp_Locale", "vn" },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", txnRef },
                { "vnp_OrderInfo", $"Thanh toan cho ma GD: {txnRef}" },
                { "vnp_OrderType", "other" },
                { "vnp_Amount", totalAmount.ToString() },
                { "vnp_ReturnUrl", $"http://localhost:5199/api/Transaction/vnpay/callback" },
                { "vnp_CreateDate", createDate },
                { "vnp_IpAddr", GetIpAddress() }
            };

            // Build signature data
            var signData = new StringBuilder();
            foreach (var (key, value) in vnpParams)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    signData.Append(WebUtility.UrlEncode(key)).Append("=").Append(WebUtility.UrlEncode(value)).Append("&");
                }
            }

            // Remove last '&'
            if (signData.Length > 0)
            {
                signData.Remove(signData.Length - 1, 1);
            }

            // Generate signature
            string signature = GenerateHmacSha512(SecretKey, signData.ToString());
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

        /// <summary>
        /// Generates a callback URL for VNPay payment processing
        /// </summary>
        /// <param name="orderId">ID of the order being processed</param>
        /// <returns>Callback URL for VNPay</returns>
        public string GenerateVnPayCallbackUrl(Guid orderId)
        {
            var baseUrl = _httpContextAccessor.HttpContext?.Request.Scheme + "://" + 
                         _httpContextAccessor.HttpContext?.Request.Host;
            
            return $"{baseUrl}/api/transaction/vnpay/callback";
        }
    }
} 