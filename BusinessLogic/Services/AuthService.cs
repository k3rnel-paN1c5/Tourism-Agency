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

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IRepository<Customer, string> customerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerRepository = customerRepository;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            User user;

            if (dto.Role == "Customer")
            {
                user = new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = $"{dto.FirstName} {dto.LastName}",
                    Whatsapp = dto.Address,
                    Country = dto.FirstName;
                };
            }
            else
            {
                user = new User
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