using Application.UseCases.ValidateZipFile.Models;

namespace Application.UseCases.ValidateZipFile.Interfaces
{
    public interface IValidateZipFileUseCase
    {
        public Task ValidateZipFileAsync(ValidateZipFileInput input, CancellationToken cancellationToken);
    }
}
