using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TermProject.models;

namespace TermProject.controllers;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : Controller
{
    private readonly UserManager<MyUser> _userManager;

    public RegisterController(UserManager<MyUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
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
                //If the User registration successful
                //Here You can generate and return a JWT token here if needed
                return Ok(new { Message = "Registration successful" });
            }
            else
            {
                
                return BadRequest(new { Errors = result.Errors });
            }
        }

       
        return BadRequest(new { Message = "Invalid registration data" });
    }
}