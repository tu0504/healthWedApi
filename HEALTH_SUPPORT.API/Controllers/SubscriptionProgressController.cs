using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionProgressController : Controller
    {
        private readonly ISubscriptionProgressService _subscriptionProgressService;

        public SubscriptionProgressController(ISubscriptionProgressService subscriptionProgressService)
        {
            _subscriptionProgressService = subscriptionProgressService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSubscriptionProgress([FromBody] SubscriptionProgressRequest.CreateSubscriptionProgressModel model)
        {
            await _subscriptionProgressService.CreateSubscriptionProgress(model);
            return Ok(new { message = "Tạo tiến độ chương trình thành công" });
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

        [HttpGet(Name = "GetSubscriptionProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSubscriptionProgress()
        {
            var result = await _subscriptionProgressService.GetAllSubscriptionProgress();
            return Ok(result);
        }
    }
}
