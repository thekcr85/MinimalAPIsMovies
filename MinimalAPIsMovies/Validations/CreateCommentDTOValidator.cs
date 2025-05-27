using FluentValidation;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.Validations
{
	public class CreateCommentDTOValidator : AbstractValidator<CreateCommentDTO>
	{
		public CreateCommentDTOValidator()
		{
			RuleFor(x => x.Body)
				.NotEmpty().WithMessage(ValidationMessages.NonEmpty);
		}
	}
}
