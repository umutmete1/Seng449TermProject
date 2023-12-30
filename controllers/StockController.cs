


using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase{
    private readonly StockService _stockService;
    public StockController(StockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet("GetStocks")]
    public async Task<ActionResult<List<Stock>>> GetStocks()
    {
        return await _stockService.GetStocksAsync();
    }

    [HttpGet("GetStockById/{id}")]
    public async Task<ActionResult<Stock>> GetStockById(int id)
    {
        var stock = await _stockService.GetStockByIdAsync(id);
        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }

    [HttpPost("AddStock")]
    public async Task<ActionResult<Stock>> AddStock(Stock stock)
    {
        await _stockService.AddStockAsync(stock);
        return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock);
    }

    [HttpPut("UpdateStock/{id}")]
    public async Task<IActionResult> UpdateStock(int id, Stock stock)
    {
        if (id != stock.Id)
        {
            return BadRequest();
        }

        await _stockService.UpdateStockAsync(stock);
        return NoContent();
    }

    [HttpDelete("DeleteStock/{id}")]   
    public async Task<ActionResult<Stock>> DeleteStock(int id)
    {
        var stock = await _stockService.DeleteStockAsync(id);
        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }
}