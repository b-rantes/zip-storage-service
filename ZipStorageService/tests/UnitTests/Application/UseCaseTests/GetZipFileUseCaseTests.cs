using Application.UseCases.GetZipFile;
using Application.UseCases.GetZipFile.Interfaces;
using Domain.Interfaces.Repository;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using UnitTests.Helpers;

namespace UnitTests.Application.UseCaseTests
{
    public class GetZipFileUseCaseTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly IGetZipFileUseCase _useCase;

        public GetZipFileUseCaseTests()
        {
            _autoMocker = new AutoMocker();
            _useCase = _autoMocker.CreateInstance<GetZipFileUseCase>();
        }

        [Fact(DisplayName = "Should get file successfully when file exists")]
        public async Task Should_Get_Successfully_When_Exists()
        {
            //Arrange
            var fileName = "any_file_name";
            var mockedFile = ZipFileFixtureGetter.GetAnyFile();
            _autoMocker.GetMock<IZipFileRepository>().Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((FileStream)mockedFile);

            //Act
            var file = await _useCase.GetZipFileAsync(fileName, CancellationToken.None);

            //Assert
            file.Should().NotBeNull();
        }

        [Fact(DisplayName = "Should return null when file dont exist")]
        public async Task Should_Return_Null_When_File_Dont_Exist()
        {
            //Arrange
            var fileName = "any_file_name";
            _autoMocker.GetMock<IZipFileRepository>().Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((FileStream?)null);

            //Act
            var file = await _useCase.GetZipFileAsync(fileName, CancellationToken.None);

            //Assert
            file.Should().BeNull();
        }
    }
}
