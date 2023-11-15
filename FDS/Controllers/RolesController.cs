using FDS.Models;
using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class RolesController : ControllerBase
    {
        private readonly Roles _roles;
        private readonly BlacklistService _blacklistService;

        public RolesController(Roles roles, BlacklistService blacklistService)
        {
            _roles = roles;
            _blacklistService = blacklistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                return Ok(_roles.GetAllRoles());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetRole(string role)
        {
            var _role = _roles.GetByName(role);
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                return Ok(_role);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRoll(string role)
        {
            var result = await _roles.AddRole(role);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        [HttpDelete("role")]
        public async Task<IActionResult> DeleteRole(string role)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                _roles.DeleteRole(role);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
