using AutoMapper;
using System;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.GraphQL
{
    public class Mutation
    {
        private readonly IDrinkRepository _drinkRepository;

        public Mutation(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        public async Task<Drink> CreateDrink(Drink drink)
        {
            drink.Id = Guid.NewGuid();
            return await _drinkRepository.CreateDrinkAsync(drink);
        }

        public async Task<Drink> UpdateDrink(Guid id, Drink drink)
        {
            var existingDrink = await _drinkRepository.GetDrinkAsync(id);
            if (existingDrink == null)
            {
                throw new ArgumentException($"Drink with ID {id} not found.");
            }

            existingDrink.Name = drink.Name;
            existingDrink.Type = drink.Type;
            existingDrink.Price = drink.Price;

            return await _drinkRepository.UpdateDrinkAsync(existingDrink);
        }

        public async Task<bool> DeleteDrink(Guid id)
        {
            return await _drinkRepository.DeleteDrinkAsync(id);
        }
    }

}