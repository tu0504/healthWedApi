using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;
using static HEALTH_SUPPORT.Services.RequestModel.AccountRequest;

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

        [HttpPost("create-psychologist")]
        public async Task<IActionResult> AddPsychologistToManager([FromBody] CreateAccountAndPsychologistModel model)
        {
            try
            {
                await _psychologistService.AddPsychologistToManager(model);
                return Ok(new { message = "Psychologist created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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
        [HttpGet("all", Name = "GetPsychologistByManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPsychologistByManager()
        {
            var result = await _psychologistService.GetPsychologistDetailByManager();
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No psychologists found" });
            }
            return Ok(result);
        }
        [HttpGet("profile", Name = "GetPsychologistProfileByManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPsychologistProfileByManager()
        {
            var result = await _psychologistService.GetPsychologistProfileByManager();
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No psychologists found" });
            }
            return Ok(result);
        }

        //[HttpPut("{psychologistId}", Name = "UpdatePsychologist")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> UpdatePsychologist(Guid psychologistId, [FromBody] PsychologistRequest.UpdatePsychologistModel model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest(new { message = "Invalid update data" });
        //    }

        //    var existingPsychologist = await _psychologistService.GetPsychologistById(psychologistId);
        //    if (existingPsychologist == null)
        //    {
        //        return NotFound(new { message = "Psychologist not found" });
        //    }

        //    await _psychologistService.UpdatePsychologist(psychologistId, model);
        //    return Ok(new { message = "Psychologist updated successfully" });
        //}

        //[HttpDelete("{psychologistId}", Name = "DeletePsychologist")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> DeletePsychologist(Guid psychologistId)
        //{
        //    var existingPsychologist = await _psychologistService.GetPsychologistById(psychologistId);
        //    if (existingPsychologist == null)
        //    {
        //        return NotFound(new { message = "Psychologist not found" });
        //    }

        //    await _psychologistService.RemovePsychologist(psychologistId);
        //    return Ok(new { message = "Psychologist deleted successfully" });
        //}

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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePsychologistToManager(Guid id, [FromBody] PsychologistRequest.UpdatePsychologistModel model)
        {
            try
            {
                await _psychologistService.UpdatePsychologistToManager(id, model);
                return Ok(new { message = "Psychologist updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePsychologistToManager(Guid id)
        {
            try
            {
                await _psychologistService.DeletePsychologistById(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

    }
}
