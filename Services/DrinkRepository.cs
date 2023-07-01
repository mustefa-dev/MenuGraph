using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly DrinkContext _context;
        private readonly IMapper _mapper;

        public DrinkRepository(DrinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Drink>> GetDrinksAsync()
        {
            var drinks = await _context.Drinks.ToListAsync();
            return _mapper.Map<List<Drink>>(drinks);
        }

        public async Task<Drink> GetDrinkAsync(Guid id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            return _mapper.Map<Drink>(drink);
        }

        public async Task<Drink> CreateDrinkAsync(Drink drink)
        {
            _context.Drinks.Add(drink);
            await _context.SaveChangesAsync();
            return drink;
        }

        public async Task<Drink> UpdateDrinkAsync(Drink drink)
        {
            _context.Entry(drink).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return drink;
        }

        public async Task<bool> DeleteDrinkAsync(Guid id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return false;
            }

            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}