using CropDeals.DTOs;
using CropDeals.Models;
using CropDeals.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAccountRepository _accountRepo;
    private readonly ICropListingRepository _cropListingRepo;
    private readonly IReportRepository _reportRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(
        IAccountRepository accountRepo,
        ICropListingRepository cropListingRepo,
        IReportRepository reportRepo,
        UserManager<ApplicationUser> userManager)
    {
        _accountRepo = accountRepo;
        _cropListingRepo = cropListingRepo;
        _reportRepo = reportRepo;
        _userManager = userManager;
    }

    [HttpPut("edit-user/{id}")]
    public async Task<IActionResult> EditUser(string id, [FromBody] AdminEditUserProfileRequest request)
    {
        var result = await _accountRepo.AdminEditUserAsync(id, request);
        if (result.Contains("not found") || result.Contains("cannot"))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _accountRepo.AdminDeleteUserAsync(id);
        if (result.Contains("not found") || result.Contains("Cannot"))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("edit-crop-listing/{id}")]
    public async Task<IActionResult> EditCropListing(Guid id, AdminUpdateCropListingRequest request)
    {
        var result = await _cropListingRepo.AdminEditCropListingAsync(id, request);
        if (result.Contains("not found"))
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("generate-report")]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateReportRequest request)
    {
        var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reportRepo.GenerateReportAsync(adminId, request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id.ToString(),
            Name = user.Name,                          // If you store full name
            Email = user.Email,
            Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? "N/A",
            PhoneNumber = user.PhoneNumber,
            Status = user.Status == 0                  // Adjust according to your enum or field
        }).ToList();

        return Ok(userDtos);
    }

}
