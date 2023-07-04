using Application.UseCases.UploadZipFile.Models;
using Application.UseCases.ValidateZipFile;
using Application.UseCases.ValidateZipFile.Interfaces;
using Application.UseCases.ValidateZipFile.Models;
using AutoFixture;
using Domain.Interfaces.Services;
using Domain.Models.Entities;
using Moq;
using Moq.AutoMock;
using UnitTests.Helpers;

namespace UnitTests.Application.UseCaseTests
{
    public class ValidateZipFileUseCaseTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly Fixture _fixture;
        private readonly IValidateZipFileUseCase _useCase;

        public ValidateZipFileUseCaseTests()
        {
            _fixture = new Fixture();
            _autoMocker = new AutoMocker();
            _useCase = _autoMocker.CreateInstance<ValidateZipFileUseCase>();
        }

        [Fact(DisplayName = "Should throw exception when validation service throws (validation fails)")]
        public async Task Should_Throw_When_Service_Throws()
        {
            //Arrage
            var input = CreateInput();
            _autoMocker.GetMock<IZipFileService>().Setup(x => x.ValidateFile(It.IsAny<ZipFileEntity>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.ValidateZipFileAsync(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should execute successfully when service dont throw")]
        public async Task Should_Execute_Successfully()
        {
            //Arrage
            var input = CreateInput();
            _autoMocker.GetMock<IZipFileService>().Setup(x => x.ValidateFile(It.IsAny<ZipFileEntity>(), It.IsAny<CancellationToken>()));

            //Act
            await _useCase.ValidateZipFileAsync(input, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<IZipFileService>().Verify(x => x.ValidateFile(It.IsAny<ZipFileEntity>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        private ValidateZipFileInput CreateInput() =>
            _fixture
            .Build<ValidateZipFileInput>()
            .With(x => x.FileStream, ZipFileFixtureGetter.GetAnyFile())
            .Create();
    }
}
