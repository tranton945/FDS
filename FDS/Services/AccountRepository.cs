using FDS.Data;
using FDS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FDS.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {

            if (!model.Email.EndsWith("@vietjetair.com"))
            {
                // Trả về null nếu tài khoản email không đáp ứng yêu cầu
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            // login fail
            if(!result.Succeeded) 
            {
                return string.Empty;
            }

            

            var autClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            var listUserRoles = await GetRolesOfUser(model.Email);
            // Thêm tất cả các claim về vai trò vào danh sách claim
            autClaims.AddRange(listUserRoles);

            var authenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: autClaims,
                signingCredentials: new SigningCredentials(authenkey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {

            if (!model.Email.EndsWith("@vietjetair.com"))
            {
                // Trả về lỗi nếu tài khoản email không đáp ứng yêu cầu
                return IdentityResult.Failed(new IdentityError { Description = "Only @vietjetair.com email addresses are allowed to register." });

            }
            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                DateOfBirt = model.DateOfBirt,
                Gender = model.Gender,
                UserName = model.Email,
            };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<bool> SignOut()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        private async Task<List<Claim>> GetRolesOfUser(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            // lây danh sách role của user
            var roles = await _userManager.GetRolesAsync(user);
            // Tạo các claim cho từng role
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            return roleClaims;
        }

        public async Task<List<ApplicationUser>> GetAllAccounts()
        {
            var accounts = await _userManager.Users.ToListAsync();
            return accounts;
        }

        public async Task<ApplicationUser> GetAccountsByEmail(string email)
        {
            if (!email.EndsWith("@vietjetair.com"))
            {
                return null;
            }
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (accounts == null)
            {
                return null;
            }
            return accounts;
        }

        public async Task<bool> UpdateAccount(string email, string newName, DateTime newDateOfBirt, string newGender)
        {
            if (!email.EndsWith("@vietjetair.com"))
            {
                return false;
            }
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (accounts == null)
            {
                return false;
            }
            var oldName = accounts.Name;
            var oldDateOfBirt = accounts.DateOfBirt;
            var oldGender = accounts.Gender;


            accounts.Name = newName;
            accounts.DateOfBirt = newDateOfBirt;
            accounts.Gender = newGender;

            var result = await _userManager.UpdateAsync(accounts);

            if (result.Succeeded)
            {
                // Xóa bộ nhớ đệm để tránh trường hợp data cũ vẫn được sử dụng
                await _userManager.UpdateSecurityStampAsync(accounts);

                return true;
            }
            else
            {
                // Khôi phục lại data cũ nếu cập nhật không thành công
                accounts.Name = newName;
                accounts.DateOfBirt = newDateOfBirt;
                accounts.Gender = newGender;

                return false;
            }

        }

        public async Task<bool> DeleteAccount(string email)
        {
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(accounts == null)
            { 
                return false; 
            }

            var result = await _userManager.DeleteAsync(accounts);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatePassword(ChangePasswordModel model)
        {
            var account = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if(account == null) 
            { 
                return false; 
            }
            if(model.NewPassword != model.ConfirmPassword)
            {
                return false;
            }
            // tạo một token để đặt lại mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(account);
            var result = await _userManager.ResetPasswordAsync(account, token, model.NewPassword);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
