using Microsoft.AspNetCore.Identity;
using DTO.User;
using System;

namespace BusinessLogic.IServices;

public interface IAuthService
{
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<SignInResult> LoginAsync(LoginUserDto dto);
        Task LogoutAsync();
}
