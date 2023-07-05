using Application.UseCases.ValidateZipFile.Interfaces;
using Application.UseCases.ValidateZipFile.Models;
using Domain.Interfaces.Services;
using Domain.Models.Entities;

namespace Application.UseCases.ValidateZipFile
{
    public class ValidateZipFileUseCase : IValidateZipFileUseCase
    {
        private readonly IZipFileService _zipFileService;

        public ValidateZipFileUseCase(IZipFileService zipFileService)
        {
            _zipFileService = zipFileService;
        }

        public async Task ValidateZipFileAsync(ValidateZipFileInput input, CancellationToken cancellationToken)
        {
            var fileEntity = new ZipFileEntity(input.FileName, input.FileStream);

            await _zipFileService.ValidateFile(fileEntity, cancellationToken);
        }
    }
}
