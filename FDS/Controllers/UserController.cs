using FDS.Data;
using FDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userRepository;

        public UserController(IUser userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        //[Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll() 
        {
            try
            {
                return Ok(_userRepository.GetAll());
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(string id)
        {
            try
            {
                var user = _userRepository.GetById(Guid.Parse(id));
                if (user != null)
                {
                    return Ok(user);
                }
                else 
                { 
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(string id, User user)
        {
            try
            {
                var _user = _userRepository.GetById(Guid.Parse(id));
                if (_user != null)
                {
                    _userRepository.Update(user, Guid.Parse(id));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(string id)
        {
            try
            {
                var _user = _userRepository.GetById(Guid.Parse(id));
                if (_user != null)
                {
                    _userRepository.Delete(Guid.Parse(id));
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(User user)
        {
            try
            {
                
                return Ok(_userRepository.Add(user));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
