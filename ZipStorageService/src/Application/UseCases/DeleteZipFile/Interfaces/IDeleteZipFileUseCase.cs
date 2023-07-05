using Application.UseCases.DeleteZipFile.Models;

namespace Application.UseCases.DeleteZipFile.Interfaces
{
    public interface IDeleteZipFileUseCase
    {
        public Task DeleteZipFileAsync(DeleteZipFileInput input, CancellationToken cancellationToken);
    }
}
