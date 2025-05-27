
using FluentValidation;
using Microsoft.Identity.Client;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Filters
{
	public class GenresValidationFilter : IEndpointFilter
	{
		public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
		{
			var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<CreateGenreDTO>>();

			if (validator == null)
			{
				return await next(context);
			}

			var obj = context.Arguments.OfType<CreateGenreDTO>().FirstOrDefault();

			if (obj == null)
			{
				return Results.Problem("Invalid request body. Expected CreateGenreDTO.");
			}

			var validationResult = await validator.ValidateAsync(obj);

			if (!validationResult.IsValid)
			{
				return Results.ValidationProblem(validationResult.ToDictionary());
			}

			return await next(context);
		}
	}
}
