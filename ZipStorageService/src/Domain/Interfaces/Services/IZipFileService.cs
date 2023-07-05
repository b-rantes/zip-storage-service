using Domain.Models.Entities;

namespace Domain.Interfaces.Services
{
    public interface IZipFileService
    {
        public Task ValidateFile(ZipFileEntity input, CancellationToken cancellationToken);
    }
}
