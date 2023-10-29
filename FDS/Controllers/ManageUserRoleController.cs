using FDS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUserRoleController : ControllerBase
    {
        private readonly ManageUserRole _manageUserRole;

        public ManageUserRoleController(ManageUserRole manageUserRole) 
        {
            _manageUserRole = manageUserRole;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AddUserRole(string email, string roleName)
        {
            try
            {
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
