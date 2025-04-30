using CropDeals.Data;
using CropDeals.Models;
using Microsoft.EntityFrameworkCore;

public class CropRepository : ICropRepository
{
    private readonly ApplicationDbContext _context;
    public CropRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Crop>> GetAllAsync() => await _context.Crops.ToListAsync();
    public async Task<Crop?> GetByIdAsync(Guid id) => await _context.Crops.FindAsync(id);

    public async Task AddAsync(Crop crop)
    {
        await _context.Crops.AddAsync(crop);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Crop crop)
    {
        _context.Crops.Update(crop);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Crop crop)
    {
        _context.Crops.Remove(crop);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Crop>> GetAllCropsAsync()
    {
        return await _context.Crops.ToListAsync();
    }

}
