using Application.UseCases.ListZipStructureFiles.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using System.Net;
using System.Text.Json;
using WebApi;
using WebApi.Filters;

namespace UnitTests.Controllers
{
    public class GlobalExceptionFilterTests
    {
        private WebApplicationFactory<Program> _webApp;
        private AutoMocker _mocker;
        private HttpClient _client;

        private const string EndpointsToTestExceptions = "v1/zips";

        public GlobalExceptionFilterTests()
        {
            _mocker = new AutoMocker();
            _webApp = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_mocker.GetMock<IZipFileRepository>().Object);
                    services.AddSingleton(_mocker.GetMock<IListZipStructureFilesUseCase>().Object);
                });
            });

            _client = _webApp.CreateDefaultClient();
        }

        [Fact(DisplayName = "Should return Conflict when any usecase throws FileAlreadyExistsException")]
        public async Task Should_Return_Conflict_When_UseCase_Throws_FileAlreadyExistsException()
        {
            //Arrange
            _mocker.GetMock<IListZipStructureFilesUseCase>()
                .Setup(x => x.GetFilesFolderStructuresAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new FileAlreadyExistsException("any-file-name.zip"));

            //Act
            var response = await _client.GetAsync(EndpointsToTestExceptions);
            var responseBody = await JsonSerializer.DeserializeAsync<GlobalErrorResponse>(await response.Content.ReadAsStreamAsync());
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            responseBody.Should().BeOfType(typeof(GlobalErrorResponse));
        }

        [Fact(DisplayName = "Should return BadRequest when any usecase throw InvalidFileStructureException")]
        public async Task Should_Return_BadRequest_When_UseCase_Throws_InvalidFileStructureException()
        {
            //Arrange
            _mocker.GetMock<IListZipStructureFilesUseCase>()
                .Setup(x => x.GetFilesFolderStructuresAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidFileStructureException());

            //Act
            var response = await _client.GetAsync(EndpointsToTestExceptions);
            var responseBody = await JsonSerializer.DeserializeAsync<GlobalErrorResponse>(await response.Content.ReadAsStreamAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeOfType(typeof(GlobalErrorResponse));
        }

        [Fact(DisplayName = "Should return BadRequest when any usecase throw InvalidFileFormatException")]
        public async Task Should_Return_BadRequest_When_UseCase_Throws_InvalidFileFormatException()
        {
            //Arrange
            _mocker.GetMock<IListZipStructureFilesUseCase>()
                .Setup(x => x.GetFilesFolderStructuresAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidFileFormatException());

            //Act
            var response = await _client.GetAsync(EndpointsToTestExceptions);
            var responseBody = await JsonSerializer.DeserializeAsync<GlobalErrorResponse>(await response.Content.ReadAsStreamAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeOfType(typeof(GlobalErrorResponse));
        }

        [Fact(DisplayName = "Should return BadRequest when any usecase throw ValidationException")]
        public async Task Should_Return_BadRequest_When_UseCase_Throws_ValidationException()
        {
            //Arrange
            _mocker.GetMock<IListZipStructureFilesUseCase>()
                .Setup(x => x.GetFilesFolderStructuresAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new ValidationException("error"));

            //Act
            var response = await _client.GetAsync(EndpointsToTestExceptions);
            var responseBody = await JsonSerializer.DeserializeAsync<GlobalErrorResponse>(await response.Content.ReadAsStreamAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeOfType(typeof(GlobalErrorResponse));
        }

        [Fact(DisplayName = "Should return NotFound when any usecase throw FileNotFoundException")]
        public async Task Should_Return_BadRequest_When_UseCase_Throws_FileNotFoundException()
        {
            //Arrange
            _mocker.GetMock<IListZipStructureFilesUseCase>()
                .Setup(x => x.GetFilesFolderStructuresAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new FileNotFoundException("file"));

            //Act
            var response = await _client.GetAsync(EndpointsToTestExceptions);
            var responseBody = await JsonSerializer.DeserializeAsync<GlobalErrorResponse>(await response.Content.ReadAsStreamAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody.Should().BeOfType(typeof(GlobalErrorResponse));
        }

        [Fact(DisplayName = "Should return InternalServerError when any usecase throw any other Exception")]
        public async Task Should_Return_BadRequest_When_UseCase_Throws_Generic_Exception()
        {
            //Arrange
            _mocker.GetMock<IListZipStructureFilesUseCase>()
                .Setup(x => x.GetFilesFolderStructuresAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("error"));

            //Act
            var response = await _client.GetAsync(EndpointsToTestExceptions);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
