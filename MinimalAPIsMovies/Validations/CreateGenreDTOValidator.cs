using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Validations
{
	public class CreateGenreDTOValidator : AbstractValidator<CreateGenreDTO>
	{
		public CreateGenreDTOValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("The field {PropertyName} is required")
				.MaximumLength(50).WithMessage("The field {PropertyName} must not exceed {MaxLength} characters")
				.Must(FirstLetterIsUpperCase).WithMessage("The field {PropertyName} should start with uppercase");
		}

		private static bool FirstLetterIsUpperCase(string value)
		{
			return string.IsNullOrEmpty(value) || char.IsUpper(value[0]);
		}

	}
}
