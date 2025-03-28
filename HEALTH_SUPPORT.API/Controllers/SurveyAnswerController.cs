﻿using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyAnswerController : ControllerBase
    {
        private readonly ISurveyAnswerService _SurveyAnswerService;

        public SurveyAnswerController(ISurveyAnswerService SurveyAnswerService)
        {
            _SurveyAnswerService = SurveyAnswerService;
        }

        [HttpGet("{SurveyAnswerId}", Name = "GetSurveyAnswerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSurveyAnswerById(Guid SurveyAnswerId)
        {
            var result = await _SurveyAnswerService.GetSurveyAnswerById(SurveyAnswerId);
            if (result == null)
            {
                return NotFound(new { message = "SurveyAnswer Not Found" });
            }
            return Ok(result);
        }

        [HttpGet("{surveyQuestionId}/SurveyQuestion", Name = "GetSurveyAnswerByQuestionId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSurveyAnswerForQuestion(Guid surveyQuestionId)
        {
            var result = await _SurveyAnswerService.GetSurveyAnswerForQuestion(surveyQuestionId);
            return Ok(result);
        }

        [HttpPost("{surveyQuestionId}", Name = "CreateSurveyAnswer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateSurveyAnswer(Guid surveyQuestionId, [FromBody] List<SurveyAnswerRequest.CreateSurveyAnswerRequest> model)
        {
            await _SurveyAnswerService.AddSurveyAnswerForSurveyQuestion(surveyQuestionId, model);
            return CreatedAtRoute("GetSurveyAnswerById", new { SurveyAnswerId = /* newly created id */ Guid.NewGuid() }, new { message = "SurveyAnswer Type created successfully" });
        }
        //Update SurveyAnswer Type
        [HttpPut("{SurveyAnswerId}", Name = "UpdateSurveyAnswer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateSurveyAnswer(Guid SurveyAnswerId, [FromBody] SurveyAnswerRequest.UpdateSurveyAnswerRequest model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            var exstingSurveyAnswer = await _SurveyAnswerService.GetByIdDeleted(SurveyAnswerId);
            if (exstingSurveyAnswer == null)
            {
                return NotFound(new { message = "SurveyAnswer Not Found" });
            }
            await _SurveyAnswerService.UpdateSurveyAnswer(SurveyAnswerId, model);
            return Ok(new { message = "Create SurveyAnswer Successfully" });
        }

        [HttpDelete("{SurveyAnswerId}", Name = "DeleteSurveyAnswer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSurveyAnswer(Guid SurveyAnswerId)
        {
            await _SurveyAnswerService.RemoveSurveyAnswer(SurveyAnswerId);
            return Ok(new { message = "Remove SurveyAnswer Successfully" });
        }
    }
}