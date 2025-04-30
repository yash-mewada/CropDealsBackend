using CropDeals.Models;

public interface ICropRepository
{
    Task<IEnumerable<Crop>> GetAllAsync();
    Task<Crop?> GetByIdAsync(Guid id);
    Task AddAsync(Crop crop);
    Task UpdateAsync(Crop crop);
    Task DeleteAsync(Crop crop);
    Task<List<Crop>> GetAllCropsAsync();

}
