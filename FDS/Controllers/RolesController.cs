using FDS.Models;
using FDS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly Roles _roles;

        public RolesController(Roles roles)
        {
            _roles = roles;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(_roles.GetAllRoles());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{role}")]
        public async Task<IActionResult> GetRole(string role)
        {
            var _role = _roles.GetByName(role);
            try
            {
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

        [HttpDelete("{role}")]
        public IActionResult DeleteRole(string role)
        {
            try
            {
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
