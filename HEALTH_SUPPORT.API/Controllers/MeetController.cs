using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetController : Controller
    {
        private readonly IGoogleMeetService _meetService;

        public MeetController(IGoogleMeetService meetService)
        {
            _meetService = meetService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMeet([FromBody] GoogleMeetRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request data");

            try
            {
                var meetLink = await _meetService.CreateGoogleMeetEvent(request);
                return Ok(new { meetLink });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
