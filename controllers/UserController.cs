using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TermProject.services.UserService;

namespace TermProject.controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IUserService _userService;

    public UserController(UserManager<MyUser> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [HttpGet("GetUserWatchlist")]
    
    public async Task<IActionResult> GetUserWatchlist()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId.Equals(null))
        {
            return BadRequest("Bir hata oluştu");
        }

        var watchlist = await _userService.GetWatchlist(userId);

        return Ok(watchlist);


    }
}