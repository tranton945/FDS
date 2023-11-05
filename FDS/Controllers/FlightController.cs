using FDS.Data;
using FDS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlight _flight;

        public FlightController(IFlight flight) 
        {
            _flight = flight;
        }

        [HttpPost]
        public async Task<IActionResult> NewFlight(Flight flight)
        {
            try
            {
                var result = await _flight.Add(flight);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFlight()
        {
            try
            {
                var result = await _flight.GetAll();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetFlightById")]
        public async Task<IActionResult> GetFlightById(int id)
        {
            try
            {
                var result = await _flight.GetById(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateFlightById(Flight flight, int id)
        {
            try
            {
                var result = await _flight.Update(flight, id);
                if (result == true)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFlightById(int id)
        {
            try
            {
                var result = await _flight.Delete(id);
                if (result == true)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
