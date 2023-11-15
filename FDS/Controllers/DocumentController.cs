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
        public async Task<IActionResult> AddDoc([FromForm] Document doc, IFormFile? file, IFormFile? signature)
        {
            try
            {
                var result = await _doc.Add(doc, file, signature);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllDoc")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetDocById(int id)
        {
            try
            {
                var result = await _doc.GetById(id);
                if (result == null)
                {
                    return Unauthorized("No Permission");
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateDoc")]
        [Authorize]
        public async Task<IActionResult> UpdateDoc([FromForm] Document? doc ,int id, IFormFile? file, IFormFile? signature)
        {
            try
            {
                var result = await _doc.Update(doc, id, file, signature);
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

        [HttpGet("DownloadById")]
        [Authorize]
        public async Task<IActionResult> DownloadById(int id)
        {
            try
            {
                var result = await _doc.DowloadDocumentById(id);
                if (result == null)
                {
                    return BadRequest("Document not found or empty.");
                }
                var zipBytes = ZipHelper.CreateZipFile(result.Name, result.DocFile);

                return File(zipBytes, "application/zip", $"{DateTime.Now}.zip");
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("DownloadByFlightId")]
        [Authorize]
        public async Task<IActionResult> DowloadDocumentByFlightId(int id)
        {
            try
            {
                var result = await _doc.DowloadDocumentByFlightId(id);
                if (result == null)
                {
                    return BadRequest("Document not found or empty.");
                }
                var zipBytes = ZipHelper.CreateZipFileFromDocuments(result, $"{DateTime.Now}.zip");
                return File(zipBytes, "application/zip", $"{DateTime.Now}.zip");

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
