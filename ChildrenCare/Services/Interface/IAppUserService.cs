using ChildrenCare.DTOs.AppUserDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenCare.Services.Interface;

public interface IAppUserService
{
    public Task Logout();
    public Task<CustomResponse> Login(LoginRequestDTO dto);
    public Task<CustomResponse> Register(RegisterRequestDTO dto);

    public Task<CustomResponse> UpdatePassword(AppUser appUser, string newPassword);
    
}