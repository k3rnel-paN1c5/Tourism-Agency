using Microsoft.AspNetCore.Identity;
using Application.DTOs.Employee;
using Application.DTOs.User;
using System;

namespace Application.IServices.Auth{
        public interface IEmployeeAuthService
        {
                Task<SignInResult> LoginAsync(LoginDTO dto);
                public Task<IdentityResult> RegisterAsync(EmployeeRegisterDTO dto);
                Task LogoutAsync();
        }

}

