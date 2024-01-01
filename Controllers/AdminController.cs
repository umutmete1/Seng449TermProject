using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TermProject.models;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase{
    private readonly UserManager<MyUser> _userManager;
    private readonly IMapper _mapper;

    public AdminController(UserManager<MyUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(){
        var users = await _userManager.Users
            .ToListAsync();
        var usersVm = _mapper.Map<List<MyUser>, List<UserVm>>(users.ToList());
        return Ok(usersVm);
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