using Domain.Models.DTOs;
using Domain.Models.Entities;

namespace Domain.Interfaces.Repository
{
    public interface IZipFileRepository
    {
        public Task SaveFile(ZipFileEntity zipFile, CancellationToken cancellationToken);
        public FileStream? GetFile(string fileName, CancellationToken cancellationToken);
        public Task<List<FolderDTO>> GetAllFilesStructure(CancellationToken cancellationToken);
        public Task DeleteFile(string fileName, CancellationToken cancellationToken);
    }
}
