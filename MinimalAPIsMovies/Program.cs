using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGenreRepository, GenreRepository>();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddOutputCache();

var app = builder.Build();

app.UseCors();

app.UseOutputCache();

app.MapGet("/genres", async (IGenreRepository genreRepository) =>
{
	return await genreRepository.GetAll();
});

app.MapGet("/genres/{id}", async (int id, IGenreRepository genreRepository) =>
{
	var genre = await genreRepository.GetById(id);
	return genre is not null ? Results.Ok(genre) : Results.NotFound();
});

app.MapPost("/genres", async (Genre genre, IGenreRepository genreRepository) =>
{
	var id = await genreRepository.Create(genre);
	return Results.Created($"/genres/{id}", genre);
});

app.Run();
