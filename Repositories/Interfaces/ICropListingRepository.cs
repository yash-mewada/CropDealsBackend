using CropDeals.Models;

public interface ICropListingRepository
{
    Task<IEnumerable<CropListing>> GetByFarmerIdAsync(Guid farmerId);
    Task<CropListing?> GetByIdAsync(Guid id);
    Task AddAsync(CropListing listing);
    Task UpdateAsync(CropListing listing);
    Task DeleteAsync(CropListing listing);
    Task<string> AdminEditCropListingAsync(Guid cropListingId, AdminUpdateCropListingRequest request);
    Task<IEnumerable<CropListing>> GetListingsByDealerLocationAsync(Guid dealerId);
    Task<IEnumerable<CropListing>> GetListingsByFarmerAsync(Guid farmerId);
    Task<IEnumerable<CropListing>> SearchListingsByCropNameAsync(string cropName);

}
