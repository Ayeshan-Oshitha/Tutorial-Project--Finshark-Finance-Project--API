﻿using Finshark.Data;
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
        [Route("Create")]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _dbContext.Stocks.Add(stockModel);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }


        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto )
        {
            var stockModel = _dbContext.Stocks.FirstOrDefault( x => x.Id == id );
            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv  = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _dbContext.SaveChanges();
            return Ok(stockModel.ToStockDto() );
        }


        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _dbContext.Stocks.FirstOrDefault( x => x.Id == id);
            if(stockModel == null)
            {
                return NotFound();
            }
            _dbContext.Stocks.Remove(stockModel);
            _dbContext.SaveChanges();
            return NoContent();
        }


    }
}
