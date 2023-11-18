using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =("Admin"))]
    public class AccountPermissionController : ControllerBase
    {
        private readonly IAccountPermission _accountPermission;
        private readonly BlacklistService _blacklistService;

        public AccountPermissionController(IAccountPermission accountPermission, BlacklistService blacklistService) 
        {
            _accountPermission = accountPermission;
            _blacklistService = blacklistService;
        }

        [HttpPost("AddAccount")]
        public async Task<IActionResult> AddAccount(List<string> listUserName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _accountPermission.AddAccount(listUserName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _accountPermission.GetAll();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAccountByUserName")]
        public async Task<IActionResult> GetAccountByUserName(string UserName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _accountPermission.GetByUserName(UserName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("AddAccount")]
        public async Task<IActionResult> DeleteAccount(string UserName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _accountPermission.Delete(UserName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
