using DTO.User;
using BusinessLogic.IServices;
using DataAccess.Entities;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<Customer, string> _customerRepository;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _studentRepository = studentRepository;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            ApplicationUser user;
                        
            if (dto.Role == "Student")
            {
                
                user = new Student
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Name = $"{dto.FirstName} {dto.LastName}",
                    Address = dto.Address,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    IsActive = true
                };
            }
            else
            {
                user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Name = $"{dto.FirstName} {dto.LastName}",
                    Address = dto.Address
                };
                
            }
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, dto.Role);
            }
            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginDto dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}
