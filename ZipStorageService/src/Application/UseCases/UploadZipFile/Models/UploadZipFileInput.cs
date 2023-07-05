namespace Application.UseCases.UploadZipFile.Models
{
    public class UploadZipFileInput
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
