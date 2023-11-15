using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManageUserRoleController : ControllerBase
    {
        private readonly ManageUserRole _manageUserRole;
        private readonly BlacklistService _blacklistService;

        public ManageUserRoleController(ManageUserRole manageUserRole, BlacklistService blacklistService) 
        {
            _manageUserRole = manageUserRole;
            _blacklistService = blacklistService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AddUserRole(string email, string roleName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageUserRole.AddUserRole(email, roleName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateUserRole(string email, string newRoleName, string oldRoleName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageUserRole.ChangeUserRole(email, newRoleName, oldRoleName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveUserRole(string email, string roleName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageUserRole.RemoveUserRole(email, roleName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
