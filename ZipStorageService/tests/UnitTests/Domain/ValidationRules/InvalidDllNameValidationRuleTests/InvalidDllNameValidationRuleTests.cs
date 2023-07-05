using Domain.Exceptions;
using Domain.Models.DTOs;
using Domain.Services.ValidationRules;
using FluentAssertions;
using UnitTests.Helpers;

namespace UnitTests.Domain.ValidationRules.InvalidDllNameValidationRuleTests
{
    public class InvalidDllNameValidationRuleTests
    {
        private readonly InvalidDllNameValidationRule _validationRule;

        public InvalidDllNameValidationRuleTests()
        {
            _validationRule = new InvalidDllNameValidationRule();
        }

        [Fact(DisplayName = "Should throw when invalid dll Name")]
        public async Task Should_Throw_When_Invalid_Dll_Name()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileDllName);

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
