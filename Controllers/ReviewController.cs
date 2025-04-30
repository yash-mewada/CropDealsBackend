using CropDeals.DTOs;
using CropDeals.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = "Dealer")]
[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepo;

    public ReviewController(IReviewRepository reviewRepo)
    {
        _reviewRepo = reviewRepo;
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] AddReviewRequest request)
    {
        var dealerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reviewRepo.AddReviewAsync(dealerId, request);

        if (result.Contains("not found") || result.Contains("Unauthorized"))
            return BadRequest(result);

        return Ok(result);
    }
}
