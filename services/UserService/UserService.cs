using Microsoft.EntityFrameworkCore;

namespace TermProject.services.UserService;
public class UserService : IUserService
{
    private readonly AppDbContext _appDbContext;

    public UserService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Stock>> GetWatchlist(string userId)
    {
        var user = await _appDbContext.Users.Include(u => u.Watchlist)
            .FirstOrDefaultAsync(u => u.Id == userId);
        //Serialization error

        if (user != null && user.Watchlist != null)
        {
            return user.Watchlist;
        }

        return new List<Stock>();
    }
}