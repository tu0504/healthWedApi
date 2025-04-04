﻿using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService SurveyService)
        {
            _surveyService = SurveyService;
        }

        [HttpGet(Name = "GetAllSurveys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSurveys()
        {
            var result = await _surveyService.GetSurveys();
            return Ok(result);
        }

        [HttpGet("{SurveyId}", Name = "GetSurveyById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSurveyById(Guid SurveyId)
        {
            var result = await _surveyService.GetSurveyById(SurveyId);
            if (result == null)
            {
                return NotFound(new { message = "Survey Not Found" });
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost(Name = "CreateSurvey")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateSurvey([FromBody] SurveyRequest.CreateSurveyRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }
            //var userId = "DAD2A80F-70E4-49F6-B3C5-3C1EEDF525E4";
            await _surveyService.AddSurvey(userId, model);
            return CreatedAtRoute("GetSurveyById", new { SurveyId = /* newly created id */ Guid.NewGuid() }, new { message = "Survey created successfully" });
        }
        //Update Survey Type
        [HttpPut("{SurveyId}", Name = "UpdateSurvey")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateSurvey(Guid SurveyId, [FromBody] SurveyRequest.UpdateSurveyRequest model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            await _surveyService.UpdateSurvey(SurveyId, model);
            return Ok(new { message = "Create Survey Successfully" });
        }

        [HttpDelete("{SurveyId}", Name = "DeleteSurvey")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSurvey(Guid SurveyId)
        {
            var exstingSurvey = await _surveyService.GetSurveyById(SurveyId);
            if (exstingSurvey == null)
            {
                return NotFound(new { message = "Survey Not Found" });
            }
            await _surveyService.RemoveSurvey(SurveyId);
            return Ok(new { message = "Delete Survey Successfully" });
        }
    }
}
