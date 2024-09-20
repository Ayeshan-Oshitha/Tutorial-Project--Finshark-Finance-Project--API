using Finshark.Data;
using Finshark.DTOs.Stock;
using Finshark.Helpers;
using Finshark.Interfaces;
using Finshark.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finshark.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StockController( IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks = await _stockRepository.GetAllAsync(query); 
            var stockDto = stocks.Select(s => s.ToStockDto());    

            return Ok(stockDto);
        }


        [HttpGet]
        [Route("GetById/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]  int id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock =  await _stockRepository.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }


        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto() );
        }


        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepository.DeleteAsync(id);
            if(stockModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
