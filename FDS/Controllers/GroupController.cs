using FDS.Data;
using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository group;
        private readonly BlacklistService _blacklistService;

        public GroupController(IGroupRepository g, BlacklistService blacklistService)
        {
            group = g;
            _blacklistService = blacklistService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(Data.Group g)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await group.Add(g);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("AddMemberToGroup")]
        [Authorize]
        public async Task<IActionResult> AddMemberToGroup([FromBody] List<string> username, int groupId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await group.AddMember(username, groupId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await group.GetAll();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetById")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await group.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteGroup")]
        [Authorize]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await group.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateGroup(Data.Group g, int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await group.Update(g, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
