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

app.MapGet("/genres", () =>
{
	var genres = new List<Genre>
	{
		new Genre { Id = 1, Name = "Action" },
		new Genre { Id = 2, Name = "Comedy" },
		new Genre { Id = 3, Name = "Drama" },
		new Genre { Id = 4, Name = "Fantasy" },
		new Genre { Id = 5, Name = "Horror" },
		new Genre { Id = 6, Name = "Romance" },
		new Genre { Id = 7, Name = "Sci-Fi" },
		new Genre { Id = 8, Name = "Thriller" }
	};

	return Results.Ok(genres);
});

app.MapPost("/genres", async (Genre genre, IGenreRepository genreRepository) => 
{
	var id = await genreRepository.Create(genre); // Call the repository to create the genre
	return Results.Created($"/genres/{id}", genre); // Return the created genre with a 201 status code
});

app.Run();
