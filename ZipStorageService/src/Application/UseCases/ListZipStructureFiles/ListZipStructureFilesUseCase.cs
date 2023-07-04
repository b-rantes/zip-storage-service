using Application.UseCases.ListZipStructureFiles.Interfaces;
using Domain.Interfaces.Repository;
using Domain.Models.DTOs;

namespace Application.UseCases.ListZipStructureFiles
{
    public class ListZipStructureFilesUseCase : IListZipStructureFilesUseCase
    {
        private readonly IZipFileRepository _repository;

        public ListZipStructureFilesUseCase(IZipFileRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FolderDTO>> GetFilesFolderStructuresAsync(CancellationToken cancellationToken)
        {
            var folders = await _repository.GetAllFilesStructure(cancellationToken);

            return folders;
        }
    }
}
