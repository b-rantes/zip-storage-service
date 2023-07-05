using FluentValidation;
using System.IO.Compression;

namespace WebApi.Controllers.v1.Validators
{
    public class ZipFileValidator : AbstractValidator<IFormFile>
    {
        private const string InvalidZipFileErrorMessage = "Invalid zip file";

        public ZipFileValidator()
        {
            RuleFor(x => x.Length).NotNull().GreaterThan(0).WithMessage(InvalidZipFileErrorMessage);

            RuleFor(x => x.FileName).Custom((file, context) =>
            {
                if (!Path.GetExtension(file).Equals(".zip") || file.Count(x => x == '.') > 1)
                    context.AddFailure(InvalidZipFileErrorMessage);

                if (file.Contains("/") || file.Contains("\\"))
                    context.AddFailure(InvalidZipFileErrorMessage);
            });

            RuleFor(x => x).Custom((file, context) =>
            {
                try
                {
                    var zipArchive = new ZipArchive(file.OpenReadStream());
                }
                catch (Exception)
                {
                    context.AddFailure(InvalidZipFileErrorMessage);
                }
            });
        }
    }
}
