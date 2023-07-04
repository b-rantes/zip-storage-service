using Application.UseCases.DeleteZipFile.Models;
using FluentValidation;

namespace Application.UseCases.DeleteZipFile.Validator
{
    public class DeleteZipFileInputValidator : AbstractValidator<DeleteZipFileInput>
    {
        private const string InvalidZipFileNameErrorMessage = "Invalid zip file name";
        public DeleteZipFileInputValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().NotNull();
            RuleFor(x => x.FileName).Custom((fileName, context) =>
            {
                if (!Path.GetExtension(fileName).Equals(".zip") || fileName.Count(x => x == '.') > 1)
                    context.AddFailure(InvalidZipFileNameErrorMessage);

                if (fileName.Contains("/") || fileName.Contains("\\"))
                    context.AddFailure(InvalidZipFileNameErrorMessage);
            });
        }
    }
}
