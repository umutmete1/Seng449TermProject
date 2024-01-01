using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TermProject.models;
using TermProject.services.WatchlistService;

namespace TermProject.controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class WatchlistController : ControllerBase
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(UserManager<MyUser> userManager, IWatchlistService watchlistService)
    {
        _userManager = userManager;
        _watchlistService = watchlistService;
    }

    [HttpGet("GetUserWatchlist")]
    
    public async Task<IActionResult> GetUserWatchlist()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Bir hata oluştu");
        }

        var watchlist = await _watchlistService.GetWatchlist(userId);

        return Ok(watchlist);


    }

    [HttpPost("AddStock")]
    public async Task<IActionResult> AddStockToWatchlist(string stockCode)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Lütfen giriş yapınız");
        }
        
        bool isStockAlreadyAdded = await _watchlistService.IsStockAlreadyAdded(stockCode, userId);
        if (isStockAlreadyAdded)
        {
            return BadRequest(ErrorResponse.Return(400, $"Hisse senedi kodu '{stockCode}' zaten izleme listesinde bulunmaktadır."));
        }
        
        Stock stock = await _watchlistService.AddStockToWatchlist(stockCode, userId);
        
        if (stock == null)
        {
            return BadRequest(ErrorResponse.Return(400, $"Hisse senedi kodu '{stockCode}' bulunamadı"));
        }

        return Ok(stock);

    }
    
    [HttpDelete("DeleteStock")]
    public async Task<IActionResult> RemoveStockToWatchlist(string stockCode)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Lütfen giriş yapınız");
        }

        if (await _watchlistService.RemoveStockFromWatchlist(stockCode, userId))
        {
            return Ok(SuccessResponse.Return(200, "Hisse başarıyla silindi"));
        }

        // Hisse silinemediyse 404 hata kodu ile geri dön
        return NotFound(ErrorResponse.Return(404, "Hisse bulunamadı"));

    }

    [HttpGet("GetStockCount")]
    public async Task<IActionResult> GetStockCount()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Lütfen giriş yapınız");
        }

        var count = await _watchlistService.GetStockCount(userId);

        return Ok(count);
    }
}