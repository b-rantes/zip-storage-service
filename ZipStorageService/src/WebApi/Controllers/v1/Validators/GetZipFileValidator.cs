using FluentValidation;
using System.Linq;

namespace WebApi.Controllers.v1.Validators
{
    public class GetZipFileValidator : AbstractValidator<string>
    {
        private const string InvalidFileNameErrorMessage = "Invalid file name";
        public GetZipFileValidator()
        {
            RuleFor(x => x).MinimumLength(1);
            RuleFor(x => x).Custom((fileName, context) =>
            {
                if (fileName.Count(y => y == '.') != 1)
                    context.AddFailure(InvalidFileNameErrorMessage);

                if (fileName.Contains("/") || fileName.Contains("\\"))
                    context.AddFailure(InvalidFileNameErrorMessage);
            });
        }
    }
}
