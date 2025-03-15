using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthDataController : ControllerBase
    {
        private readonly IHealthDataService _healthDataService;

        public HealthDataController(IHealthDataService healthDataService)
        {
            _healthDataService = healthDataService;
        }

        [HttpGet(Name = "GetHealthDatas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetHealthDatas()
        {
            var result = await _healthDataService.GetHealthDatas();
            return Ok(result);
        }
        
        [HttpGet("{accountId}/account", Name = "GetHealthDataByAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetHealthDataByAccount(Guid accountId)
        {
            var result = await _healthDataService.GetHealthDataByAccountId(accountId);
            return Ok(result);
        }
        
        [HttpGet("{psychologistId}/psychologist", Name = "GetHealthDataByPsyochologist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetHealthDataByPsychologist(Guid psychologistId)
        {
            var result = await _healthDataService.GetHealthDataByPsychologistId(psychologistId);
            return Ok(result);
        }

        [HttpGet("{HealthDataId}", Name = "GetHealthDataById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHealthDataById(Guid HealthDataId)
        {
            var result = await _healthDataService.GetHealthDataById(HealthDataId);
            if (result == null)
            {
                return NotFound(new { message = "Heal data not found" });
            }
            return Ok(result);
        }

        [HttpPost(Name = "CreateHealthData")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateHealthData([FromBody] HealthDataRequest.AddHealthDataRequest model)
        {
            await _healthDataService.AddHealthData(model);
            return CreatedAtRoute("GetHealthDataById", new { HealthDataId = /* newly created id */ Guid.NewGuid() }, new { message = "Health Data created successfully" });
        }
        //Update Health Data
        [HttpPut("{HealthDataId}", Name = "UpdateHealthData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateHealthData(Guid HealthDataId, [FromBody] HealthDataRequest.UpdateHealthDataRequest model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            // check Health Data exist
            var exstingHealthData = await _healthDataService.GetHealthDataById(HealthDataId);
            if (exstingHealthData == null)
            {
                return NotFound(new { message = "Health Data not found" });
            }

            await _healthDataService.UpdateHealthData(HealthDataId, model);
            return Ok(new { message = "Health Data successfully" });
        }

        [HttpDelete("{HealthDataId}", Name = "DeleteHealthData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteHealthData(Guid HealthDataId)
        {
            var exstingHealthData = await _healthDataService.GetHealthDataById(HealthDataId);
            if (exstingHealthData == null)
            {
                return NotFound(new { message = "Health Data not found" });
            }
            await _healthDataService.RemoveHealthData(HealthDataId);
            return Ok(new { message = "Health Data successfully" });
        }
    }
}
