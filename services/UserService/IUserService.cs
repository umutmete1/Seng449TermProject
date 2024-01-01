﻿using TermProject.models;

namespace TermProject.services.UserService;

public interface IUserService
{
    Task<List<object>> GetWatchlist(string userId);
    Task<Stock> AddStockToWatchlist(string stockCode, string userId);

    Task<bool> IsStockAlreadyAdded(string stockCode, string userId);

}