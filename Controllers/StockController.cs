using api.Data;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Stock;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _repo;

        public StockController(IStockRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? symbol, 
            [FromQuery] string? companyName,
            [FromQuery] string? sortBy,
            [FromQuery] bool isDescending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 2)
        {
            List<Stock> stocks = await _repo.GetAllAsync(symbol, companyName, sortBy, isDescending, pageNumber, pageSize);
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _repo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            StockDto dto = stock.ToStockDto();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Stock stockModel = stockDto.ToStockFromCreateDTO();
            await _repo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _repo.UpdateAsync(id, updateDto);
            if(stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel.ToStockDto());
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _repo.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
