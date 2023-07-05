using Application.UseCases.UploadZipFile.Models;

namespace Application.UseCases.UploadZipFile.Interfaces
{
    public interface IUploadZipFileUseCase
    {
        public Task UploadZipFileAsync(UploadZipFileInput input, CancellationToken cancellationToken);
    }
}
