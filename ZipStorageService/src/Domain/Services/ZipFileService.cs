using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.Models.DTOs;
using Domain.Models.Entities;

namespace Domain.Services
{
    public sealed class ZipFileService : IZipFileService
    {
        private readonly List<IValidationRule> _validationRules;

        public ZipFileService(IEnumerable<IValidationRule> validationRules)
        {
            _validationRules = validationRules.ToList();
        }

        public async Task ValidateFile(ZipFileEntity input, CancellationToken cancellationToken)
        {
            try
            {
                var archiveDto = new ZipArchiveDTO(input.FileStream, input.FileName);

                for (int i = 0; i < _validationRules.Count; i++)
                {
                    var validator = _validationRules[i];

                    await validator.Validate(archiveDto, cancellationToken);
                }
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is ArgumentNullException ||
                ex is InvalidDataException
            )
            {
                throw new InvalidFileFormatException(input.FileName);
            }
        }
    }
}
