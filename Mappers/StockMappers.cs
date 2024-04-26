using api.Dtos.Stock;
using api.Models;
using System.Runtime.CompilerServices;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto request)
        {
            return new Stock
            {
                Symbol = request.Symbol,
                CompanyName = request.CompanyName,
                Industry = request.Industry,
                Purchase = request.Purchase,
                LastDiv = request.LastDiv,
                MarketCap = request.MarketCap,

            };
        }
    }
}
