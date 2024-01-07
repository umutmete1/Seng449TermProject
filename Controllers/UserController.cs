using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TermProject.models;
using TermProject.Services.UserService;

namespace TermProject.controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IUserService _userService;

    public UserController(UserManager<MyUser> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        
        if (!IsEmailValid(model.Email))
        {
            return BadRequest(ErrorResponse.Return(400, "Enter a valid e mail"));
        }
        
        if (ModelState.IsValid)
        {
            var user = new MyUser
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                Gender = model.Gender,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(SuccessResponse.Return(200, "Registration is successfull"));
            }

            var error = result.Errors.First().Description;
            
            return BadRequest(ErrorResponse.Return(400, error));
        }

       
        return BadRequest(new { Message = "Invalid data" });
    }
    
    private bool IsEmailValid(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswdRm model)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return BadRequest(ErrorResponse.Return(400, "Bir hata meydana geldi"));
        }

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(ErrorResponse.Return(400, result.ToString()));
        }

        return Ok(SuccessResponse.Return(200, "Şifre başarı ile değişitirildi"));
    }

    [HttpGet("GetInformation")]
    [Authorize]
    public async Task<IActionResult> GetInformation()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetInformation(userId);

        if (user == null)
        {
            return BadRequest(ErrorResponse.Return(400, "Bir hata meydana geldi"));

        }

        return Ok(user);
    }
    
    [HttpGet("CheckAdmin")]
    [Authorize]
    public async Task<IActionResult> CheckAdmin()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return BadRequest(ErrorResponse.Return(400, "Bir hata meydana geldi"));

        }
        
        var userRoles = await _userManager.GetRolesAsync(user);
        

        return Ok(userRoles.FirstOrDefault() == "Admin");
    }
}