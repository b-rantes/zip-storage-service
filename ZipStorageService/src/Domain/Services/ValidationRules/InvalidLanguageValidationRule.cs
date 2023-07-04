using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.Models.DTOs;
using System.IO.Compression;

namespace Domain.Services.ValidationRules
{
    public class InvalidLanguageValidationRule : IValidationRule
    {
        private string[] LanguageCodes = new string[] {
            "ar", "bg", "ca", "cs", "da", "de", "el", "en", "es", "eu", "fi", "fr", "hi",
            "hu", "id", "it", "ja", "ko", "no", "nl", "pl", "pt", "ro", "ru", "sv", "tr", "zh" };

        private const string LanguagesPath = "languages/";
        private const string XmlExtension = ".xml";

        private const string InvalidLanguageConventionErrorMessage =
            "Invalid languages file found (extension, language code or name convention)";

        private const string EnLanguageCodeNotFound = "English language file not found";

        public Task Validate(ZipArchiveDTO archive, CancellationToken cancellationToken)
        {
            var languagesArchives = archive.ZipArchive.Entries.Where(x => x.FullName.StartsWith(LanguagesPath) && x.Length > 0);

            var rootFileNameWithoutExtension = archive.FileName.Split(".").FirstOrDefault();

            var englishLanguageFound = false;

            foreach (var language in languagesArchives)
            {
                if (!HasValidNameConvetion(language, rootFileNameWithoutExtension!))
                {
                    return Task.FromException(new InvalidFileStructureException(InvalidLanguageConventionErrorMessage));
                }

                if (IsEnglishLanguageCode(language) && !englishLanguageFound)
                {
                    englishLanguageFound = true;
                }
            }

            if (!englishLanguageFound)
            {
                return Task.FromException(new InvalidFileStructureException(EnLanguageCodeNotFound));
            }

            return Task.CompletedTask;
        }

        private bool IsEnglishLanguageCode(ZipArchiveEntry language)
        {
            try
            {
                var languageCode = language.Name.Split(".").FirstOrDefault()!.Split("_").LastOrDefault();

                if (languageCode == "en") return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool HasValidNameConvetion(ZipArchiveEntry language, string rootFileNameWithoutExtension)
        {
            try
            {
                var fileNameWithoutExtension = language.Name.Split(".").FirstOrDefault();
                var fileExtension = "." + language.Name.Split(".").LastOrDefault();
                var fileNameBeforeUnderline = fileNameWithoutExtension!.Split("_").First();
                var fileNameLanguageCode = fileNameWithoutExtension!.Split("_").Last();

                if (fileNameBeforeUnderline != rootFileNameWithoutExtension) return false;

                if (!LanguageCodes.Contains(fileNameLanguageCode)) return false;

                if (fileExtension != XmlExtension) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
