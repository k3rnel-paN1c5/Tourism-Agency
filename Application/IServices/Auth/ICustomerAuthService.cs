using Microsoft.AspNetCore.Identity;
using Application.DTOs.Customer;
using Application.DTOs.User;

namespace Application.IServices.Auth{
    public interface ICustomerAuthService
    {
        Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task<IdentityResult> RegisterAsync(CustomerRegisterDTO dto);
        Task LogoutAsync();
    }

}

