using FDS.Data;
using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocTypeController : ControllerBase
    {
        private readonly IDocTypeRepository _type;
        private readonly BlacklistService _blacklistService;

        public DocTypeController(IDocTypeRepository type, BlacklistService blacklistService) 
        {
            _type = type;
            _blacklistService = blacklistService;
        }

        [HttpPost("addType")]
        [Authorize]
        public async Task<IActionResult> addType([FromBody] DocType docType)
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _type.Add(docType);
            return Ok(result);

        }
        [HttpGet("GetAllType")]
        [Authorize]
        public async Task<IActionResult> GetAllType()
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _type.GetAll();
            return Ok(result);

        }
        [HttpGet("GetTypeById")]
        [Authorize]
        public async Task<IActionResult> GetTypeById(int id)
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _type.GetById(id);
            return Ok(result);

        }
        [HttpPut("UpdateType")]
        [Authorize]
        public async Task<IActionResult> UpdateType(DocType docType, int id)
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _type.Update(docType, id);
            return Ok(result);

        }
        [HttpDelete("DeleteType")]
        [Authorize]
        public async Task<IActionResult> DeleteType(int id)
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _type.Delete(id);
            return Ok(result);

        }
    }
}
