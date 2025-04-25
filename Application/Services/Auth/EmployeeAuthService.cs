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
        public async Task<IdentityResult> RegisterAsync(EmployeeRegisterDTO dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
            };

            // Create with password in one call
            var result = await _userManager.CreateAsync(user, dto.Password!);

            if (!result.Succeeded)
                return result;
            // Now create the Employee profile
            Employee newEmp = new Employee
            {
                UserId = user.Id, // Use the generated ID
                HireDate = DateTime.Now
            };

            await _Repository.AddAsync(newEmp);
            await _Repository.SaveAsync();

            await _userManager.AddToRoleAsync(user, dto.EmpRole.ToString());

            return result;
        }

        public async  Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
