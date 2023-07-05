using Application.UseCases.GetZipFile.Interfaces;
using Domain.Interfaces.Repository;

namespace Application.UseCases.GetZipFile
{
    public class GetZipFileUseCase : IGetZipFileUseCase
    {
        private readonly IZipFileRepository _repository;
        public GetZipFileUseCase(IZipFileRepository repository)
        {
            _repository = repository;
        }

        public Task<FileStream?> GetZipFileAsync(string fileName, CancellationToken cancellationToken)
        {
            var file = _repository.GetFile(fileName, cancellationToken);

            return Task.FromResult(file);
        }
    }
}
