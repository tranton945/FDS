using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, GO")]
    public class OldDocVerController : ControllerBase
    {
        private readonly IOldDocVerRepository _oldDoc;
        private readonly BlacklistService _blacklistService;

        public OldDocVerController(IOldDocVerRepository oldDoc, BlacklistService blacklistService) 
        {
            _oldDoc = oldDoc;
            _blacklistService = blacklistService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() 
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _oldDoc.getAll();
            return Ok(result);
        } 
        
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id) 
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _oldDoc.getById(id);
            return Ok(result);
        }

        [HttpGet("GetAllByDocId")]
        public async Task<IActionResult> GetAllByDocId(int docId)
        {
            if (await _blacklistService.CheckJWT() == true)
            {
                return BadRequest("access token invalid");
            }
            var result = await _oldDoc.getAllByDocId(docId);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _oldDoc.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteAllByDocId")]
        public async Task<IActionResult> DeleteAllByDocId(int docId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _oldDoc.DeleteAllByDocId(docId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
