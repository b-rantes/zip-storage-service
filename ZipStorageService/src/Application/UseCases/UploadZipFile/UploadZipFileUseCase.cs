using Application.UseCases.UploadZipFile.Interfaces;
using Application.UseCases.UploadZipFile.Models;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Models.Entities;

namespace Application.UseCases.UploadZipFile
{
    public class UploadZipFileUseCase : IUploadZipFileUseCase
    {
        private readonly IZipFileRepository _repository;
        private readonly IZipFileService _service;

        public UploadZipFileUseCase(IZipFileRepository repository, IZipFileService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task UploadZipFileAsync(UploadZipFileInput input, CancellationToken cancellationToken)
        {
            var fileEntity = new ZipFileEntity(input.FileName, input.FileStream);

            var file = _repository.GetFile(fileEntity.FileName, cancellationToken);
            if (file is not null)
            {
                throw new FileAlreadyExistsException(fileEntity.FileName);
            }

            await _service.ValidateFile(fileEntity, cancellationToken);

            await _repository.SaveFile(fileEntity, cancellationToken);
        }
    }
}