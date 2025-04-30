// Controllers/TransactionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = "Dealer")]
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepo;

    public TransactionController(ITransactionRepository transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    [HttpPost]
    public async Task<IActionResult> PurchaseCrop(CreateTransactionRequest request)
    {
        var dealerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (dealerId == null) return Unauthorized("Dealer not authenticated");

        var result = await _transactionRepo.CreateTransactionAsync(Guid.Parse(dealerId), request);

        if (result.Contains("not found") || result.Contains("exceeds"))
            return BadRequest(result);

        return Ok(result);
    }
}
