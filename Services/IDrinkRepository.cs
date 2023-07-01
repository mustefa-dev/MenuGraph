namespace WebApplication1.Services;

public interface IDrinkRepository
{
    Task<List<Drink>> GetDrinksAsync();
    Task<Drink> GetDrinkAsync(Guid id);
    Task<Drink> CreateDrinkAsync(Drink drink);
    Task<Drink> UpdateDrinkAsync(Drink drink);
    Task<bool> DeleteDrinkAsync(Guid id);
}
