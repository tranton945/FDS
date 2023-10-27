using FDS.Models;
using Microsoft.AspNetCore.Identity;

namespace FDS.Services
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);

        public Task<string> SignInAsync(SignInModel model);
    }
}
