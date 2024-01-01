using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase{
    private readonly UserManager<MyUser> _userManager;

    public AdminController(UserManager<MyUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(){
        var users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost("AppointAdmin")]
    public async Task<IActionResult> AppointAdmin([FromBody] string email){
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }

        await _userManager.AddToRoleAsync(user, "Admin");
        return Ok();
    }
    
}