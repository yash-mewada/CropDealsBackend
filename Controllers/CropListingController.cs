// Controllers/CropListingController.cs

using System.Security.Claims;
using AutoMapper;
using CropDeals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CropListingController : ControllerBase
{
    private readonly ICropListingRepository _listingRepo;
    private readonly ICropRepository _cropRepo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CropListingController(ICropListingRepository listingRepo, ICropRepository cropRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _listingRepo = listingRepo;
        _cropRepo = cropRepo;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> Create([FromBody] CropListingCreateDto dto)
    {
        var crop = await _cropRepo.GetByIdAsync(dto.CropId);
        if (crop == null) return BadRequest("Invalid crop.");

        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var listing = _mapper.Map<CropListing>(dto);
        listing.FarmerId = Guid.Parse(userId!);

        await _listingRepo.AddAsync(listing);
        return Ok(listing);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CropListingUpdateDto dto)
    {
        var listing = await _listingRepo.GetByIdAsync(id);
        if (listing == null) return NotFound();

        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (listing.FarmerId != Guid.Parse(userId!))
            return Forbid();

        _mapper.Map(dto, listing);
        listing.UpdatedAt = DateTime.UtcNow;

        await _listingRepo.UpdateAsync(listing);
        return Ok(listing);
    }

    [Authorize(Roles = "Dealer")]
    [HttpGet("by-location")]
    public async Task<IActionResult> GetByLocation()
    {
        var dealerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var listings = await _listingRepo.GetListingsByDealerLocationAsync(dealerId);

        var listingDtos = _mapper.Map<IEnumerable<CropListingReadDto>>(listings);
        return Ok(listingDtos);
    }

    [Authorize(Roles = "Farmer")]
    [HttpGet("my-listings")]
    public async Task<IActionResult> GetMyListings()
    {
        var farmerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var listings = await _listingRepo.GetListingsByFarmerAsync(farmerId);

        var listingDtos = _mapper.Map<IEnumerable<CropListingReadDto>>(listings);
        return Ok(listingDtos);
    }

    [Authorize(Roles = "Dealer")]
    [HttpGet("search")]
    public async Task<IActionResult> SearchByCropName([FromQuery] string cropName)
    {
        if (string.IsNullOrWhiteSpace(cropName))
            return BadRequest("Crop name is required.");

        var listings = await _listingRepo.SearchListingsByCropNameAsync(cropName);
        var listingDtos = _mapper.Map<IEnumerable<CropListingReadDto>>(listings);
        return Ok(listingDtos);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Farmer,Admin")]
    public async Task<IActionResult> DeleteListing(Guid id)
    {
        var listing = await _listingRepo.GetByIdAsync(id);
        if (listing == null)
            return NotFound("Crop listing not found.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        // If user is Farmer, only allow deleting their own listing
        if (userRole == "Farmer" && listing.FarmerId != Guid.Parse(userId!))
            return Forbid("You can only delete your own listings.");

        await _listingRepo.DeleteAsync(listing);
        return Ok("Crop listing deleted successfully.");
    }

    [HttpGet("{id}/details")]
    [Authorize(Roles = "Dealer,Admin")] 
    public async Task<IActionResult> GetListingDetails(Guid id)
    {
        var listing = await _listingRepo.GetDetailsByIdAsync(id);
        if (listing == null)
            return NotFound("Listing not found or is out of stock.");

        var dto = new CropListingDetailsDto
        {
            CropName = listing.Crop.Name,
            Description = listing.Description,
            ImageBase64 = listing.ImageBase64,
            PricePerKg = listing.PricePerKg,
            Quantity = listing.Quantity,
            FarmerName = listing.Farmer.Name,
            FarmerPhoneNumber = listing.Farmer.PhoneNumber,
            Street = listing.Farmer.Address?.Street ?? "",
            City = listing.Farmer.Address?.City ?? "",
            State = listing.Farmer.Address?.State ?? "",
            ZipCode = listing.Farmer.Address?.ZipCode ?? ""
        };

        return Ok(dto);
    }

}
