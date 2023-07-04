using AutoFixture;
using Domain.Interfaces.Services;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using Domain.Services;
using Moq;
using Moq.AutoMock;
using UnitTests.Helpers;

namespace UnitTests.Domain.FileValidatorServiceTests
{
    public class FileValidatorServiceTests
    {
        private ZipFileService _service;
        private Fixture _fixture;
        private AutoMocker _autoMocker;

        public FileValidatorServiceTests()
        {
            _autoMocker = new AutoMocker();
            _fixture = new Fixture();
            _autoMocker.Use<IEnumerable<IValidationRule>>(new List<IValidationRule>()
            {
                _autoMocker.GetMock<IValidationRule>().Object
            });
            _service = _autoMocker.CreateInstance<ZipFileService>();
        }

        [Fact(DisplayName = "Should throw exception when some validator throws")]
        public async Task Should_Throw_When_Invalid_Dll_Name()
        {
            //Arrange
            var input = CreateInput();
            _autoMocker.GetMock<IValidationRule>().Setup(x => x.Validate(It.IsAny<ZipArchiveDTO>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _service.ValidateFile(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should execute successfully when no validator throws")]
        public async Task Should_Execute_Successfully_When_No_Validator_Throws()
        {
            //Arrange
            var input = CreateInput();
            _autoMocker.GetMock<IValidationRule>().Setup(x => x.Validate(It.IsAny<ZipArchiveDTO>(), It.IsAny<CancellationToken>()));

            //Act
            await _service.ValidateFile(input, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<IValidationRule>().Verify(x => x.Validate(It.IsAny<ZipArchiveDTO>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
        }

        private ZipFileEntity CreateInput() => new ZipFileEntity(_fixture.Create<string>(), ZipFileFixtureGetter.GetAnyFile());
    }
}
