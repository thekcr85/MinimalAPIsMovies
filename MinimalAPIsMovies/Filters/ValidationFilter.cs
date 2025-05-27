
using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Filters
{
	public class ValidationFilter<T> : IEndpointFilter
	{
		public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
		{
			var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();

			if (validator == null)
			{
				return await next(context);
			}

			var obj = context.Arguments.OfType<T>().FirstOrDefault();

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
