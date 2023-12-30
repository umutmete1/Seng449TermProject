using Microsoft.AspNetCore.Identity;

public class MyUser : IdentityUser
{
    public string Address { get; set; }
    public string Gender { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Stock>? Watchlist { get; set; }
    
}