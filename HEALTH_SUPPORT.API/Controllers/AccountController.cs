using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> CreateAccount([FromBody] AccountRequest.CreateAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.AddAccount(model);
            if (result.StartsWith("Email"))
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { message = result });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AccountRequest.LoginRequestModel model)
        {
            var loginResult = await _accountService.ValidateLoginAsync(model);
            if (loginResult == null)
            {
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng" });
            }

            var token = _accountService.GenerateJwtToken(loginResult);

            return Ok(new
            {
                accessToken = token,
                user = loginResult
            });
        }

        [HttpGet(Name = "GetAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAccounts()
        {
            var result = await _accountService.GetAccounts();
            return Ok(result);
        }

        [HttpGet("{accountId}", Name = "GetAccountById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAccountById(Guid accountId)
        {
            var result = await _accountService.GetAccountById(accountId);
            if(result == null)
            {
                return NotFound(new { message = "Account not found" });
            }
            return Ok(result);
        }

        [HttpPut("{accountId}", Name = "UpdateAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAccount(Guid accountId, [FromBody] AccountRequest.UpdateAccountModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }

            var existingAccount = await _accountService.GetAccountById(accountId);
            if (existingAccount == null)
            {
                return NotFound(new { message = "Account not found" });
            }

            await _accountService.UpdateAccount(accountId, model);
            return Ok(new { message = "Account updated successfully" });
        }

        [HttpDelete("{accountId}", Name = "DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccount(Guid accountId)
        {
            var existingAccount = await _accountService.GetAccountById(accountId);
            if (existingAccount == null)
            {
                return NotFound(new { message = "Account not found" });
            }
            await _accountService.RemoveAccount(accountId);
            return Ok(new { message = "Account deleted successfully" });
        }
    }
}
