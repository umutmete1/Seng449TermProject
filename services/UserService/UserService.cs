using Microsoft.EntityFrameworkCore;
using TermProject.models;

namespace TermProject.services.UserService;
public class UserService : IUserService
{
    private readonly AppDbContext _appDbContext;

    public UserService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<object>> GetWatchlist(string userId)
    {
        var watchlist = await _appDbContext.UserWatchlist
            .Include(w => w.Stock)
            .Where(w => w.MyUser.Id == userId)
            .Select(w => new
            {
                Code = w.Stock.Code,
                Name = w.Stock.Name,
                Type = w.Stock.Type
            })
            .ToListAsync();

        return watchlist.Cast<object>().ToList();
    }

    public async Task<Stock> AddStockToWatchlist(string stockCode, string userId)
    {
        var stock = await _appDbContext.Stocks.FirstOrDefaultAsync(s => s.Code == stockCode);
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

       if (stock != null && user != null)
    {
        UserWatchlist userWatchlist = new UserWatchlist()
        {
            Stock = stock,
            MyUser = user
        };

        await _appDbContext.UserWatchlist.AddAsync(userWatchlist);
        await _appDbContext.SaveChangesAsync();
    }

       return stock;

    }
    
    public async Task<bool> IsStockAlreadyAdded(string stockCode, string userId)
    {
        return await _appDbContext.UserWatchlist
            .AnyAsync(uw => uw.Stock.Code == stockCode && uw.MyUser.Id == userId);
    }
}