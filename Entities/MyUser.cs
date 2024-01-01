using Microsoft.AspNetCore.Identity;
using TermProject.models;

public class MyUser : IdentityUser
{
    public string Address { get; set; }
    public string Gender { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ICollection<UserWatchlist> Watchlist { get; set; }
    
}