using Microsoft.AspNetCore.Identity;
using DTO.Customer;
using DTO.User;

namespace BusinessLogic.IServices;

public interface ICustomerAuthService
{
    Task<SignInResult> LoginAsync(LoginDTO dto);
    public Task<IdentityResult> RegisterAsync(CustomerRegisterDTO dto);
    Task LogoutAsync();
}
