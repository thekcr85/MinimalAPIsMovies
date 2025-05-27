using AutoMapper;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Filters
{
	public class TestFilter : IEndpointFilter
	{
		public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
		{
			// You can add your custom logic here before the endpoint is executed
			// Below is an example of how to access the parameters passed to the endpoint
			var param1 = context.Arguments.OfType<int>().FirstOrDefault();
			var param2 = context.Arguments.OfType<IGenreRepository>().FirstOrDefault();
			var param3 = context.Arguments.OfType<IMapper>().FirstOrDefault();

			// Now
			var result = await next(context);
			// You can add your custom logic here after the endpoint is executed
			// For example, modifying the response
			return result;
		}
	}
	{
	}
}
