using Domain.Exceptions;
using Domain.Models.DTOs;
using Domain.Services.ValidationRules;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests.Domain.ValidationRules.InvalidImagesValidationRulesTests
{
    public class InvalidImagesEmptyTests
    {
        private readonly InvalidImagesValidationRule _validationRule;

        public InvalidImagesEmptyTests()
        {
            _validationRule = new InvalidImagesValidationRule();
        }

        [Fact(DisplayName = "Should throw when images folder is empty")]
        public async Task Should_Throw_When_Empty_Image_Folder()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileImagesEmpty);

            //Act, Assert
            await Assert.ThrowsAsync<InvalidFileStructureException>(
                async () => await _validationRule.Validate(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should throw when images has invalid file extension")]
        public async Task Should_Throw_When_Invalid_File_Extension()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileImagesFormat);

            //Act, Assert
            await Assert.ThrowsAsync<InvalidFileStructureException>(
                async () => await _validationRule.Validate(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should validate correctly")]
        public async Task Should_Execute_Successfully_When_Valid_File()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.ValidZipFile);

            //Act
            var validationTask = _validationRule.Validate(input, CancellationToken.None);
            await validationTask;
        
            //Assert
            validationTask.Exception.Should().BeNull();
        }

        public ZipArchiveDTO CreateInput(string zipFileReference) =>
            new ZipArchiveDTO(
                zipArchive: ZipFileFixtureGetter.GetZipFile(zipFileReference),
                fileName: zipFileReference.Split("/").Last()
                );
    }
}
