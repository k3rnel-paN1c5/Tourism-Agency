using Microsoft.AspNetCore.Identity;
using DTO.Employee;
using System;

namespace BusinessLogic.IServices;

public interface IEmployeeAuthService
{
        Task<SignInResult> LoginAsync(EmployeeLoginDTO dto);
        Task LogoutAsync();
}
