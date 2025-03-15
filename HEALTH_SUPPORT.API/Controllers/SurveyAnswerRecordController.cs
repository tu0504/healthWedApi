using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyAnswerRecordController : ControllerBase
    {
        private readonly ISurveyAnswerRecordService _surveyAnswerService;

        public SurveyAnswerRecordController(ISurveyAnswerRecordService surveyAnswerService)
        {
            _surveyAnswerService = surveyAnswerService;
        }

        [HttpGet("{accountSurveyId}", Name = "GetSurveyAnswerRecordById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSurveyAnswerRecordById(Guid accountSurveyId)
        {
            var result = await _surveyAnswerService.GetSurveyAnswerRecordById(accountSurveyId);
            if (result == null)
            {
                return NotFound(new { message = "SurveyAnswerRecord Not Found" });
            }
            return Ok(result);
        }

        [HttpPost(Name = "CreateSurveyAnswerRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateSurveyAnswerRecord([FromBody] SurveyAnswerRecordRequest.AddSurveyAnswerRecordRequest model)
        {
            await _surveyAnswerService.AddSurveyAnswerRecord(model);
            return Ok(new { message = "Survey Answer Record created successfully" });
        }

        [HttpDelete("{SurveyId}", Name = "DeleteSurveyAnswerRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSurvey(Guid SurveyId)
        {
            await _surveyAnswerService.RemoveSurveyAnswerRecord(SurveyId);
            return Ok(new { message = "Delete Survey Answer Record Successfully" });
        }
    }
}
