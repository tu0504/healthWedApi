using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriptionProgressController : Controller
    {
        private readonly ISubscriptionProgressService _subscriptionProgressService;
        public SubscriptionProgressController(ISubscriptionProgressService subscriptionProgressService)
        {
            _subscriptionProgressService = subscriptionProgressService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSubscriptionProgress([FromBody] SubscriptionProgressRequest.CreateProgressModel model)
        {
            await _subscriptionProgressService.AddSubscriptionProgress(model);

            return Ok(new { message = "Tạo tiến độ chương trình thành công!" });
        }

        [HttpGet(Name = "GetSubscriptionProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSubscriptionProgress()
        {
            var result = await _subscriptionProgressService.GetSubscriptionProgress();
            return Ok(result);
        }

        [HttpGet("{progressId}", Name = "GetSubscriptionProgressById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSubscriptionProgressById(Guid progressId)
        {
            var result = await _subscriptionProgressService.GetSubscriptionProgressById(progressId);
            if (result == null)
            {
                return NotFound(new { message = "Progress not found" });
            }
            return Ok(result);
        }

        [HttpPut("{progressId}", Name = "UpdateSubscriptionProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateSubscriptionProgress(Guid progressId, [FromBody] SubscriptionProgressRequest.UpdateProgressModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            // Kiểm tra xem progress có tồn tại không
            var existingProgress = await _subscriptionProgressService.GetSubscriptionProgressByIdDeleted(progressId);
            if (existingProgress == null)
            {
                return NotFound(new { message = "Progress not found" });
            }

            await _subscriptionProgressService.UpdateSubscriptionProgress(progressId, model);
            return Ok(new { message = "Subscription progress updated successfully" });
        }

        [HttpDelete("{progressId}", Name = "DeleteSubscriptionProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSubscriptionProgress(Guid progressId)
        {
            var existingProgress = await _subscriptionProgressService.GetSubscriptionProgressById(progressId);
            if (existingProgress == null)
            {
                return NotFound(new { message = "Subscription progress not found" });
            }
            await _subscriptionProgressService.RemoveSubscriptionProgress(progressId);
            return Ok(new { message = "Subscription progress deleted successfully" });
        }
    }
}
