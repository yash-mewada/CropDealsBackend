// Controllers/CropController.cs
using System.Security.Claims;
using AutoMapper;
using CropDeals.Data;
using CropDeals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CropController : ControllerBase
{
    private readonly ICropRepository _cropRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public CropController(ICropRepository cropRepository, IMapper mapper, ApplicationDbContext context)
    {
        _cropRepository = cropRepository;
        _mapper = mapper;
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CropCreateDto dto)
    {
        var crop = _mapper.Map<Crop>(dto);
        await _cropRepository.AddAsync(crop);
        return Ok(crop);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CropUpdateDto dto)
    {
        var crop = await _cropRepository.GetByIdAsync(id);
        if (crop == null) return NotFound();

        _mapper.Map(dto, crop);
        await _cropRepository.UpdateAsync(crop);
        return Ok(crop);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCrop(Guid id)
    {
        var crop = await _cropRepository.GetByIdAsync(id);
        if (crop == null)
            return NotFound("Crop not found.");

        // 🛡️ Safety check: prevent deleting crops used in listings
        var isInUse = await _context.CropListings.AnyAsync(cl => cl.CropId == id);
        if (isInUse)
            return BadRequest("Cannot delete crop. It's being used in listings.");

        await _cropRepository.DeleteAsync(crop);
        return Ok("Crop deleted successfully.");
    }

    [HttpGet("admin-crops")]
    [Authorize(Roles = "Farmer,Admin")] // ✅ Only Admins can access
    public async Task<ActionResult<IEnumerable<CropDto>>> GetAdminCrops()
    {
        var crops = await _cropRepository.GetAllCropsAsync();

        var cropDtos = crops.Select(c => new CropDto
        {
            Id = c.Id,
            Name = c.Name,
            Type = c.Type.ToString(),
            CreatedAt = c.CreatedAt
        }).ToList();

        return Ok(cropDtos);
    }

}
