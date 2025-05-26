using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Validations
{
	public class CreateActorDTOValidator : AbstractValidator<CreateActorDTO>
	{
		public CreateActorDTOValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("The field {PropertyName} is required")
				.MaximumLength(50).WithMessage("The field {PropertyName} must not exceed {MaxLength} characters");

			var minimumDate = new DateTime(1900, 1, 1);

			RuleFor(x => x.DateOfBirth)
				.GreaterThanOrEqualTo(minimumDate).WithMessage("The field {PropertyName} must be greater than or equal to " + minimumDate.ToString("yyyy-MM-dd"));

		}
	}
}
