using Finshark.Data;
using Finshark.Interfaces;
using Finshark.Models;
using Microsoft.EntityFrameworkCore;

namespace Finshark.Repository
{
    public class EFStockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public EFStockRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }


        public Task<List<Stock>> GetAllAsync()
        {
            return _dbContext.Stocks.ToListAsync();
        }
    }
}
