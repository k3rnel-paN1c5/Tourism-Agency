using System;
using BusinessLogic.IServices;
using DataAccess.Entities;
using DataAccess.Repositories.IRepositories;
using DTO.Customer;
using DTO.User;
using Microsoft.AspNetCore.Identity;
namespace BusinessLogic.Services;

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
        throw new NotImplementedException();
    }
    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }
}
