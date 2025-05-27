using FluentValidation;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.Validations
{
	public class CreateMovieDTOValidator : AbstractValidator<CreateMovieDTO>
	{
		public CreateMovieDTOValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage(ValidationMessages.NonEmpty)
				.MaximumLength(200).WithMessage(ValidationMessages.MaximumLength);
		}
	}
}
