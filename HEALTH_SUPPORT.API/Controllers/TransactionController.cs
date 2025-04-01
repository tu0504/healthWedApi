using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Logging;

namespace HEALTH_SUPPORT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, IConfiguration configuration, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponse.GetTransactionModel>> GetTransactionById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionById(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet("deleted/{id}")]
        public async Task<ActionResult<TransactionResponse.GetTransactionModel>> GetTransactionByIdDeleted(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdDeleted(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<TransactionResponse.GetTransactionModel>>> GetTransactionsByOrderId(Guid orderId)
        {
            var transactions = await _transactionService.GetTransactionsByOrderId(orderId);
            return Ok(transactions);
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionResponse.GetTransactionModel>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionResponse.GetTransactionModel>> CreateTransaction(TransactionRequest.CreateTransactionModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid transaction data" });
            }

            try 
            {
                var transaction = await _transactionService.CreateTransaction(model);
                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<TransactionResponse.GetTransactionModel>>> GetTransactionsByStatus(string status)
        {
            var transactions = await _transactionService.GetTransactionsByStatus(status);
            return Ok(transactions);
        }

        [HttpPut("{id}", Name = "UpdateTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTransaction(Guid id, [FromBody] TransactionRequest.UpdateTransactionModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }

            // Check if transaction exists
            var existingTransaction = await _transactionService.GetTransactionByIdDeleted(id);
            if (existingTransaction == null)
            {
                return NotFound(new { message = "Transaction not found" });
            }

            await _transactionService.UpdateTransaction(id, model);
            return Ok(new { message = "Transaction updated successfully" });
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<Dictionary<string, float>>> GetPaymentStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var statistics = await _transactionService.GetPaymentStatistics(startDate, endDate);
            return Ok(statistics);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            try
            {
                await _transactionService.RemoveTransaction(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("vnpay/url")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GenerateVnPayUrl([FromBody] TransactionRequest.GenerateVnPayUrlModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            try
            {
                var paymentUrl = await _transactionService.GenerateVnPayUrl(model);
                return Ok(paymentUrl);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("vnpay/callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, object>>> ProcessVnPayCallback([FromQuery] Dictionary<string, string> vnpResponse)
        {
            if (vnpResponse == null || vnpResponse.Count == 0)
            {
                return BadRequest(new { message = "Invalid VNPay response" });
            }

            var result = await _transactionService.ProcessVnPayResponse(vnpResponse);
            return Ok(result);
        }
    }
} 