using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TermProject.models;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IMapper _mapper;

    public AdminController(UserManager<MyUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(string role)
    {
        var usersWithUserRole = await _userManager.GetUsersInRoleAsync(role);
        var usersVm = _mapper.Map<IList<MyUser>, List<UserVm>>(usersWithUserRole);
        return Ok(usersVm);
    }

    [HttpPost("AppointAdmin")]
    public async Task<IActionResult> AppointAdmin([FromBody] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }

        await _userManager.AddToRoleAsync(user, "Admin");
        return Ok();
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return NotFound();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("DeleteUser")]
    public async Task<IActionResult> DeleteUser([FromBody] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}


