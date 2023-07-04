using Domain.Models.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IValidationRule
    {
        public Task Validate(ZipArchiveDTO archive, CancellationToken cancellationToken);
    }
}
