using FDS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OldDocVerController : ControllerBase
    {
        private readonly IOldDocVerRepository _oldDoc;

        public OldDocVerController(IOldDocVerRepository oldDoc) 
        {
            _oldDoc = oldDoc;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _oldDoc.getAll();
            return Ok(result);
        } 
        
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id) 
        {
            var result = await _oldDoc.getById(id);
            return Ok(result);
        }

        [HttpGet("GetAllByDocId")]
        public async Task<IActionResult> GetAllByDocId(int docId)
        {
            var result = await _oldDoc.getAllByDocId(docId);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
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
