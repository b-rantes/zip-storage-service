namespace Domain.Models.Entities
{
    public sealed class ZipFileEntity
    {
        public ZipFileEntity(string fileName, Stream fileStream)
        {
            FileName = fileName;
            FileStream = fileStream;
        }

        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
