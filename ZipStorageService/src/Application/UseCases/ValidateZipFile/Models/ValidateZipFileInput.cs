namespace Application.UseCases.ValidateZipFile.Models
{
    public class ValidateZipFileInput
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
