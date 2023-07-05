using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.Models.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Domain.Services.ValidationRules
{
    internal class InvalidDllNameValidationRule : IValidationRule
    {
        private const string DllPath = "dlls/";
        private const string DllExtension = ".dll";

        private const string ErrorMessage = "RootFolder dll name not found";

        public Task Validate(ZipArchiveDTO archive, CancellationToken cancellationToken)
        {
            var dllArchives = archive.ZipArchive.Entries.Where(x => x.FullName.StartsWith(DllPath) && x.Length > 0).ToList();

            var rootFileNameWithoutExtension = archive.FileName.Split(".").FirstOrDefault();

            if (!dllArchives.Any(x => x.Name == rootFileNameWithoutExtension + DllExtension))
            {
                return Task.FromException(new InvalidFileStructureException(ErrorMessage));
            }

            return Task.CompletedTask;
        }
    }
}
