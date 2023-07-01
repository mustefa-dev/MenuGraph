using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.GraphQL;
using WebApplication1.Services;
using WebApplication1.Types;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<DrinkContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();

builder.Services.AddAutoMapper(typeof(DrinkRepository));

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<DrinkType>();

var app = builder.Build();

// Configure the app
app.UseRouting();

app.MapGraphQL("/graphql"); // Use top-level route registration

app.Run();