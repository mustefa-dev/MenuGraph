using WebApplication1.Services;

namespace WebApplication1.GraphQL;

public class Query
{
    private readonly IDrinkRepository _drinkRepository;

    public Query(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<List<Drink>> GetDrinks()
    {
        return await _drinkRepository.GetDrinksAsync();
    }

    public async Task<Drink> GetDrink(Guid id)
    {
        return await _drinkRepository.GetDrinkAsync(id);
    }
}
