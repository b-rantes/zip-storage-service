using Domain.Exceptions;
using Domain.Models.DTOs;
using Domain.Services.ValidationRules;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests.Domain.ValidationRules.InvalidLanguageValidationRuleTests
{
    public class InvalidLanguageValidationRuleTests
    {
        private readonly InvalidLanguageValidationRule _validationRule;

        public InvalidLanguageValidationRuleTests()
        {
            _validationRule = new InvalidLanguageValidationRule();
        }

        [Fact(DisplayName = "Should throw when name is not the same as root folder")]
        public async Task Should_Throw_When_Invalid_RootNameFile()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileLanguagesInvalidRootName);

            //Act, Assert
            await Assert.ThrowsAsync<InvalidFileStructureException>(
                async () => await _validationRule.Validate(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should throw when invalid language code")]
        public async Task Should_Throw_When_Invalid_Language_Code()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileLanguagesInvalidLanguageCode);

            //Act, Assert
            await Assert.ThrowsAsync<InvalidFileStructureException>(
                async () => await _validationRule.Validate(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should throw when en language code not found")]
        public async Task Should_Throw_When_En_LanguageCode_Not_Found()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileLanguagesEnNotFound);

            //Act, Assert
            await Assert.ThrowsAsync<InvalidFileStructureException>(
                async () => await _validationRule.Validate(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should throw when some file not xml")]
        public async Task Should_Throw_When_Invalid_File_Extension()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileLanguagesFileNotXml);

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
