using FluentValidation;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.Validations
{
	public class CreateGenreDTOValidator : AbstractValidator<CreateGenreDTO>
	{
		public CreateGenreDTOValidator(IGenreRepository genreRepository, IHttpContextAccessor httpContextAccessor)
		{
			var routeValueId = httpContextAccessor.HttpContext!.Request.RouteValues["id"];
			var id = 0;

			if (routeValueId is string routeValueIdString)
			{
				int.TryParse(routeValueIdString, out id); // Parse the ID from the route value if it exists
			}

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidationMessages.NonEmpty)
				.MaximumLength(50).WithMessage(ValidationMessages.MaximumLength)
				.Must(ValidationHelpers.FirstLetterIsUpperCase).WithMessage(ValidationMessages.FirstLetterIsUpperCase)
				.MustAsync(async (name, _) =>
				{
					var exists = await genreRepository.Exists(id, name);
					return !exists; // Ensure the genre name does not already exist
				}).WithMessage(g => $"A genre with the name {g.Name} already exists");
		}
	}
}
