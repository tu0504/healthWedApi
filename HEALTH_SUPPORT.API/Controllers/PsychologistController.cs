using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PsychologistController : ControllerBase
    {
        private readonly IPsychologistService _psychologistService;

        public PsychologistController(IPsychologistService psychologistService)
        {
            _psychologistService = psychologistService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePsychologist([FromBody] PsychologistRequest.CreatePsychologistModel model)
        {
            await _psychologistService.AddPsychologist(model);
            return Ok(new { message = "Psychologist created successfully!" });
        }

        [HttpGet(Name = "GetPsychologists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPsychologists()
        {
            var result = await _psychologistService.GetPsychologists();
            return Ok(result);
        }

        [HttpGet("{psychologistId}", Name = "GetPsychologistById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPsychologistById(Guid psychologistId)
        {
            var result = await _psychologistService.GetPsychologistById(psychologistId);
            if (result == null)
            {
                return NotFound(new { message = "Psychologist not found" });
            }
            return Ok(result);
        }

        [HttpPut("{psychologistId}", Name = "UpdatePsychologist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePsychologist(Guid psychologistId, [FromBody] PsychologistRequest.UpdatePsychologistModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }

            var existingPsychologist = await _psychologistService.GetPsychologistById(psychologistId);
            if (existingPsychologist == null)
            {
                return NotFound(new { message = "Psychologist not found" });
            }

            await _psychologistService.UpdatePsychologist(psychologistId, model);
            return Ok(new { message = "Psychologist updated successfully" });
        }

        [HttpDelete("{psychologistId}", Name = "DeletePsychologist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePsychologist(Guid psychologistId)
        {
            var existingPsychologist = await _psychologistService.GetPsychologistById(psychologistId);
            if (existingPsychologist == null)
            {
                return NotFound(new { message = "Psychologist not found" });
            }

            await _psychologistService.RemovePsychologist(psychologistId);
            return Ok(new { message = "Psychologist deleted successfully" });
        }

        [HttpPost("{psychologistId}/avatar")]
        public async Task<IActionResult> UploadAvatar(Guid psychologistId, [FromForm] PsychologistRequest.UploadAvatarModel model)
        {
            var response = await _psychologistService.UploadAvatarAsync(psychologistId, model);
            return Ok(response);
        }

        [HttpPut("{psychologistId}/avatar")]
        public async Task<IActionResult> UpdateAvatar(Guid psychologistId, [FromForm] PsychologistRequest.UploadAvatarModel model)
        {
            var response = await _psychologistService.UpdateAvatarAsync(psychologistId, model);
            return Ok(response);
        }

        [HttpDelete("{psychologistId}/avatar")]
        public async Task<IActionResult> DeleteAvatar(Guid psychologistId)
        {
            await _psychologistService.RemoveAvatarAsync(psychologistId);
            return Ok(new { Message = "Avatar deleted successfully" });
        }
    }
}
