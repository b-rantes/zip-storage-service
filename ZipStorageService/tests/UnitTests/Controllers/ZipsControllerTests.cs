using AutoFixture;
using Domain.Interfaces.Repository;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using UnitTests.Helpers;
using WebApi;

namespace UnitTests.Controllers
{
    public class ZipsControllerTests
    {
        private WebApplicationFactory<Program> _webApp;
        private AutoMocker _mocker;
        private Fixture _fixture;
        private HttpClient _client;
        private JsonSerializerOptions _options;

        public ZipsControllerTests()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _mocker = new AutoMocker();
            _fixture = new Fixture();
            _webApp = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_mocker.GetMock<IZipFileRepository>().Object);
                });
            });

            _client = _webApp.CreateDefaultClient();
        }

        [Fact(DisplayName = "List files folder structure successfully")]
        public async Task Should_Get_All_Files_Folder_Structure_Successfully()
        {
            //Arrange
            var mockedRepositoryResponse = _fixture.Create<List<FolderDTO>>();
            _mocker.GetMock<IZipFileRepository>().Setup(x => x.GetAllFilesStructure(It.IsAny<CancellationToken>())).ReturnsAsync(mockedRepositoryResponse);

            //Act
            var result = await _client.GetAsync("v1/zips");
            var responseBody = await JsonSerializer.DeserializeAsync<List<FolderDTO>>(await result.Content.ReadAsStreamAsync(), _options);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody.Should().BeEquivalentTo(mockedRepositoryResponse);
        }

        [Fact(DisplayName = "Get file should get successfully")]
        public async Task Should_Get_File_Successfully()
        {
            //Arrange
            var mockedRepositoryResponse = ZipFileFixtureGetter.GetAnyFile();
            _mocker.GetMock<IZipFileRepository>().Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((FileStream?)mockedRepositoryResponse);

            //Act
            var result = await _client.GetAsync("v1/zips/any-file-name.zip");

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Get file should return BadRequest if invalid zip file name")]
        public async Task Should_Get_File_Return_Bad_Request_When_Invalid_Zip_Name()
        {
            //Arrange
            var mockedRepositoryResponse = ZipFileFixtureGetter.GetAnyFile();
            _mocker.GetMock<IZipFileRepository>().Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((FileStream?)mockedRepositoryResponse);

            //Act
            var result = await _client.GetAsync("v1/zips/any-file-name");

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Get file should return NoContent if no zip file found")]
        public async Task Should_Get_File_Return_NoContent_When_No_Zip_Found()
        {
            //Arrange
            var mockedRepositoryResponse = ZipFileFixtureGetter.GetAnyFile();
            _mocker.GetMock<IZipFileRepository>().Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((FileStream?)null);

            //Act
            var result = await _client.GetAsync("v1/zips/any-file-name.zip");

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Upload file should execute successfully")]
        public async Task Should_Upload_Zip_File_Successfully()
        {
            //Arrange
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var mockedFileFullPath = ZipFileFixtureGetter.GetFullPath(ZipFileReferenceConstants.ValidZipFile);

            MultipartFormDataContent content = new MultipartFormDataContent();
            ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(mockedFileFullPath));

            var fileName = ZipFileReferenceConstants.ValidZipFile.Split("/").Last();

            content.Add(fileContent, "file", fileName);

            _mocker.GetMock<IZipFileRepository>().Setup(x => x.SaveFile(It.IsAny<ZipFileEntity>(), It.IsAny<CancellationToken>()));

            //Act
            var response = await _client.PostAsync("v1/zips", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Upload file should throw and return BadRequest when invalid file name")]
        public async Task Should_Throw_When_Upload_Zip_File_With_Invalid_Name()
        {
            //Arrange
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var mockedFileFullPath = ZipFileFixtureGetter.GetFullPath(ZipFileReferenceConstants.ValidZipFile);

            var fileName = ZipFileReferenceConstants.ValidZipFile.Split("/").Last();

            MultipartFormDataContent content = new MultipartFormDataContent();
            ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(mockedFileFullPath));

            content.Add(fileContent, "file", Path.GetFileNameWithoutExtension(fileName));

            _mocker.GetMock<IZipFileRepository>().Setup(x => x.SaveFile(It.IsAny<ZipFileEntity>(), It.IsAny<CancellationToken>()));

            //Act
            var response = await _client.PostAsync("v1/zips", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Validate file should return Ok if file is valid")]
        public async Task Should_Validate_File_Successfully_When_File_Valid()
        {
            //Arrange
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var mockedFileFullPath = ZipFileFixtureGetter.GetFullPath(ZipFileReferenceConstants.ValidZipFile);

            var fileName = ZipFileReferenceConstants.ValidZipFile.Split("/").Last();

            MultipartFormDataContent content = new MultipartFormDataContent();
            ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(mockedFileFullPath));

            content.Add(fileContent, "file", Path.GetFileName(fileName));

            //Act
            var response = await _client.PostAsync("v1/zips/validate", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Validate file should return BadRequest if file is invalid")]
        public async Task Should_Validate_File_WithError_When_File_Invalid()
        {
            //Arrange
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var mockedFileFullPath = ZipFileFixtureGetter.GetFullPath(ZipFileReferenceConstants.InvalidZipFileDllName);

            var fileName = ZipFileReferenceConstants.InvalidZipFileDllName.Split("/").Last();

            MultipartFormDataContent content = new MultipartFormDataContent();
            ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(mockedFileFullPath));

            content.Add(fileContent, "file", Path.GetFileName(fileName));

            //Act
            var response = await _client.PostAsync("v1/zips/validate", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
