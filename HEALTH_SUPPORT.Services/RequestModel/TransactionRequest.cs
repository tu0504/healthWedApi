using System;
using System.ComponentModel.DataAnnotations;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class TransactionRequest
    {
        public class CreateTransactionModel
        {
            [Required(ErrorMessage = "OrderId is required.")]
            public Guid OrderId { get; set; }

            [Required(ErrorMessage = "Amount is required.")]
            [Range(0.01f, float.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
            public float Amount { get; set; }

            [Required(ErrorMessage = "PaymentMethod is required.")]
            public string PaymentMethod { get; set; } = "VNPay";

            public string? VnpayOrderInfo { get; set; }
        }

        public class UpdateTransactionModel
        {
            [Required(ErrorMessage = "PaymentStatus is required.")]
            public string PaymentStatus { get; set; }

            public string? VnpayTransactionId { get; set; }

            public string? VnpayResponseCode { get; set; }

            public string? RedirectUrl { get; set; }

            public bool IsDeleted { get; set; }
        }

        public class GenerateVnPayUrlModel
        {
            [Required(ErrorMessage = "OrderId is required.")]
            public Guid OrderId { get; set; }
        }
    }
}