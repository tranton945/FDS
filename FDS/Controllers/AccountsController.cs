using FDS.Models;
using FDS.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _account;

        public AccountsController(IAccountRepository account) 
        {
            _account = account;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await _account.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _account.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);  
        }

        [HttpPost("SignOut")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _account.SignOut();
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var result = await _account.GetAllAccounts();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("SelectAccountByEmail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccountByEmail(string email)
        {
            try
            {
                var result = await _account.GetAccountsByEmail(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateAccount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccount(string email, string newName, DateTime newDateOfBirt, string newGender)
        {
            try
            {
                var result = await _account.UpdateAccount(email, newName, newDateOfBirt, newGender);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdatePassword")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePassword(ChangePasswordModel model)
        {
            try
            {
                var result = await _account.UpdatePassword(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteAccount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(string email)
        {
            try
            {
                var result = await _account.DeleteAccount(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
