using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Validations
{
	public class CreateGenreDTOValidator : AbstractValidator<CreateGenreDTO>
	{
		public CreateGenreDTOValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty();
		}
	}
}
