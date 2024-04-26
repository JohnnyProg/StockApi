using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null)
            {
                return null;
            }
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(string? symbol, string? companyName, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            var stocks = _context.Stock.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
            if(!string.IsNullOrWhiteSpace(symbol))
            {
                stocks = stocks.Where(x=> x.Symbol == symbol);
            }
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                stocks = stocks.Where(x => x.CompanyName == companyName);
            }
            if(!string.IsNullOrEmpty(sortBy))
            {
                if(sortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = isDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (pageNumber - 1) * pageSize;


            return await stocks.Skip(skipNumber).Take(pageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<bool> StockExists(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            if(stock == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(existingStock == null)
            {
                return null;
            }
            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;
            
            await _context.SaveChangesAsync();

            return existingStock;
        }
    }
}
