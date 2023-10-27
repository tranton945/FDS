using FDS.Data;
using FDS.Services;
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
