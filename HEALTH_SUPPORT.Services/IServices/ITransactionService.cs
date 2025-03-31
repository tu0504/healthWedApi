using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ITransactionService
    {
        // CRUD operations
        Task<TransactionResponse.GetTransactionModel?> GetTransactionById(Guid id);
        Task<TransactionResponse.GetTransactionModel?> GetTransactionByIdDeleted(Guid id);
        Task<List<TransactionResponse.GetTransactionModel>> GetTransactionsByOrderId(Guid orderId);
        Task<List<TransactionResponse.GetTransactionModel>> GetAllTransactions();
        
        // Payment processing
        Task<Dictionary<string, object>> ProcessVnPayResponse(Dictionary<string, string> vnpResponse);
        Task<TransactionResponse.GetTransactionModel> CreateTransaction(TransactionRequest.CreateTransactionModel model);
        Task UpdateTransactionStatus(Guid transactionId, string status);
        Task UpdateTransaction(Guid id, TransactionRequest.UpdateTransactionModel model);
        Task<string> GenerateVnPayUrl(TransactionRequest.GenerateVnPayUrlModel model);
        Task RemoveTransaction(Guid id);
        
        // Payment statistics/analytics (optional)
        Task<Dictionary<string, float>> GetPaymentStatistics(DateTime startDate, DateTime endDate);
        Task<List<TransactionResponse.GetTransactionModel>> GetTransactionsByStatus(string status);
    }
} 