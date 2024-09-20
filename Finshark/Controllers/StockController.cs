using Finshark.Data;
using Finshark.DTOs.Stock;
using Finshark.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Finshark.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public StockController(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var stocks = _dbContext.Stocks.ToList()
                .Select( s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById([FromRoute]  int id) 
        { 
            var stock = _dbContext.Stocks.Find(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _dbContext.Stocks.Add(stockModel);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }
    }
}
