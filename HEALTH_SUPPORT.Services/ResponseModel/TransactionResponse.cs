using System;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class TransactionResponse
    {
        public record GetTransactionModel(
            Guid Id,
            Guid OrderId,
            float Amount,
            string PaymentMethod,
            string PaymentStatus,
            string? VnpayTransactionId,
            DateTimeOffset PaymentTime,
            DateTimeOffset CreateAt,
            DateTimeOffset? ModifiedAt,
            string? VnpayOrderInfo,
            string? VnpayResponseCode,
            string? RedirectUrl
        );

        public record TransactionSummaryModel(
            Guid Id,
            float Amount,
            string PaymentStatus,
            DateTimeOffset PaymentTime
        );
    }
}