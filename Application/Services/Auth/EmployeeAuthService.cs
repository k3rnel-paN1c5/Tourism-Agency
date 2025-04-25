using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.IRepositories;
using Application.IServices.Auth;
using Application.DTOs.User;
using Application.DTOs.Employee;

namespace Application.Services.Auth
{
    public class EmployeeAuthService : IEmployeeAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<Employee,string> _Repository;

        public EmployeeAuthService(UserManager<User> userManager, SignInManager<User> signInManager, IRepository<Employee,string> Repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Repository = Repository;
        }
        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        { 
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        }

        public async  Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public Task<IdentityResult> RegisterAsync(EmployeeRegisterDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
