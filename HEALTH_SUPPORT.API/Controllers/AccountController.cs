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

        [HttpPost(Name = "CreateAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateAccount([FromBody] AccountRequest.CreateAccountModel model)
        {
            await _accountService.AddAccount(model);
            return CreatedAtRoute("GetAccountById", new { accountId = /* id mới tạo */ Guid.NewGuid() }, new { message = "Account created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountRequest.LoginRequestModel model)
        {
            // Gọi service để kiểm tra thông tin đăng nhập
            var loginResult = await _accountService.ValidateLoginAsync(model);
            if (loginResult == null)
            {
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng" });
            }

            // Tạo JWT token dựa trên thông tin đăng nhập thành công
            var token = _accountService.GenerateJwtToken(loginResult);

            // Trả về token cùng thông tin tài khoản (bao gồm RoleName)
            return Ok(new
            {
                accessToken = token,
                user = loginResult
            });
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

            // Kiểm tra xem account có tồn tại không
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
