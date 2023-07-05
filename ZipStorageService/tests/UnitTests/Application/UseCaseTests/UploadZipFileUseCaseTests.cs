using Application.UseCases.UploadZipFile;
using Application.UseCases.UploadZipFile.Interfaces;
using Application.UseCases.UploadZipFile.Models;
using AutoFixture;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Models.Entities;
using Moq;
using Moq.AutoMock;
using UnitTests.Helpers;

namespace UnitTests.Application.UseCaseTests
{
    public class UploadZipFileUseCaseTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly IUploadZipFileUseCase _useCase;
        private Mock<IZipFileRepository> _repository;
        private Mock<IZipFileService> _zipFileService;

        private readonly Fixture _fixture;

        public UploadZipFileUseCaseTests()
        {
            _fixture = new Fixture();
            _autoMocker = new AutoMocker();
            _useCase = _autoMocker.CreateInstance<UploadZipFileUseCase>();
            _repository = _autoMocker.GetMock<IZipFileRepository>();
            _zipFileService = _autoMocker.GetMock<IZipFileService>();
        }

        [Fact(DisplayName = "Should throw exception when uploading a file that already exists")]
        public async Task Should_ThrowException_When_Uploading_File_That_Already_Exists()
        {
            //Arrange
            var input = CreateInput();

            _repository.Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns((FileStream)input.FileStream);

            //Act, Assert
            await Assert.ThrowsAsync<FileAlreadyExistsException>(
                async () => await _useCase.UploadZipFileAsync(input, CancellationToken.None));
            _repository.Verify(x => x.SaveFile(It.Is<ZipFileEntity>(y => y.FileName == input.FileName), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact(DisplayName = "Should throw exception when validate file throws exception")]
        public async Task Should_Throw_Exception_When_File_Dll_Invalid()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.InvalidZipFileDllName);

            _repository.Setup(x => x.GetFile(input.FileName, It.IsAny<CancellationToken>())).Returns((FileStream?)null);
            _zipFileService
                .Setup(x => x.ValidateFile(
                    It.Is<ZipFileEntity>(y => y.FileName == input.FileName),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.UploadZipFileAsync(input, CancellationToken.None));
            _repository.Verify(x => x.SaveFile(It.Is<ZipFileEntity>(y => y.FileName == input.FileName), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact(DisplayName = "Should save new file successfully")]
        public async Task Should_Save_NewFile_Successfully()
        {
            //Arrange
            var input = CreateInput(ZipFileReferenceConstants.ValidZipFile);

            _repository.Setup(x => x.GetFile(input.FileName, It.IsAny<CancellationToken>())).Returns((FileStream?)null);
            _zipFileService
                .Setup(x => x.ValidateFile(
                    It.Is<ZipFileEntity>(y => y.FileName == input.FileName),
                    It.IsAny<CancellationToken>()));

            //Act
            await _useCase.UploadZipFileAsync(input, CancellationToken.None);

            //Assert
            _repository.Verify(x => x.SaveFile(It.Is<ZipFileEntity>(y => y.FileName == input.FileName), It.IsAny<CancellationToken>()), Times.Once());
        }

        private UploadZipFileInput CreateInput() =>
            _fixture
            .Build<UploadZipFileInput>()
            .With(x => x.FileStream, ZipFileFixtureGetter.GetAnyFile())
            .Create();

        private UploadZipFileInput CreateInput(string fileName)
        {
            var file = ZipFileFixtureGetter.GetZipFile(fileName);
            var input = new UploadZipFileInput
            {
                FileName = fileName,
                FileStream = file
            };
            return input;
        }
    }
}
