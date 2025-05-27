using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.Validations
{
	public class CreateActorDTOValidator : AbstractValidator<CreateActorDTO>
	{
		public CreateActorDTOValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidationMessages.NonEmpty)
				.MaximumLength(50).WithMessage(ValidationMessages.MaximumLength);

			var minimumDate = new DateTime(1900, 1, 1);

			RuleFor(x => x.DateOfBirth)
				.GreaterThanOrEqualTo(minimumDate).WithMessage(ValidationMessages.GreaterThanDate(minimumDate));

		}
	}
}
