namespace Application.UseCases.GetZipFile.Interfaces
{
    public interface IGetZipFileUseCase
    {
        public Task<FileStream?> GetZipFileAsync(string fileName, CancellationToken cancellationToken);
    }
}
