using Application.UseCases.DeleteZipFile;
using Application.UseCases.DeleteZipFile.Interfaces;
using Application.UseCases.DeleteZipFile.Models;
using Application.UseCases.DeleteZipFile.Validator;
using AutoFixture;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using FluentValidation;
using Moq;
using Moq.AutoMock;

namespace UnitTests.Application.UseCaseTests
{
    public class DeleteZipFileUseCaseTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly IDeleteZipFileUseCase _useCase;

        public DeleteZipFileUseCaseTests()
        {
            _autoMocker = new AutoMocker();
            _autoMocker.Use<IValidator<DeleteZipFileInput>>(new DeleteZipFileInputValidator());
            _useCase = _autoMocker.CreateInstance<DeleteZipFileUseCase>();
        }

        [Fact(DisplayName = "Should throw when deleting invalid zip file name")]
        public async Task Should_Throw_When_Invalid_FileName()
        {
            //Arrange
            var input = CreateInvalidInput();

            //Act, Assert
            await Assert.ThrowsAsync<InvalidFileFormatException>(async () => await _useCase.DeleteZipFileAsync(input, CancellationToken.None));
        }

        [Fact(DisplayName = "Should execute successfully when valid zip file name")]
        public async Task Should_Execute_Successfully()
        {
            //Arrange
            var input = CreateValidInput();

            _autoMocker.GetMock<IZipFileRepository>().Setup(x => x.DeleteFile(input.FileName, It.IsAny<CancellationToken>()));

            //Act
            await _useCase.DeleteZipFileAsync(input, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<IZipFileRepository>().Verify(x => x.DeleteFile(input.FileName, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact(DisplayName = "Should throw when repository throws")]
        public async Task Should_Propagate_Repository_Exception()
        {
            //Arrange
            var input = CreateValidInput();

            _autoMocker.GetMock<IZipFileRepository>().Setup(x => x.DeleteFile(input.FileName, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            //Act, Assert
            await Assert.ThrowsAsync<Exception>(async () => await _useCase.DeleteZipFileAsync(input, CancellationToken.None));
        }

        private DeleteZipFileInput CreateInvalidInput() => new()
        {
            FileName = "any_input_file_name"
        };

        private DeleteZipFileInput CreateValidInput() => new() { FileName = $"{CreateInvalidInput().FileName}.zip" };
    }
}
