using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProgressController : Controller
    {
        private readonly IUserProgressService _userProgressService;
        public UserProgressController(IUserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserProgress([FromBody] UserProgressRequest.CreateUserProgressModel model)
        {
            await _userProgressService.AddUserProgress(model);

            return Ok(new { message = "Tạo tiến độ người dùng thành công!" });
        }

        [HttpGet(Name = "GetUserProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserProgress()
        {
            var result = await _userProgressService.GetUserProgress();
            return Ok(result);
        }

        [HttpGet("{progressId}", Name = "GetUserProgressById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserProgressById(Guid progressId)
        {
            var result = await _userProgressService.GetUserProgressById(progressId);
            if (result == null)
            {
                return NotFound(new { message = "User Progress not found" });
            }
            return Ok(result);
        }

        [HttpPut("{progressId}", Name = "UpdateUserProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUserProgress(Guid progressId, [FromBody] UserProgressRequest.UpdateUserProgressModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            // Kiểm tra xem progress có tồn tại không
            var existingProgress = await _userProgressService.GetUserProgressByIdDeleted(progressId);
            if (existingProgress == null)
            {
                return NotFound(new { message = "User Progress not found" });
            }

            await _userProgressService.UpdateUserProgress(progressId, model);
            return Ok(new { message = "User progress updated successfully" });
        }

        [HttpDelete("{progressId}", Name = "DeleteUserProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserProgress(Guid progressId)
        {
            var existingProgress = await _userProgressService.GetUserProgressById(progressId);
            if (existingProgress == null)
            {
                return NotFound(new { message = "User Progress not found" });
            }
            await _userProgressService.RemoveUserProgress(progressId);
            return Ok(new { message = "User progress deleted successfully" });
        }
    }
}
