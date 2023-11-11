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

        public DocTypeController(IDocTypeRepository type) 
        {
            _type = type;
        }

        [HttpPost("addType")]
        [Authorize]
        public async Task<IActionResult> addType([FromBody] DocType docType)
        {
            var result = await _type.Add(docType);
            return Ok(result);

        }
        [HttpGet("GetAllType")]
        [Authorize]
        public async Task<IActionResult> GetAllType()
        {
            var result = await _type.GetAll();
            return Ok(result);

        }
        [HttpGet("GetTypeById")]
        [Authorize]
        public async Task<IActionResult> GetTypeById(int id)
        {
            var result = await _type.GetById(id);
            return Ok(result);

        }
        [HttpPut("UpdateType")]
        [Authorize]
        public async Task<IActionResult> UpdateType(DocType docType, int id)
        {
            var result = await _type.Update(docType, id);
            return Ok(result);

        }
        [HttpDelete("DeleteType")]
        [Authorize]
        public async Task<IActionResult> DeleteType(int id)
        {
            var result = await _type.Delete(id);
            return Ok(result);

        }
    }
}
