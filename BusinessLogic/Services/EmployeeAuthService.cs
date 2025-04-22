using Microsoft.AspNetCore.Identity;
using DTO;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.IServices;
using DTO.User;
using DataAccess.Repositories.IRepositories;
using DTO.Employee;

namespace BusinessLogic.Services
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
