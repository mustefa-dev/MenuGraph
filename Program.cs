using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<DrinkContext>(options =>
    options.UseInMemoryDatabase("DrinkDatabase"));

builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<DrinkType>()
    .AddType<ReviewType>();

var app = builder.Build();

// Configure the app
app.UseRouting();

app.MapGraphQL("/graphql"); // Use top-level route registration

app.Run();

public class Drink
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }

    // Remove [Required] attribute to make the Reviews field optional
    public List<Review> Reviews { get; set; }
}

public class Review
{
    public Guid Id { get; set; }
    public Guid DrinkId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }

    public Drink Drink { get; set; }
}

public class DrinkContext : DbContext
{
    public DrinkContext(DbContextOptions<DrinkContext> options)
        : base(options)
    {
    }

    public DbSet<Drink> Drinks { get; set; }
    public DbSet<Review> Reviews { get; set; }
}

public interface IDrinkRepository
{
    Task<List<Drink>> GetDrinksAsync();
    Task<Drink> GetDrinkAsync(Guid id);
    Task<Drink> CreateDrinkAsync(Drink drink);
    Task<Drink> UpdateDrinkAsync(Drink drink);
    Task<bool> DeleteDrinkAsync(Guid id);
}

public class DrinkRepository : IDrinkRepository
{
    private readonly DrinkContext _context;

    public DrinkRepository(DrinkContext context)
    {
        _context = context;
    }

    public async Task<List<Drink>> GetDrinksAsync()
    {
        return await _context.Drinks.ToListAsync();
    }

    public async Task<Drink> GetDrinkAsync(Guid id)
    {
        return await _context.Drinks.FindAsync(id);
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

public class DrinkType : ObjectType<Drink>
{
    protected override void Configure(IObjectTypeDescriptor<Drink> descriptor)
    {
        descriptor.Field(d => d.Id).Type<IdType>();
        descriptor.Field(d => d.Name).Type<NonNullType<StringType>>();
        descriptor.Field(d => d.Type).Type<NonNullType<StringType>>();
        descriptor.Field(d => d.Price).Type<NonNullType<DecimalType>>();
        descriptor.Field(d => d.Reviews).Ignore();
    }

}

public class ReviewType : ObjectType<Review>
{
    protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
    {
        descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
        descriptor.Field(r => r.DrinkId).Type<NonNullType<IdType>>();
        descriptor.Field(r => r.Rating).Type<NonNullType<IntType>>();
        descriptor.Field(r => r.Comment).Type<StringType>();
        descriptor.Field(r => r.Drink).Ignore();
    }
}

public class Mutation
{
    public async Task<Drink> CreateDrink(Drink drink, [Service] IDrinkRepository drinkRepository)
    {
        // Generate a new Guid for the ID
        drink.Id = Guid.NewGuid();

        return await drinkRepository.CreateDrinkAsync(drink);
    }


    public async Task<Drink> UpdateDrink(Guid id, Drink drink, [Service] IDrinkRepository drinkRepository)
    {
        var existingDrink = await drinkRepository.GetDrinkAsync(id);
        if (existingDrink == null)
        {
            throw new ArgumentException($"Drink with ID {id} not found.");
        }

        existingDrink.Name = drink.Name;
        existingDrink.Type = drink.Type;
        existingDrink.Price = drink.Price;

        return await drinkRepository.UpdateDrinkAsync(existingDrink);
    }

    public async Task<bool> DeleteDrink(Guid id, [Service] IDrinkRepository drinkRepository)
    {
        return await drinkRepository.DeleteDrinkAsync(id);
    }
}

public class Query
{
    public async Task<List<Drink>> GetDrinks([Service] IDrinkRepository drinkRepository)
    {
        return await drinkRepository.GetDrinksAsync();
    }

    public async Task<Drink> GetDrink(Guid id, [Service] IDrinkRepository drinkRepository)
    {
        return await drinkRepository.GetDrinkAsync(id);
    }
}
