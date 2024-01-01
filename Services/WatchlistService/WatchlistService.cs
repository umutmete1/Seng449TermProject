using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TermProject.models;
using TermProject.models.WatchlistModels;

namespace TermProject.services.UserService;
public class WatchlistService : IWatchlistService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public WatchlistService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<List<WatchlistVm>> GetWatchlist(string userId)
    {
        var watchlist = await _appDbContext.UserWatchlist
            .Include(w => w.Stock)
            .Where(w => w.MyUser.Id == userId)
            .ToListAsync();

        var watchlistVm = _mapper.Map<List<UserWatchlist>, List<WatchlistVm>>(watchlist);

        return watchlistVm;
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

    public async Task<bool> RemoveStockFromWatchlist(string stockCode, string userId)
    {
        var userWatchlist = await _appDbContext.UserWatchlist
            .FirstOrDefaultAsync(w => w.Stock.Code == stockCode && w.MyUser.Id == userId);

        if (userWatchlist != null)
        {
            _appDbContext.UserWatchlist.Remove(userWatchlist);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> IsStockAlreadyAdded(string stockCode, string userId)
    {
        return await _appDbContext.UserWatchlist
            .AnyAsync(uw => uw.Stock.Code == stockCode && uw.MyUser.Id == userId);
    }
}