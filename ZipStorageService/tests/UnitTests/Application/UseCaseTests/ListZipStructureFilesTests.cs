using Application.UseCases.ListZipStructureFiles;
using Application.UseCases.ListZipStructureFiles.Interfaces;
using AutoFixture;
using Domain.Interfaces.Repository;
using Domain.Models.DTOs;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace UnitTests.Application.UseCaseTests
{
    public class ListZipStructureFilesTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly Fixture _fixture;
        private readonly IListZipStructureFilesUseCase _useCase;

        public ListZipStructureFilesTests()
        {
            _fixture = new Fixture();
            _autoMocker = new AutoMocker();
            _useCase = _autoMocker.CreateInstance<ListZipStructureFilesUseCase>();
        }

        [Fact(DisplayName = "Should throw exception when repository throws")]
        public async Task Should_Throw_When_Repository_Throws()
        {
            //Arrange
            _autoMocker.GetMock<IZipFileRepository>().Setup(x => x.GetAllFilesStructure(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.GetFilesFolderStructuresAsync(CancellationToken.None));
        }

        [Fact(DisplayName = "Should get folders structure correctly")]
        public async Task Should_Get_Folder_Structure_Correctly()
        {
            //Arrange
            var mockedFilesList = _fixture.Create<List<FolderDTO>>();
            _autoMocker.GetMock<IZipFileRepository>().Setup(x => x.GetAllFilesStructure(It.IsAny<CancellationToken>())).ReturnsAsync(mockedFilesList);

            //Act
            var listOfFiles = await _useCase.GetFilesFolderStructuresAsync(CancellationToken.None);
            
            //Assert
            listOfFiles.Should().BeEquivalentTo(mockedFilesList);
        }
    }
}
