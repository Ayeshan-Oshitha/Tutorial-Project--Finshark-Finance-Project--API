﻿using Finshark.Models;

namespace Finshark.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser, String symbol);
    }
}
