using Application.UseCases.DeleteZipFile.Interfaces;
using Application.UseCases.DeleteZipFile.Models;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using FluentValidation;

namespace Application.UseCases.DeleteZipFile
{
    public class DeleteZipFileUseCase : IDeleteZipFileUseCase
    {
        private readonly IZipFileRepository _zipFileRepository;
        private readonly IValidator<DeleteZipFileInput> _validator;

        public DeleteZipFileUseCase(IZipFileRepository zipFileRepository, IValidator<DeleteZipFileInput> validator)
        {
            _zipFileRepository = zipFileRepository;
            _validator = validator;
        }

        public async Task DeleteZipFileAsync(DeleteZipFileInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new InvalidFileFormatException(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
            }

            await _zipFileRepository.DeleteFile(input.FileName, cancellationToken);
        }
    }
}
