using FDS.Data;
using FDS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FDS.Services
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);

        public Task<string> SignInAsync(SignInModel model);

        public Task<bool> SignOut();

        public Task<List<ApplicationUser>> GetAllAccounts();
        public Task<ApplicationUser> GetAccountsByEmail(string email);
        public Task<bool> UpdateAccount(string email, string newName, DateTime newDateOfBirt, string newGender);
        public Task<bool> UpdatePassword(ChangePasswordModel model);
        public Task<bool> DeleteAccount(string email);

    }
}
