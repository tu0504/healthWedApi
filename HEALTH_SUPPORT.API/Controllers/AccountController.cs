using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Http;
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

        [HttpGet(Name = "GetAccounnt")]
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

        [HttpPost(Name = "CreateAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateAccount([FromBody] AccountRequest.CreateAccountModel model)
        {
            await _accountService.AddAccount(model);
            return Ok(new { message = "Account created successfully" });
        }

        [HttpPut("{accountId}", Name = "UpdateAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAccount(Guid accountId, [FromBody] AccountRequest.UpdateAccountModel model)
        {
            if (model == null)
            {
                var account = await _accountService.GetAccountById(accountId);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found" });
                }
                return Ok(account);
            }
            await _accountService.UpdateAccount(accountId, model);
            return Ok(new { message = "Account updated successfully" });
        }

        [HttpDelete("{accountId}", Name = "DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccount(Guid accountId)
        {
            await _accountService.RemoveAccount(accountId);
            return Ok(new { message = "Account deleted successfully" });
        }

    }
}
