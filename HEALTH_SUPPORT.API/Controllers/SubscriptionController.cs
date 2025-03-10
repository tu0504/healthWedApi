using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionRequest.CreateSubscriptionModel model)
        {
            await _subscriptionService.AddSubscription(model);

            return Ok(new { message = "Tạo chương trình thành công!" });
        }

        [HttpGet(Name = "GetSubscriptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSubscriptions()
        {
            var result = await _subscriptionService.GetSubscriptions();
            return Ok(result);
        }

        [HttpGet("{subscriptionId}", Name = "GetSubscriptionById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSubscriptionById(Guid subscriptionId)
        {
            var result = await _subscriptionService.GetSubscriptionById(subscriptionId);
            if (result == null)
            {
                return NotFound(new { message = "Subscription not found" });
            }
            return Ok(result);
        }

        [HttpPut("{subscriptionId}", Name = "UpdateSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateSubscription(Guid subscriptionId, [FromBody] SubscriptionRequest.UpdateSubscriptionModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            // Kiểm tra xem subscription có tồn tại không
            var existingSubscription = await _subscriptionService.GetSubscriptionById(subscriptionId);
            if (existingSubscription == null)
            {
                return NotFound(new { message = "Subscription not found" });
            }

            await _subscriptionService.UpdateSubscription(subscriptionId, model);
            return Ok(new { message = "Subscription updated successfully" });
        }

        [HttpDelete("{subscriptionId}", Name = "DeleteSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSubscription(Guid subscriptionId)
        {
            var existingSubscription = await _subscriptionService.GetSubscriptionById(subscriptionId);
            if (existingSubscription == null)
            {
                return NotFound(new { message = "Subscription not found" });
            }
            await _subscriptionService.RemoveSubscription(subscriptionId);
            return Ok(new { message = "Subscription deleted successfully" });
        }
    }
}
