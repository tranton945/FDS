using FDS.Data;
using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocument _doc;

        public DocumentController(IDocument doc) 
        { 
            _doc = doc;
        }

        [HttpPost("AddDoc")]
        [Authorize]
        public async Task<IActionResult> AddDoc(Document doc)
        {
            try
            {
                //var id =Convert.ToInt32(HttpContext.User.FindFirstValue("userId"));
                var result = await _doc.Add(doc);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllDoc")]
        public async Task<IActionResult> GetAllDoc()
        {
            try
            {
                var result = await _doc.GetAll();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocById")]
        public async Task<IActionResult> GetDocById(int id)
        {
            try
            {
                var result = await _doc.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateDoc")]
        [Authorize]
        public async Task<IActionResult> UpdateDoc(Document doc ,int id)
        {
            try
            {
                var result = await _doc.Update(doc, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteDoc(int id)
        {
            try
            {
                var result = await _doc.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> Search(string searchString)
        {
            try
            {
                var result = await _doc.Search(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SortByDate")]
        [Authorize]
        public async Task<IActionResult> SortByDate()
        {
            try
            {
                var result = await _doc.GetDocumentsByDate();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SortByAuthor")]
        [Authorize]
        public async Task<IActionResult> SortByAuthor(string author)
        {
            try
            {
                var result = await _doc.GetDocumentsByAuthor(author);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
