using ChildrenCare.DTOs.AppUserDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenCare.Services;

public class AppUserService : IAppUserService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserStore<AppUser> _userStore;
    private readonly IUserEmailStore<AppUser> _emailStore;

    public AppUserService(SignInManager<AppUser> signInManager, IUserStore<AppUser> userStore, UserManager<AppUser> userManager, IRepositoryWrapper repositoryWrapper)
    {
        _signInManager = signInManager;
        _userStore = userStore;
        _userManager = userManager;
        _repositoryWrapper = repositoryWrapper;
        _emailStore = GetEmailStore();
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<CustomResponse> Login(LoginRequestDTO dto)
    {
        //var hasBlocked =
        //    await _repositoryWrapper.User.AnyAsync(
        //        user => user.Email.ToLower() == dto.Email.ToLower());
        

        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var getAccount = await _repositoryWrapper.User.FindByConditionAsync(a => a.Email == dto.Email && a.IsPotentialCustomer == true);
            if (getAccount.Count() != 0)
            {
                await _signInManager.SignOutAsync();
                return new CustomResponse(false, "This account has been inactive!");
            }
            else
            {
                return new CustomResponse(true, "User Logged In");
            }
        }
        else
        {
            return new CustomResponse(false, "Invalid Email or Password");
        }

    }

    public async Task<CustomResponse> Register(RegisterRequestDTO dto)
    {
        //Check email
        var hasExistingUser =
            await _repositoryWrapper.User.AnyAsync(
                user => user.Email.ToLower() == dto.Email.ToLower());
        if (hasExistingUser)
        {
            return new CustomResponse(false, "User with email already exists");
        }

        var newUser = dto.MapToNewUser();

        await _userStore.SetUserNameAsync(newUser, dto.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(newUser, dto.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        if (!result.Succeeded)
        {
            
            return new CustomResponse(false, result.ToString());
        }
        await _userManager.AddToRoleAsync(newUser, GlobalVariables.CustomerRole);
        return new CustomResponse(true, "User created");
    }
    
    public async Task<CustomResponse> UpdatePassword(AppUser dto, string newPassword)
    {
        //Check email
        var isExistedUser =
            await _repositoryWrapper.User.AnyAsync(
                user => user.Email.ToLower() == dto.Email.ToLower());
        if (!isExistedUser)
        {
            return new CustomResponse(false, "User not exist");
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(dto);
        await _userManager.ResetPasswordAsync(dto, token, newPassword);
        
        return new CustomResponse(true, "User Updated");
    }
    private IUserEmailStore<AppUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<AppUser>)_userStore;
    }
}