namespace Domain.Models.DTOs
{
    public class FolderDTO
    {
        public string RootFolderName { get; set; }
        public List<string> DllsFolder { get; set; }
        public List<string> ImagesFolder { get; set; }
        public List<string> LanguagesFolder { get; set; }
    }
}
