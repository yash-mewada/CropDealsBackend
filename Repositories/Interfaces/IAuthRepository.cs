namespace CropDeals.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> SignInAsync(SignInModel model);
    }
}
