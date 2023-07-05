using Domain.Interfaces.Repository;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using System.IO.Compression;

namespace Infrastructure.Repositories.ZipFileRepository
{
    public class InMemoryZipFileRepository : IZipFileRepository
    {
        private string StoragePathReference = "";

        private const string FolderToLookFor = "zips";

        private const string DllPath = "dlls/";
        private const string ImagesPath = "images/";
        private const string LanguagesPath = "languages/";

        public InMemoryZipFileRepository()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectories = Directory.GetParent(currentDirectory);

            var referenceDirectoryNotFound = true;

            while (referenceDirectoryNotFound)
            {
                if (parentDirectories is null || !parentDirectories.Exists)
                {
                    throw new Exception("In memory ZipFileRepository could not locate 'zips' folder");
                }

                var directories = parentDirectories.GetDirectories();

                if (parentDirectories.Exists)

                    if (IsZipsFolderFound(directories))
                    {
                        referenceDirectoryNotFound = false;

                        StoragePathReference = directories.First(x => x.Name == FolderToLookFor).FullName;
                    }

                parentDirectories = parentDirectories.Parent;
            }
        }

        public async Task SaveFile(ZipFileEntity zipFile, CancellationToken cancellationToken)
        {
            var path = Path.Combine(StoragePathReference, zipFile.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                zipFile.FileStream.Seek(0, SeekOrigin.Begin);
                await zipFile.FileStream.CopyToAsync(stream);
                await stream.FlushAsync();
            }
        }

        public FileStream? GetFile(string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var filePath = Path.Combine(StoragePathReference, fileName);

                var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                return file;
            }
            catch (Exception ex)
                when (
                    ex is FileNotFoundException ||
                    ex is DirectoryNotFoundException
                    )
            {
                return null;
            }
        }

        public Task<List<FolderDTO>> GetAllFilesStructure(CancellationToken cancellationToken)
        {
            var listOfFiles = new List<FolderDTO>();

            var filesInZipsFolder = Directory.GetFiles(StoragePathReference);

            foreach (var zipFilePath in filesInZipsFolder)
            {
                var rootFolder = new FolderDTO
                {
                    RootFolderName = Path.GetFileName(zipFilePath),
                    DllsFolder = new List<string>(),
                    ImagesFolder = new List<string>(),
                    LanguagesFolder = new List<string>()
                };

                ReadFilesInZipSubFolders(zipFilePath, rootFolder);

                listOfFiles.Add(rootFolder);
            }

            return Task.FromResult(listOfFiles);
        }

        public Task DeleteFile(string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var filePath = Path.Combine(StoragePathReference, fileName);

                if (!File.Exists(filePath))
                {
                    return Task.FromException(new FileNotFoundException());
                }

                File.Delete(filePath);

                return Task.CompletedTask;
            }
            catch (Exception ex) when (
                ex is DirectoryNotFoundException)
            {
                return Task.FromException(new FileNotFoundException(fileName));
            }
        }

        private static void ReadFilesInZipSubFolders(string zipPath, FolderDTO parentFolder)
        {
            ZipArchive zipArchive;
            using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(0, SeekOrigin.Begin);

                zipArchive = new ZipArchive(fs);

                var entriesWithoutEmptyEntry = zipArchive.Entries.Where(x => x.Length > 0).ToList();

                foreach (var entry in entriesWithoutEmptyEntry)
                {
                    var file = entry.FullName.Split("/").Last();

                    if (IsFromPath(entry, DllPath))
                    {
                        parentFolder.DllsFolder.Add(file);
                    }

                    if (IsFromPath(entry, ImagesPath))
                    {
                        parentFolder.ImagesFolder.Add(file);
                    }

                    if (IsFromPath(entry, LanguagesPath))
                    {
                        parentFolder.LanguagesFolder.Add(file);
                    }
                }
            }
        }

        private static bool IsZipsFolderFound(DirectoryInfo[] directories)
        {
            return directories.Any(x => x.Name == FolderToLookFor);
        }

        private static bool IsFromPath(ZipArchiveEntry entry, string path)
        {
            return entry.FullName.StartsWith(path) && entry.FullName.Length > 0;
        }
    }
}
