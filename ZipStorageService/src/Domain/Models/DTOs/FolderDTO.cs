namespace Domain.Models.DTOs
{
    public class FolderDTO
    {
        public string RootFolderName { get; set; }
        public List<string> DllFolder { get; set; }
        public List<string> ImagesFolder { get; set; }
        public List<string> LanguageFolder { get; set; }
    }
}
