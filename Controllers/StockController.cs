using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TermProject.models;
using TermProject.services.StockService;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
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
        if (stock == null) return NotFound();

        return stock;
    }

    [HttpPost("AddStock")]
    public async Task<ActionResult<Stock>> AddStock(Stock stock)
    {
        await _stockService.AddStockAsync(stock);
        return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock);
    }

    [HttpPut("UpdateStock")]
    public async Task<IActionResult> UpdateStock(Stock stock)
    {
        var getStock = await _stockService.GetStockByIdAsync(stock.Id);

        if (getStock == null)
        {
            return BadRequest(ErrorResponse.Return(404, "Hisse bulunamadı"));
        }

        await _stockService.UpdateStockAsync(stock);
        return Ok();
    }

    [HttpDelete("DeleteStock/{id}")]
    public async Task<ActionResult<Stock>> DeleteStock(int id)
    {
        var stock = await _stockService.DeleteStockAsync(id);
        if (stock == null) return NotFound(ErrorResponse.Return(404,"Hisse bulunamadı"));

        return stock;
    }
    
    [HttpGet]
    [Route("/ReadAllStocks")]
    public async Task<IActionResult> ReadAllStocks()
    {
        try
        {
            var result = await _stockService.ReadStockAsync();
            return Ok(new
            {
                NumberOfAddedStocks = result.NumberOfAddedStocks,
                AddedStocks = result.AddedStocks
                
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
}