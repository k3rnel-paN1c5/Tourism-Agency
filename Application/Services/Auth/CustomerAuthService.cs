using Domain.Entities;
using Domain.IRepositories;
using Application.IServices.Auth;
using Application.DTOs.Customer;
using Application.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Auth{
    public class CustomerAuthService : ICustomerAuthService
    {
        private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IRepository<Customer, string> _Repository;

            public CustomerAuthService(UserManager<User> userManager, SignInManager<User> signInManager, IRepository<Customer,string> Repository)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _Repository = Repository;
            }
        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        }
        public async Task<IdentityResult> RegisterAsync(CustomerRegisterDTO dto)
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
            // Now create the Customer profile
            Customer newCustomer = new Customer
            {
                UserId = user.Id, // Use the generated ID
                FirstName = dto.FirstName!,
                LastName = dto.LastName!,
                PhoneNumber = dto.PhoneNumber!,
                Whatsapp = dto.Whatsapp!,
                Country = dto.Country!
            };

            await _Repository.AddAsync(newCustomer);
            await _Repository.SaveAsync();

            // await _userManager.AddToRoleAsync(user, "Customer");
            
            return result;
        }
        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }

}