using Application.UseCases.DeleteZipFile.Interfaces;
using Application.UseCases.DeleteZipFile.Models;
using Application.UseCases.GetZipFile.Interfaces;
using Application.UseCases.ListZipStructureFiles.Interfaces;
using Application.UseCases.UploadZipFile.Interfaces;
using Application.UseCases.UploadZipFile.Models;
using Application.UseCases.ValidateZipFile.Interfaces;
using Application.UseCases.ValidateZipFile.Models;
using Domain.Models.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ZipsController : ControllerBase
    {
        private readonly IUploadZipFileUseCase _uploadZipFileUseCase;
        private readonly IGetZipFileUseCase _getZipFileUseCase;
        private readonly IListZipStructureFilesUseCase _listZipStructureUseCase;
        private readonly IValidateZipFileUseCase _validateZipFileUseCase;
        private readonly IDeleteZipFileUseCase _deleteZipFileUseCase;

        private readonly IValidator<IFormFile> _zipFileValidator;
        private readonly IValidator<string> _getZipFileValidator;

        public ZipsController(
            IUploadZipFileUseCase uploadZipFileUseCase,
            IGetZipFileUseCase getZipFileUseCase,
            IListZipStructureFilesUseCase listZipStructureUseCase,
            IValidateZipFileUseCase validateZipFileUseCase,
            IDeleteZipFileUseCase deleteZipFileUseCase,
            IValidator<IFormFile> zipFileValidator,
            IValidator<string> getZipFileValidator)
        {
            _uploadZipFileUseCase = uploadZipFileUseCase;
            _getZipFileUseCase = getZipFileUseCase;
            _listZipStructureUseCase = listZipStructureUseCase;
            _validateZipFileUseCase = validateZipFileUseCase;
            _deleteZipFileUseCase = deleteZipFileUseCase;

            _zipFileValidator = zipFileValidator;
            _getZipFileValidator = getZipFileValidator;
        }

        /// <summary>
        /// List all zip files structure
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<FolderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFilesFolderStructures(CancellationToken cancellationToken)
        {
            var filesFolderStructure = await _listZipStructureUseCase.GetFilesFolderStructuresAsync(cancellationToken);

            return Ok(filesFolderStructure);
        }

        /// <summary>
        /// Download an uploaded file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GlobalErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFile(string fileName, CancellationToken cancellationToken)
        {
            var validationResult = await _getZipFileValidator.ValidateAsync(fileName, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var mimeType = "application/zip";
            var file = await _getZipFileUseCase.GetZipFileAsync(fileName, cancellationToken);

            if (file == null)
            {
                return NoContent();
            }

            return File(file, mimeType, fileName);
        }

        /// <summary>
        /// Upload a new valid zip file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GlobalErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {

            await _zipFileValidator.ValidateAndThrowAsync(file, cancellationToken);

            var input = new UploadZipFileInput
            {
                FileStream = file.OpenReadStream(),
                FileName = file.FileName,
            };

            await _uploadZipFileUseCase.UploadZipFileAsync(input, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Validate a zip file to see if upload process will succeed
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidateFile(IFormFile file, CancellationToken cancellationToken)
        {
            await _zipFileValidator.ValidateAndThrowAsync(file, cancellationToken);

            var input = new ValidateZipFileInput
            {
                FileStream = file.OpenReadStream(),
                FileName = file.FileName,
            };

            await _validateZipFileUseCase.ValidateZipFileAsync(input, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Delete a previously uploaded file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFile(string fileName, CancellationToken cancellationToken)
        {
            var input = new DeleteZipFileInput { FileName = fileName };

            await _deleteZipFileUseCase.DeleteZipFileAsync(input, cancellationToken);

            return Ok();
        }
    }
}
