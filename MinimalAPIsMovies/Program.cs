using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
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

var genresEnpoints = app.MapGroup("/genres");




app.Run();



// Endpoints with lambda expressions

//genresEnpoints.MapGet("/", async (IGenreRepository genreRepository) =>
//{
//	return await genreRepository.GetAll();
//}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("GetGenres"));

//genresEnpoints.MapGet("/{id}", async (int id, IGenreRepository genreRepository) =>
//{
//	var genre = await genreRepository.GetById(id);
//	return genre is not null ? Results.Ok(genre) : Results.NotFound();
//});

//genresEnpoints.MapPost("/", async (Genre genre, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore) =>
//{
//	var id = await genreRepository.Create(genre);
//	await outputCacheStore.EvictByTagAsync("GetGenres", default);
//	return Results.Created($"/genres/{id}", genre);
//});

//genresEnpoints.MapPut("/{id}", async (int id, Genre genre, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore) =>
//{
//	var exists = await genreRepository.Exists(id);
//	if (!exists)
//	{
//		return Results.NotFound();
//	}
//	await genreRepository.Update(genre);
//	await outputCacheStore.EvictByTagAsync("GetGenres", default);
//	return Results.NoContent();
//});

//genresEnpoints.MapDelete("/{id}", async (int id, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore) =>
//{
//	var exists = await genreRepository.Exists(id);
//	if (!exists)
//	{
//		return Results.NotFound();
//	}
//	await genreRepository.Delete(id);
//	await outputCacheStore.EvictByTagAsync("GetGenres", default);
//	return Results.NoContent();
//});