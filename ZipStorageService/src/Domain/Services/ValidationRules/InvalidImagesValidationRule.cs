using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.Models.DTOs;

namespace Domain.Services.ValidationRules
{
    public class InvalidImagesValidationRule : IValidationRule
    {
        private const string ImagesPath = "images/";
        private string[] AllowedExtensions = new string[] { "jpg", "jpeg", "png" };

        private const string EmptyFolderErrorMessage = "Images folder can't be empty";
        private const string InvalidImageFormatErrorMessage = "Invalid images extension";

        public Task Validate(ZipArchiveDTO archive, CancellationToken cancellationToken)
        {
            var imagesArchives = archive.ZipArchive.Entries.Where(x => x.FullName.StartsWith(ImagesPath) && x.Length > 0);

            if (imagesArchives.Count() == 0)
            {
                return Task.FromException(new InvalidFileStructureException(EmptyFolderErrorMessage));
            }

            if (HasFileWithInvalidExtension(imagesArchives))
            {
                return Task.FromException(new InvalidFileStructureException(InvalidImageFormatErrorMessage));
            }

            return Task.CompletedTask;
        }

        private bool HasFileWithInvalidExtension(IEnumerable<System.IO.Compression.ZipArchiveEntry> imagesArchives)
        {
            return imagesArchives.Any(x => !AllowedExtensions.Contains(GetExtension(x.FullName)));
        }
        private string GetExtension(string fullName) => fullName.Split(".").Last();
    }
}
