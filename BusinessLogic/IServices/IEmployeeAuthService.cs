using Microsoft.AspNetCore.Identity;
using DTO.Employee;
using DTO.User;
using System;

namespace BusinessLogic.IServices;

public interface IEmployeeAuthService
{
        Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task<IdentityResult> RegisterAsync(EmployeeRegisterDTO dto);
        Task LogoutAsync();
}
