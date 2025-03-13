using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService AppointmentService)
        {
            _appointmentService = AppointmentService;
        }

        [HttpGet("{accountId}/Account", Name = "GetAppointmentByAccountId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAppointmentByAccount(Guid accountId)
        {
            var result = await _appointmentService.GetAppointmentsForAccount(accountId);
            if (result == null)
            {
                return NotFound(new { message = "Appointment Not Found" });
            }
            return Ok(result);
        }

        [HttpGet("{psychologistId}/Psychologist", Name = "GetAppointmentByPsychologistId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAppointmentByPPsychologist(Guid psychologistId)
        {
            var result = await _appointmentService.GetAppointmentsForPsychologist(psychologistId);
            if (result == null)
            {
                return NotFound(new { message = "Appointment Not Found" });
            }
            return Ok(result);
        }

        [HttpGet("{appointmentId}", Name = "GetAppointmentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAppointmentById(Guid appointmentId)
        {
            var result = await _appointmentService.GetAppointmentById(appointmentId);
            if (result == null)
            {
                return NotFound(new { message = "Appointment Not Found" });
            }
            return Ok(result);
        }

        [HttpPost(Name = "CreateAppointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateAppointment([FromBody] AppointmentRequest.AddAppointmentRequestRequest model)
        {
            await _appointmentService.AddAppointment(model);
            return CreatedAtRoute("GetAppointmentById", new { AppointmentId = /* newly created id */ Guid.NewGuid() }, new { message = "Appointment created successfully" });
        }
        //Update Appointment Type
        [HttpPut("{AppointmentId}", Name = "UpdateAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAppointment(Guid AppointmentId, [FromBody] AppointmentRequest.EditAppointmentRequestRequest model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }
            await _appointmentService.UpdateAppointment(AppointmentId, model);
            return Ok(new { message = "Create Appointment Successfully" });
        }

        [HttpDelete("{AppointmentId}", Name = "DeleteAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAppointment(Guid AppointmentId)
        {
            var exstingAppointment = await _appointmentService.GetAppointmentById(AppointmentId);
            if (exstingAppointment == null)
            {
                return NotFound(new { message = "Appointment Not Found" });
            }
            await _appointmentService.RemoveAppointment(AppointmentId);
            return Ok(new { message = "Delete Appointment Successfully" });
        }
    }
}
