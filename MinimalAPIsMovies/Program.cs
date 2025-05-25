using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Endpoints;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

builder.Services.AddTransient<IFileStorage, LocalFileStorage>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

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

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();
app.MapGroup("/movies").MapMovies();

app.Run();