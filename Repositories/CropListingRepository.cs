// Repositories/CropListingRepository.cs
using CropDeals.Data;
using CropDeals.Models;
using Microsoft.EntityFrameworkCore;

public class CropListingRepository : ICropListingRepository
{
    private readonly ApplicationDbContext _context;
    public CropListingRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<CropListing>> GetByFarmerIdAsync(Guid farmerId) =>
        await _context.CropListings.Include(c => c.Crop).Where(c => c.FarmerId == farmerId).ToListAsync();

    public async Task<CropListing?> GetByIdAsync(Guid id) =>
        await _context.CropListings.Include(c => c.Crop).FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(CropListing listing)
    {
        await _context.CropListings.AddAsync(listing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CropListing listing)
    {
        _context.CropListings.Update(listing);
        await _context.SaveChangesAsync();
    }

    public async Task<string> AdminEditCropListingAsync(Guid cropListingId, AdminUpdateCropListingRequest request)
    {
        var listing = await _context.CropListings.FindAsync(cropListingId);
        if (listing == null)
            return "Crop listing not found.";

        // Update values
        listing.PricePerKg = (float)request.PricePerKg;
        listing.Quantity = request.Quantity;
        listing.Status = Enum.TryParse<CropAvailability>(request.Status, true, out var parsedStatus)
            ? parsedStatus
            : CropAvailability.Available;
        listing.ImageBase64 = request.ImageBase64;
        listing.Description = request.Description;

        _context.CropListings.Update(listing);
        await _context.SaveChangesAsync();

        return "Crop listing updated successfully.";
    }

    public async Task<IEnumerable<CropListing>> GetListingsByDealerLocationAsync(Guid dealerId)
    {
        var dealer = await _context.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == dealerId);

        if (dealer?.Address == null)
            return Enumerable.Empty<CropListing>();

        var locationMatches = await _context.CropListings
            .Include(cl => cl.Farmer)
            .ThenInclude(f => f.Address)
            .Include(cl => cl.Crop)
            .Where(cl =>
                cl.Status == CropAvailability.Available &&
                cl.Farmer.Address.City == dealer.Address.City &&
                cl.Farmer.Address.State == dealer.Address.State &&
                cl.Quantity > 0)
            .ToListAsync();

        return locationMatches;
    }

    public async Task<IEnumerable<CropListing>> GetListingsByFarmerAsync(Guid farmerId)
    {
        return await _context.CropListings
            .Include(c => c.Crop)
            .Include(c => c.Farmer)
            .Where(c => c.FarmerId == farmerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CropListing>> SearchListingsByCropNameAsync(string cropName)
    {
        return await _context.CropListings
            .Include(c => c.Crop)
            .Include(c => c.Farmer)
            .Where(c =>
                c.Crop.Name.ToLower().Contains(cropName.ToLower()) &&
                c.Status == CropAvailability.Available &&
                c.Quantity > 0
            )
            .ToListAsync();
    }

    public async Task DeleteAsync(CropListing listing)
    {
        _context.CropListings.Remove(listing);
        await _context.SaveChangesAsync();
    }

    public async Task<CropListing?> GetDetailsByIdAsync(Guid listingId)
    {
        return await _context.CropListings
            .Include(cl => cl.Crop)
            .Include(cl => cl.Farmer)
                .ThenInclude(f => f.Address)
            .FirstOrDefaultAsync(cl => cl.Id == listingId && cl.Status == CropAvailability.Available);
    }


}
