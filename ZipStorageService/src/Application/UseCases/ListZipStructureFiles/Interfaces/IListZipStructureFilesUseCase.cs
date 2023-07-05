using Domain.Models.DTOs;

namespace Application.UseCases.ListZipStructureFiles.Interfaces
{
    public interface IListZipStructureFilesUseCase
    {
        public Task<List<FolderDTO>> GetFilesFolderStructuresAsync(CancellationToken cancellationToken);
    }
}
