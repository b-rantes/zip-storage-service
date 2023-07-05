using System.IO.Compression;

namespace Domain.Models.DTOs
{
    public sealed class ZipArchiveDTO
    {
        public ZipArchiveDTO(Stream zipArchive, string fileName)
        {
            ZipArchive = new ZipArchive(zipArchive);
            FileName = fileName;
        }
        public ZipArchive ZipArchive { get; set; }
        public string FileName { get; set; }
    }
}
