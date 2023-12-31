using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TermProject.models;
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

    [HttpPost("AddStock")]
    public async Task<IActionResult> AddStockToWatchlist(string stockCode)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId.Equals(null))
        {
            throw new UnauthorizedAccessException("Lütfen giriş yapınız");
        }
        
        bool isStockAlreadyAdded = await _userService.IsStockAlreadyAdded(stockCode, userId);
        if (isStockAlreadyAdded)
        {
            return BadRequest(ErrorResponse.Return(400, $"Hisse senedi kodu '{stockCode}' zaten izleme listesinde bulunmaktadır."));
        }
        
        Stock stock = await _userService.AddStockToWatchlist(stockCode, userId);
        
        if (stock == null)
        {
            return BadRequest(ErrorResponse.Return(400, $"Hisse senedi kodu '{stockCode}' bulunamadı"));
        }

        return Ok(stock);

    }
}